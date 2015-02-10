
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using Exceptions;
    using Host;
    using Infra;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Util;

    internal sealed class AppSystemSet : IAppSystemSet, IMemorySet
    {
        public static readonly IAppSystemSet Empty = new AppSystemSet(EmptyAcDomain.SingleInstance);
        private static readonly object Locker = new object();

        private readonly Dictionary<string, AppSystemState> _dicByCode = new Dictionary<string, AppSystemState>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Guid, AppSystemState> _dicById = new Dictionary<Guid, AppSystemState>();
        private bool _initialized;
        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;

        public Guid Id
        {
            get { return _id; }
        }

        internal AppSystemSet(IAcDomain acDomain)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (acDomain.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            _acDomain = acDomain;
            new MessageHandler(this).Register();
        }

        public AppSystemState SelfAppSystem
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (string.IsNullOrEmpty(_acDomain.Config.SelfAppSystemCode))
                {
                    throw new AnycmdException("尚未配置SelfAppSystemCode，在AcDomain初始化时由传入的IAppConfig对象配置。");
                }
                else if (!_dicByCode.ContainsKey(_acDomain.Config.SelfAppSystemCode))
                {
                    throw new AnycmdException("意外的SelfAppSystemCode：" + _acDomain.Config.SelfAppSystemCode);
                }
                else
                {
                    return _dicByCode[_acDomain.Config.SelfAppSystemCode];
                }
            }
        }

        public bool TryGetAppSystem(string appSystemCode, out AppSystemState appSystem)
        {
            if (!_initialized)
            {
                Init();
            }
            if (string.IsNullOrEmpty(appSystemCode))
            {
                throw new ArgumentNullException("appSystemCode");
            }

            return _dicByCode.TryGetValue(appSystemCode, out appSystem);
        }

        public bool TryGetAppSystem(Guid appSystemId, out AppSystemState appSystem)
        {
            if (!_initialized)
            {
                Init();
            }
            if (appSystemId == Guid.Empty)
            {
                throw new ArgumentException("传入的appSystemId不应为Guid.Empty。");
            }

            return _dicById.TryGetValue(appSystemId, out appSystem);
        }

        public bool ContainsAppSystem(Guid appSystemId)
        {
            if (!_initialized)
            {
                Init();
            }
            if (appSystemId == Guid.Empty)
            {
                throw new ArgumentException("传入的appSystemId不应为Guid.Empty。");
            }

            return _dicById.ContainsKey(appSystemId);
        }

        public bool ContainsAppSystem(string appSystemCode)
        {
            if (!_initialized)
            {
                Init();
            }
            if (string.IsNullOrEmpty(appSystemCode))
            {
                throw new ArgumentNullException("appSystemCode");
            }

            return _dicByCode.ContainsKey(appSystemCode);
        }

        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        public IEnumerator<AppSystemState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }

            return _dicByCode.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }

            return _dicByCode.Values.GetEnumerator();
        }

        #region Init
        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dicByCode.Clear();
                _dicById.Clear();
                var appSystems = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllAppSystems();
                foreach (var appSystem in appSystems)
                {
                    Debug.Assert(appSystem != null, "appSystem != null");
                    if (_dicByCode.ContainsKey(appSystem.Code))
                    {
                        throw new AnycmdException("意外重复的应用系统编码" + appSystem.Code);
                    }
                    if (_dicById.ContainsKey(appSystem.Id))
                    {
                        throw new AnycmdException("意外重复的应用系统标识" + appSystem.Id);
                    }
                    var value = AppSystemState.Create(_acDomain, appSystem);
                    _dicByCode.Add(appSystem.Code, value);
                    _dicById.Add(appSystem.Id, value);
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #endregion

        #region MessageHandler
        private class MessageHandler :
            IHandler<AppSystemUpdatedEvent>,
            IHandler<AppSystemRemovedEvent>,
            IHandler<AddAppSystemCommand>,
            IHandler<AppSystemAddedEvent>,
            IHandler<UpdateAppSystemCommand>,
            IHandler<RemoveAppSystemCommand>
        {
            private static readonly object MessageLocker = new object();
            private readonly AppSystemSet _set;

            internal MessageHandler(AppSystemSet set)
            {
                _set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._acDomain.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("AcDomain对象'{0}'尚未设置MessageDispatcher。".Fmt(_set._acDomain.Name));
                }
                messageDispatcher.Register((IHandler<AddAppSystemCommand>)this);
                messageDispatcher.Register((IHandler<AppSystemAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateAppSystemCommand>)this);
                messageDispatcher.Register((IHandler<AppSystemUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveAppSystemCommand>)this);
                messageDispatcher.Register((IHandler<AppSystemRemovedEvent>)this);
            }

            public void Handle(AddAppSystemCommand message)
            {
                Handle(message.AcSession, message.Input, true);
            }

            public void Handle(AppSystemAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateAppSystemAddedEvent))
                {
                    return;
                }
                Handle(message.AcSession, message.Input, false);
            }

            private void Handle(IAcSession acSession, IAppSystemCreateIo input, bool isCommand)
            {
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var acDomain = _set._acDomain;
                var repository = acDomain.RetrieveRequiredService<IRepository<AppSystem>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new AnycmdException("标识是必须的");
                }
                AppSystem entity;
                lock (MessageLocker)
                {
                    if (acDomain.AppSystemSet.ContainsAppSystem(input.Id.Value))
                    {
                        throw new AnycmdException("给定标识的记录已经存在" + input.Id);
                    }
                    if (acDomain.AppSystemSet.ContainsAppSystem(input.Code))
                    {
                        throw new ValidationException("重复的应用系统编码" + input.Code);
                    }
                    AccountState principal;
                    // TODO:考虑将AppSystem.PrincipalId重命名为AppSystem.DevPrincipalId，从而与业务负责人分开。
                    if (!acDomain.SysUserSet.TryGetDevAccount(input.PrincipalId, out principal))
                    {
                        throw new ValidationException("意外的应用系统负责人，业务系统负责人必须是开发人员");
                    }

                    entity = AppSystem.Create(input);

                    var state = AppSystemState.Create(acDomain, entity);
                    if (!dicByCode.ContainsKey(state.Code))
                    {
                        dicByCode.Add(state.Code, state);
                    }
                    if (!dicById.ContainsKey(state.Id))
                    {
                        dicById.Add(state.Id, state);
                    }
                    // 如果是命令则持久化
                    if (isCommand)
                    {
                        try
                        {
                            repository.Add(entity);
                            repository.Context.Commit();
                        }
                        catch
                        {
                            if (dicByCode.ContainsKey(entity.Code))
                            {
                                dicByCode.Remove(entity.Code);
                            }
                            if (dicById.ContainsKey(entity.Id))
                            {
                                dicById.Remove(entity.Id);
                            }
                            repository.Context.Rollback();
                            throw;
                        }
                    }
                }
                // 如果是命令则分发事件
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateAppSystemAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateAppSystemAddedEvent : AppSystemAddedEvent, IPrivateEvent
            {
                internal PrivateAppSystemAddedEvent(IAcSession acSession, AppSystemBase source, IAppSystemCreateIo input)
                    : base(acSession, source, input)
                {
                }
            }

            public void Handle(UpdateAppSystemCommand message)
            {
                Handle(message.AcSession, message.Input, true);
            }

            public void Handle(AppSystemUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateAppSystemUpdatedEvent))
                {
                    return;
                }
                Handle(message.AcSession, message.Input, false);
            }

            private void Handle(IAcSession acSession, IAppSystemUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var repository = acDomain.RetrieveRequiredService<IRepository<AppSystem>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                AppSystemState bkState;
                if (!acDomain.AppSystemSet.TryGetAppSystem(input.Id, out bkState))
                {
                    throw new NotExistException("意外的应用系统标识" + input.Id);
                }
                AppSystem entity;
                var stateChanged = false;
                lock (MessageLocker)
                {
                    AppSystemState oldState;
                    if (!acDomain.AppSystemSet.TryGetAppSystem(input.Id, out oldState))
                    {
                        throw new NotExistException("意外的应用系统标识" + input.Id);
                    }
                    AppSystemState outAppSystem;
                    if (acDomain.AppSystemSet.TryGetAppSystem(input.Code, out outAppSystem) && outAppSystem.Id != input.Id)
                    {
                        throw new ValidationException("重复的应用系统编码" + input.Code);
                    }
                    entity = repository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = AppSystemState.Create(acDomain, entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            repository.Update(entity);
                            repository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            repository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateAppSystemUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(AppSystemState state)
            {
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var oldState = dicById[state.Id];
                var oldKey = oldState.Code;
                var newKey = state.Code;
                dicById[state.Id] = state;
                // 如果应用系统编码改变了
                if (!dicByCode.ContainsKey(newKey))
                {
                    dicByCode.Remove(oldKey);
                    dicByCode.Add(newKey, dicById[state.Id]);
                }
                else
                {
                    dicByCode[oldKey] = state;
                }
            }

            private class PrivateAppSystemUpdatedEvent : AppSystemUpdatedEvent, IPrivateEvent
            {
                internal PrivateAppSystemUpdatedEvent(IAcSession acSession, AppSystemBase source, IAppSystemUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveAppSystemCommand message)
            {
                Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(AppSystemRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateAppSystemRemovedEvent))
                {
                    return;
                }
                Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid appSystemId, bool isCommand)
            {
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var acDomain = _set._acDomain;
                var repository = acDomain.RetrieveRequiredService<IRepository<AppSystem>>();
                AppSystemState bkState;
                if (!acDomain.AppSystemSet.TryGetAppSystem(appSystemId, out bkState))
                {
                    return;
                }
                if (acDomain.CatalogSet.Any(a => a.Code.StartsWith(bkState.Code)))
                {
                    throw new ValidationException("应用系统下有资源类型时不能删除应用系统。");
                }
                if (acDomain.MenuSet.Any(a => a.AppSystemId == appSystemId))
                {
                    throw new ValidationException("应用系统下有菜单时不能删除应用系统");
                }
                AppSystem entity;
                lock (MessageLocker)
                {
                    AppSystemState state;
                    if (!acDomain.AppSystemSet.TryGetAppSystem(appSystemId, out state))
                    {
                        return;
                    }
                    entity = repository.GetByKey(appSystemId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (dicById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            acDomain.MessageDispatcher.DispatchMessage(new AppSystemRemovingEvent(acSession, entity));
                        }
                        if (dicByCode.ContainsKey(bkState.Code))
                        {
                            dicByCode.Remove(bkState.Code);
                        }
                        dicById.Remove(bkState.Id);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            repository.Remove(entity);
                            repository.Context.Commit();
                        }
                        catch
                        {
                            if (!dicById.ContainsKey(bkState.Id))
                            {
                                dicById.Add(bkState.Id, bkState);
                            }
                            if (!dicByCode.ContainsKey(bkState.Code))
                            {
                                dicByCode.Add(bkState.Code, bkState);
                            }
                            repository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateAppSystemRemovedEvent(acSession, entity));
                }
            }

            private class PrivateAppSystemRemovedEvent : AppSystemRemovedEvent, IPrivateEvent
            {
                internal PrivateAppSystemRemovedEvent(IAcSession acSession, AppSystemBase source) : base(acSession, source) { }
            }
        }
        #endregion
    }
}
