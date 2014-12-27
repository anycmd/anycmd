
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
    using System.Linq;
    using functionCode = System.String;

    public sealed class FunctionSet : IFunctionSet
    {
        public static readonly IFunctionSet Empty = new FunctionSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<ResourceTypeState, Dictionary<functionCode, FunctionState>>
            _dicByCode = new Dictionary<ResourceTypeState, Dictionary<functionCode, FunctionState>>();
        private readonly Dictionary<Guid, FunctionState> _dicById = new Dictionary<Guid, FunctionState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        public FunctionSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            new MessageHandler(this).Register();
        }

        public bool TryGetFunction(ResourceTypeState resource, string functionCode, out FunctionState function)
        {
            if (!_initialized)
            {
                Init();
            }
            if (!_dicByCode.ContainsKey(resource))
            {
                function = FunctionState.Empty;
                return false;
            }
            if (functionCode == null)
            {
                function = FunctionState.Empty;
                return false;
            }
            return _dicByCode[resource].TryGetValue(functionCode, out function);
        }

        public bool TryGetFunction(Guid functionId, out FunctionState function)
        {
            if (!_initialized)
            {
                Init();
            }
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
            if (!_initialized)
            {
                lock (this)
                {
                    if (_initialized) return;
                    _dicByCode.Clear();
                    _dicById.Clear();
                    var functions = _host.GetRequiredService<IOriginalHostStateReader>().GetAllFunctions();
                    foreach (var entity in functions)
                    {
                        var function = FunctionState.Create(_host, entity);
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
                }
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
            IHandler<ResourceTypeUpdatedEvent>, 
            IHandler<ResourceTypeRemovedEvent>
        {
            private readonly FunctionSet set;

            public MessageHandler(FunctionSet set)
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
                messageDispatcher.Register((IHandler<AddFunctionCommand>)this);
                messageDispatcher.Register((IHandler<FunctionAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateFunctionCommand>)this);
                messageDispatcher.Register((IHandler<FunctionUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveFunctionCommand>)this);
                messageDispatcher.Register((IHandler<FunctionRemovedEvent>)this);
                messageDispatcher.Register((IHandler<ResourceTypeUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<ResourceTypeRemovedEvent>)this);
            }

            public void Handle(ResourceTypeUpdatedEvent message)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                ResourceTypeState newKey;
                if (!host.ResourceTypeSet.TryGetResource(message.Source.Id, out newKey))
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

            public void Handle(ResourceTypeRemovedEvent message)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var key = dicByCode.Keys.FirstOrDefault(a => a.Id == message.Source.Id);
                if (key != null)
                {
                    dicByCode.Remove(key);
                }
            }

            public void Handle(AddFunctionCommand message)
            {
                this.Handle(message.Input, true);
            }

            public void Handle(FunctionAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateFunctionAddedEvent))
                {
                    return;
                }
                this.Handle(message.Output, false);
            }

            private void Handle(IFunctionCreateIo input, bool isCommand)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var functionRepository = host.GetRequiredService<IRepository<Function>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                ResourceTypeState resource;
                if (!host.ResourceTypeSet.TryGetResource(input.ResourceTypeId, out resource))
                {
                    throw new ValidationException("意外的功能资源标识" + input.ResourceTypeId);
                }

                var entity = Function.Create(input);

                lock (this)
                {
                    FunctionState functionState;
                    if (host.FunctionSet.TryGetFunction(input.Id.Value, out functionState))
                    {
                        throw new AnycmdException("记录已经存在");
                    }
                    var state = FunctionState.Create(host, entity);
                    if (host.FunctionSet.TryGetFunction(resource, input.Code, out functionState))
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
                    host.MessageDispatcher.DispatchMessage(new PrivateFunctionAddedEvent(entity, input));
                }
            }

            private class PrivateFunctionAddedEvent : FunctionAddedEvent
            {
                public PrivateFunctionAddedEvent(FunctionBase source, IFunctionCreateIo input) : base(source, input) { }
            }

            public void Handle(UpdateFunctionCommand message)
            {
                this.Handle(message.Output, true);
            }

            public void Handle(FunctionUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateFunctionUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.Input, false);
            }

            private void Handle(IFunctionUpdateIo input, bool isCommand)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var functionRepository = host.GetRequiredService<IRepository<Function>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                FunctionState bkState;
                if (!host.FunctionSet.TryGetFunction(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                ResourceTypeState resource;
                if (!host.ResourceTypeSet.TryGetResource(bkState.ResourceTypeId, out resource))
                {
                    throw new ValidationException("意外的功能资源标识" + bkState.ResourceTypeId);
                }
                Function entity;
                bool stateChanged = false;
                lock (bkState)
                {
                    FunctionState oldState;
                    if (!host.FunctionSet.TryGetFunction(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    FunctionState functionState;
                    if (host.FunctionSet.TryGetFunction(resource, input.Code, out functionState) && functionState.Id != input.Id)
                    {
                        throw new ValidationException("重复的编码");
                    }
                    entity = functionRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException("更新的实体不存在");
                    }

                    entity.Update(input);

                    var newState = FunctionState.Create(host, entity);
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
                    host.MessageDispatcher.DispatchMessage(new PrivateFunctionUpdatedEvent(entity, input));
                }
            }

            private void Update(FunctionState state)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var oldState = dicById[state.Id];
                string oldKey = oldState.Code;
                string newKey = state.Code;
                dicById[state.Id] = state;
                ResourceTypeState resource;
                if (!host.ResourceTypeSet.TryGetResource(oldState.ResourceTypeId, out resource))
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

            private class PrivateFunctionUpdatedEvent : FunctionUpdatedEvent
            {
                public PrivateFunctionUpdatedEvent(FunctionBase source, IFunctionUpdateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(RemoveFunctionCommand message)
            {
                this.Handle(message.EntityId, true);
            }

            public void Handle(FunctionRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateFunctionRemovedEvent))
                {
                    return;
                }
                this.Handle(message.Source.Id, false);
            }

            private void Handle(Guid functionId, bool isCommand)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var functionRepository = host.GetRequiredService<IRepository<Function>>();
                var operationHelpRepository = host.GetRequiredService<IRepository<OperationHelp>>();

                FunctionState bkState;
                if (!host.FunctionSet.TryGetFunction(functionId, out bkState))
                {
                    return;
                }
                Function entity;
                lock (bkState)
                {
                    FunctionState state;
                    if (!host.FunctionSet.TryGetFunction(functionId, out state))
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
                            host.MessageDispatcher.DispatchMessage(new FunctionRemovingEvent(entity));
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
                    host.MessageDispatcher.DispatchMessage(new PrivateFunctionRemovedEvent(entity));
                }
            }

            private class PrivateFunctionRemovedEvent : FunctionRemovedEvent
            {
                public PrivateFunctionRemovedEvent(FunctionBase function)
                    : base(function)
                {

                }
            }
        }
        #endregion
    }
}
