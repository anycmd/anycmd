
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Abstractions.Infra;
    using Exceptions;
    using Extensions;
    using Host;
    using Infra;
    using Infra.Messages;
    using InOuts;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public sealed class AppSystemSet : IAppSystemSet
    {
        public static readonly IAppSystemSet Empty = new AppSystemSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<string, AppSystemState> _dicByCode = new Dictionary<string, AppSystemState>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Guid, AppSystemState> _dicById = new Dictionary<Guid, AppSystemState>();
        private bool _initialized = false;
        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        #region Ctor
        public AppSystemSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            new MessageHandler(this).Register();
        }
        #endregion

        public AppSystemState SelfAppSystem
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (_dicByCode.ContainsKey(_host.Config.SelfAppSystemCode))
                {
                    return _dicByCode[_host.Config.SelfAppSystemCode];
                }
                else
                {
                    throw new AnycmdException("尚未配置SelfAppSystemCode");
                }
            }
        }

        public bool TryGetAppSystem(string appSystemCode, out AppSystemState appSystem)
        {
            if (!_initialized)
            {
                Init();
            }
            if (appSystemCode == null)
            {
                appSystem = AppSystemState.Empty;
                return false;
            }
            return _dicByCode.TryGetValue(appSystemCode, out appSystem);
        }

        public bool TryGetAppSystem(Guid appSystemId, out AppSystemState appSystem)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.TryGetValue(appSystemId, out appSystem);
        }

        public bool ContainsAppSystem(Guid appSystemId)
        {
            if (!_initialized)
            {
                Init();
            }

            return _dicById.ContainsKey(appSystemId);
        }

        public bool ContainsAppSystem(string appSystemCode)
        {
            if (!_initialized)
            {
                Init();
            }
            if (appSystemCode == null)
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
            if (!_initialized)
            {
                lock (this)
                {
                    if (_initialized) return;
                    _dicByCode.Clear();
                    _dicById.Clear();
                    var appSystems = _host.GetRequiredService<IOriginalHostStateReader>().GetAllAppSystems();
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
                        var value = AppSystemState.Create(_host, appSystem);
                        _dicByCode.Add(appSystem.Code, value);
                        _dicById.Add(appSystem.Id, value);
                    }
                    _initialized = true;
                }
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
            private readonly AppSystemSet set;

            public MessageHandler(AppSystemSet set)
            {
                this.set = set;
            }

            public void Register()
            {
                var messageDispatcher = set._host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(set._host.Name));
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
                this.Handle(message.Input, true);
            }

            public void Handle(AppSystemAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateAppSystemAddedEvent))
                {
                    return;
                }
                this.Handle(message.Input, false);
            }

            private void Handle(IAppSystemCreateIo input, bool isCommand)
            {
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var host = set._host;
                var repository = host.GetRequiredService<IRepository<AppSystem>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                AppSystem entity;
                lock (this)
                {
                    if (host.AppSystemSet.ContainsAppSystem(input.Code))
                    {
                        throw new ValidationException("重复的应用系统编码" + input.Code);
                    }
                    if (!input.Id.HasValue || host.AppSystemSet.ContainsAppSystem(input.Id.Value))
                    {
                        throw new AnycmdException("意外的应用系统标识");
                    }
                    AccountState principal;
                    if (!host.SysUsers.TryGetDevAccount(input.PrincipalId, out principal))
                    {
                        throw new ValidationException("意外的应用系统负责人，业务系统负责人必须是开发人员");
                    }

                    entity = AppSystem.Create(input);

                    var state = AppSystemState.Create(host, entity);
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
                    host.MessageDispatcher.DispatchMessage(new PrivateAppSystemAddedEvent(entity, input));
                }
            }

            private class PrivateAppSystemAddedEvent : AppSystemAddedEvent
            {
                public PrivateAppSystemAddedEvent(AppSystemBase source, IAppSystemCreateIo input)
                    : base(source, input)
                {
                }
            }
            public void Handle(UpdateAppSystemCommand message)
            {
                this.Handle(message.Output, true);
            }

            public void Handle(AppSystemUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateAppSystemUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.Input, false);
            }

            private void Handle(IAppSystemUpdateIo input, bool isCommand)
            {
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var host = set._host;
                var repository = host.GetRequiredService<IRepository<AppSystem>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                AppSystemState bkState;
                if (!host.AppSystemSet.TryGetAppSystem(input.Id, out bkState))
                {
                    throw new NotExistException("意外的应用系统标识" + input.Id);
                }
                AppSystem entity;
                var stateChanged = false;
                lock (bkState)
                {
                    AppSystemState oldState;
                    if (!host.AppSystemSet.TryGetAppSystem(input.Id, out oldState))
                    {
                        throw new NotExistException("意外的应用系统标识" + input.Id);
                    }
                    AppSystemState outAppSystem;
                    if (host.AppSystemSet.TryGetAppSystem(input.Code, out outAppSystem) && outAppSystem.Id != input.Id)
                    {
                        throw new ValidationException("重复的应用系统编码" + input.Code);
                    }
                    entity = repository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = AppSystemState.Create(host, entity);
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
                    if (!stateChanged)
                    {
                        return;
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateAppSystemUpdatedEvent(entity, input));
                }
            }

            private void Update(AppSystemState state)
            {
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var host = set._host;
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

            private class PrivateAppSystemUpdatedEvent : AppSystemUpdatedEvent
            {
                public PrivateAppSystemUpdatedEvent(AppSystemBase source, IAppSystemUpdateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(RemoveAppSystemCommand message)
            {
                this.Handle(message.EntityId, true);
            }

            public void Handle(AppSystemRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateAppSystemRemovedEvent))
                {
                    return;
                }
                this.Handle(message.Source.Id, false);
            }

            private void Handle(Guid appSystemId, bool isCommand)
            {
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var host = set._host;
                var repository = host.GetRequiredService<IRepository<AppSystem>>();
                AppSystemState bkState;
                if (!host.AppSystemSet.TryGetAppSystem(appSystemId, out bkState))
                {
                    return;
                }
                if (host.ResourceTypeSet.Any(a => a.AppSystemId == appSystemId))
                {
                    throw new ValidationException("应用系统下有资源类型时不能删除应用系统。");
                }
                if (host.MenuSet.Any(a => a.AppSystemId == appSystemId))
                {
                    throw new ValidationException("应用系统下有菜单时不能删除应用系统");
                }
                AppSystem entity;
                lock (bkState)
                {
                    AppSystemState state;
                    if (!host.AppSystemSet.TryGetAppSystem(appSystemId, out state))
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
                            host.MessageDispatcher.DispatchMessage(new AppSystemRemovingEvent(entity));
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
                    host.MessageDispatcher.DispatchMessage(new PrivateAppSystemRemovedEvent(entity));
                }
            }

            private class PrivateAppSystemRemovedEvent : AppSystemRemovedEvent
            {
                public PrivateAppSystemRemovedEvent(AppSystemBase source) : base(source) { }
            }
        }
        #endregion
    }
}
