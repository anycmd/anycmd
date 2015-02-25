
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Ac.Functions;
    using Engine.Ac.Catalogs;
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
    using functionCode = System.String;

    internal sealed class FunctionSet : IFunctionSet, IMemorySet
    {
        public static readonly IFunctionSet Empty = new FunctionSet(EmptyAcDomain.SingleInstance);
        private static readonly object Locker = new object();

        private readonly Dictionary<CatalogState, Dictionary<functionCode, FunctionState>>
            _dicByCode = new Dictionary<CatalogState, Dictionary<functionCode, FunctionState>>();
        private readonly Dictionary<Guid, FunctionState> _dicById = new Dictionary<Guid, FunctionState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;

        public Guid Id
        {
            get { return _id; }
        }

        internal FunctionSet(IAcDomain acDomain)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            if (acDomain.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            this._acDomain = acDomain;
            new MessageHandler(this).Register();
        }

        public bool TryGetFunction(CatalogState resourceType, string functionCode, out FunctionState function)
        {
            if (!_initialized)
            {
                Init();
            }
            if (resourceType == null)
            {
                throw new ArgumentNullException("resourceType");
            }
            if (string.IsNullOrEmpty(functionCode))
            {
                throw new ArgumentNullException("functionCode");
            }
            if (_dicByCode.ContainsKey(resourceType))
                return _dicByCode[resourceType].TryGetValue(functionCode, out function);
            function = FunctionState.Empty;
            return false;
        }

        public bool TryGetFunction(Guid functionId, out FunctionState function)
        {
            if (!_initialized)
            {
                Init();
            }
            Debug.Assert(functionId != Guid.Empty);

            return _dicById.TryGetValue(functionId, out function);
        }

        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        public IEnumerator<FunctionState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.Values.GetEnumerator();
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
                var functions = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllFunctions();
                foreach (var entity in functions)
                {
                    var function = FunctionState.Create(_acDomain, entity);
                    _dicById.Add(function.Id, function);
                    if (!_dicByCode.ContainsKey(function.Resource))
                    {
                        _dicByCode.Add(function.Resource, new Dictionary<functionCode, FunctionState>(StringComparer.OrdinalIgnoreCase));
                    }
                    if (!_dicByCode[function.Resource].ContainsKey(function.Code))
                    {
                        _dicByCode[function.Resource].Add(function.Code, function);
                    }
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #endregion

        #region MessageHandler
        private class MessageHandler :
            IHandler<FunctionAddedEvent>,
            IHandler<FunctionRemovedEvent>,
            IHandler<AddFunctionCommand>, 
            IHandler<UpdateFunctionCommand>, 
            IHandler<FunctionUpdatedEvent>, 
            IHandler<RemoveFunctionCommand>,
            IHandler<CatalogUpdatedEvent>,
            IHandler<CatalogRemovedEvent>
        {
            private readonly FunctionSet _set;

            internal MessageHandler(FunctionSet set)
            {
                this._set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._acDomain.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("AcDomain对象'{0}'尚未设置MessageDispatcher。".Fmt(_set._acDomain.Name));
                }
                messageDispatcher.Register((IHandler<AddFunctionCommand>)this);
                messageDispatcher.Register((IHandler<FunctionAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateFunctionCommand>)this);
                messageDispatcher.Register((IHandler<FunctionUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveFunctionCommand>)this);
                messageDispatcher.Register((IHandler<FunctionRemovedEvent>)this);
                messageDispatcher.Register((IHandler<CatalogUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<CatalogRemovedEvent>)this);
            }

            public void Handle(CatalogUpdatedEvent message)
            {
                var acDomain = _set._acDomain;
                var dicByCode = _set._dicByCode;
                CatalogState newKey;
                if (!acDomain.CatalogSet.TryGetCatalog(message.Source.Id, out newKey))
                {
                    throw new AnycmdException("意外的资源标识" + message.Source.Id);
                }
                var oldKey = dicByCode.Keys.FirstOrDefault(a => a.Id == message.Source.Id);
                if (oldKey != null && !dicByCode.ContainsKey(newKey))
                {
                    dicByCode.Add(newKey, dicByCode[oldKey]);
                    dicByCode.Remove(oldKey);
                }
            }

            public void Handle(CatalogRemovedEvent message)
            {
                var acDomain = _set._acDomain;
                var dicByCode = _set._dicByCode;
                var key = dicByCode.Keys.FirstOrDefault(a => a.Id == message.Source.Id);
                if (key != null)
                {
                    dicByCode.Remove(key);
                }
            }

            public void Handle(AddFunctionCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(FunctionAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateFunctionAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IFunctionCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var functionRepository = acDomain.RetrieveRequiredService<IRepository<Function>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                CatalogState resource;
                if (!acDomain.CatalogSet.TryGetCatalog(input.ResourceTypeId, out resource))
                {
                    throw new ValidationException("意外的功能资源标识" + input.ResourceTypeId);
                }

                var entity = Function.Create(input);

                lock (Locker)
                {
                    FunctionState functionState;
                    if (acDomain.FunctionSet.TryGetFunction(input.Id.Value, out functionState))
                    {
                        throw new AnycmdException("记录已经存在");
                    }
                    var state = FunctionState.Create(acDomain, entity);
                    if (acDomain.FunctionSet.TryGetFunction(resource, input.Code, out functionState))
                    {
                        throw new ValidationException("重复的编码");
                    }
                    if (!dicById.ContainsKey(entity.Id))
                    {
                        dicById.Add(entity.Id, state);
                    }
                    if (!dicByCode.ContainsKey(resource))
                    {
                        dicByCode.Add(resource, new Dictionary<functionCode, FunctionState>(StringComparer.OrdinalIgnoreCase));
                    }
                    if (!dicByCode[resource].ContainsKey(entity.Code))
                    {
                        dicByCode[resource].Add(state.Code, state);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            functionRepository.Add(entity);
                            functionRepository.Context.Commit();
                        }
                        catch
                        {
                            if (dicById.ContainsKey(entity.Id))
                            {
                                dicById.Remove(entity.Id);
                            }
                            if (dicByCode.ContainsKey(resource) && dicByCode[resource].ContainsKey(entity.Code))
                            {
                                dicByCode[resource].Remove(entity.Code);
                            }
                            functionRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateFunctionAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateFunctionAddedEvent : FunctionAddedEvent, IPrivateEvent
            {
                public PrivateFunctionAddedEvent(IAcSession acSession, FunctionBase source, IFunctionCreateIo input) : base(acSession, source, input) { }
            }

            public void Handle(UpdateFunctionCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(FunctionUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateFunctionUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Input, false);
            }

            private void Handle(IAcSession acSession, IFunctionUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var functionRepository = acDomain.RetrieveRequiredService<IRepository<Function>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                FunctionState bkState;
                if (!acDomain.FunctionSet.TryGetFunction(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                CatalogState resource;
                if (!acDomain.CatalogSet.TryGetCatalog(bkState.ResourceTypeId, out resource))
                {
                    throw new ValidationException("意外的功能资源标识" + bkState.ResourceTypeId);
                }
                Function entity;
                bool stateChanged = false;
                lock (Locker)
                {
                    FunctionState oldState;
                    if (!acDomain.FunctionSet.TryGetFunction(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    FunctionState functionState;
                    if (acDomain.FunctionSet.TryGetFunction(resource, input.Code, out functionState) && functionState.Id != input.Id)
                    {
                        throw new ValidationException("重复的编码");
                    }
                    entity = functionRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException("更新的实体不存在");
                    }

                    entity.Update(input);

                    var newState = FunctionState.Create(acDomain, entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            functionRepository.Update(entity);
                            functionRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            functionRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateFunctionUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(FunctionState state)
            {
                var acDomain = _set._acDomain;
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var oldState = dicById[state.Id];
                string oldKey = oldState.Code;
                string newKey = state.Code;
                dicById[state.Id] = state;
                CatalogState resource;
                if (!acDomain.CatalogSet.TryGetCatalog(oldState.ResourceTypeId, out resource))
                {
                    throw new ValidationException("意外的功能资源标识" + oldState.ResourceTypeId);
                }
                if (!dicByCode[resource].ContainsKey(newKey))
                {
                    dicByCode[resource].Remove(oldKey);
                    dicByCode[resource].Add(newKey, state);
                }
                else
                {
                    dicByCode[resource][newKey] = state;
                }
            }

            private class PrivateFunctionUpdatedEvent : FunctionUpdatedEvent, IPrivateEvent
            {
                internal PrivateFunctionUpdatedEvent(IAcSession acSession, FunctionBase source, IFunctionUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveFunctionCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(FunctionRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateFunctionRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid functionId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var dicByCode = _set._dicByCode;
                var dicById = _set._dicById;
                var functionRepository = acDomain.RetrieveRequiredService<IRepository<Function>>();
                var operationHelpRepository = acDomain.RetrieveRequiredService<IRepository<OperationHelp>>();

                FunctionState bkState;
                if (!acDomain.FunctionSet.TryGetFunction(functionId, out bkState))
                {
                    return;
                }
                Function entity;
                lock (Locker)
                {
                    FunctionState state;
                    if (!acDomain.FunctionSet.TryGetFunction(functionId, out state))
                    {
                        return;
                    }
                    entity = functionRepository.GetByKey(functionId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (dicById.ContainsKey(functionId))
                    {
                        if (isCommand)
                        {
                            acDomain.MessageDispatcher.DispatchMessage(new FunctionRemovingEvent(acSession, entity));
                        }
                        if (dicByCode.ContainsKey(bkState.Resource)
                            && dicByCode[bkState.Resource].ContainsKey(bkState.Code))
                        {
                            dicByCode[bkState.Resource].Remove(bkState.Code);
                        }
                        dicById.Remove(functionId);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            // 删除操作帮助
                            var operationLog = operationHelpRepository.GetByKey(functionId);
                            if (operationLog != null)
                            {
                                operationHelpRepository.Remove(operationLog);
                                operationHelpRepository.Context.Commit();
                            }
                            functionRepository.Remove(entity);
                            functionRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!dicById.ContainsKey(functionId))
                            {
                                if (!dicByCode.ContainsKey(bkState.Resource))
                                {
                                    dicByCode.Add(bkState.Resource, new Dictionary<functionCode, FunctionState>(StringComparer.OrdinalIgnoreCase));
                                }
                                if (!dicByCode[bkState.Resource].ContainsKey(bkState.Code))
                                {
                                    dicByCode[bkState.Resource].Add(bkState.Code, bkState);
                                }
                                dicById.Add(bkState.Id, bkState);
                            }
                            operationHelpRepository.Context.Rollback();
                            functionRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateFunctionRemovedEvent(acSession, entity));
                }
            }

            private class PrivateFunctionRemovedEvent : FunctionRemovedEvent, IPrivateEvent
            {
                internal PrivateFunctionRemovedEvent(IAcSession acSession, FunctionBase function)
                    : base(acSession, function)
                {

                }
            }
        }
        #endregion
    }
}
