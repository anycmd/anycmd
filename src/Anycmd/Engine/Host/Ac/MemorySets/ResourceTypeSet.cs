
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

    /// <summary>
    /// 资源上下文
    /// </summary>
    public sealed class ResourceTypeSet : IResourceTypeSet
    {
        public static readonly IResourceTypeSet Empty = new ResourceTypeSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<AppSystemState, Dictionary<string, ResourceTypeState>> _dicByCode = new Dictionary<AppSystemState,Dictionary<string,ResourceTypeState>>();
        private readonly Dictionary<Guid, ResourceTypeState> _dicById = new Dictionary<Guid, ResourceTypeState>();
        private bool _initialized = false;

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        public ResourceTypeSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            new MessageHandler(this).Register();
        }

        public bool TryGetResource(AppSystemState appSystem, string resourceTypeCode, out ResourceTypeState resource)
        {
            if (!_initialized)
            {
                Init();
            }
            if (appSystem == null)
            {
                throw new ArgumentNullException("appSystem");
            }
            if (resourceTypeCode == null)
            {
                throw new ArgumentNullException("resourceTypeCode");
            }
            if (!_dicByCode.ContainsKey(appSystem))
            {
                resource = ResourceTypeState.Empty;
                return false;
            }

            return _dicByCode[appSystem].TryGetValue(resourceTypeCode, out resource);
        }

        public bool TryGetResource(Guid resourceTypeId, out ResourceTypeState resource)
        {
            if (!_initialized)
            {
                Init();
            }

            return _dicById.TryGetValue(resourceTypeId, out resource);
        }

        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        public IEnumerator<ResourceTypeState> GetEnumerator()
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

        private void Init()
        {
            if (!_initialized)
            {
                lock (this)
                {
                    if (!_initialized)
                    {
                        _dicByCode.Clear();
                        _dicById.Clear();
                        var allResources = _host.GetRequiredService<IOriginalHostStateReader>().GetAllResources();
                        foreach (var resource in allResources)
                        {
                            AppSystemState appSystem;
                            if (!_host.AppSystemSet.TryGetAppSystem(resource.AppSystemId, out appSystem))
                            {
                                throw new AnycmdException("意外的资源类型应用系统标识" + resource.AppSystemId);
                            }
                            if (!_dicByCode.ContainsKey(appSystem))
                            {
                                _dicByCode.Add(appSystem, new Dictionary<string, ResourceTypeState>(StringComparer.OrdinalIgnoreCase));
                            }
                            if (_dicByCode[appSystem].ContainsKey(resource.Code))
                            {
                                throw new AnycmdException("意外重复的资源标识" + resource.Id);
                            }
                            if (_dicById.ContainsKey(resource.Id))
                            {
                                throw new AnycmdException("意外重复的资源标识" + resource.Id);
                            }
                            var resourceState = ResourceTypeState.Create(resource);
                            _dicByCode[appSystem].Add(resource.Code, resourceState);
                            _dicById.Add(resource.Id, resourceState);
                        }
                        _initialized = true;
                    }
                }
            }
        }

        #region MessageHandler
        private class MessageHandler:
            IHandler<AddResourceCommand>,
            IHandler<ResourceTypeAddedEvent>,
            IHandler<UpdateResourceCommand>,
            IHandler<ResourceTypeUpdatedEvent>,
            IHandler<RemoveResourceTypeCommand>,
            IHandler<ResourceTypeRemovedEvent>
        {
            private readonly ResourceTypeSet set;

            public MessageHandler(ResourceTypeSet set)
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
                messageDispatcher.Register((IHandler<AddResourceCommand>)this);
                messageDispatcher.Register((IHandler<ResourceTypeAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateResourceCommand>)this);
                messageDispatcher.Register((IHandler<ResourceTypeUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveResourceTypeCommand>)this);
                messageDispatcher.Register((IHandler<ResourceTypeRemovedEvent>)this);
            }

            public void Handle(AddResourceCommand message)
            {
                this.Handle(message.Input, true);
            }

            public void Handle(ResourceTypeAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateResourceAddedEvent))
                {
                    return;
                }
                this.Handle(message.Input, false);
            }

            private void Handle(IResourceTypeCreateIo input, bool isCommand)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var resourceRepository = host.GetRequiredService<IRepository<ResourceType>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                ResourceType entity;
                lock (this)
                {
                    ResourceTypeState resource;
                    if (host.ResourceTypeSet.TryGetResource(input.Id.Value, out resource))
                    {
                        throw new ValidationException("相同标识的资源已经存在" + input.Id.Value);
                    }
                    AppSystemState appSystem;
                    if (!host.AppSystemSet.TryGetAppSystem(input.AppSystemId, out appSystem))
                    {
                        throw new ValidationException("意外的应用系统标识" + input.AppSystemId);
                    }
                    if (host.ResourceTypeSet.TryGetResource(appSystem, input.Code, out resource))
                    {
                        throw new ValidationException("重复的资源编码" + input.Code);
                    }

                    entity = ResourceType.Create(input);

                    var state = ResourceTypeState.Create(entity);
                    if (!dicByCode.ContainsKey(appSystem))
                    {
                        dicByCode.Add(appSystem, new Dictionary<string, ResourceTypeState>(StringComparer.OrdinalIgnoreCase));
                    }
                    if (!dicByCode[appSystem].ContainsKey(entity.Code))
                    {
                        dicByCode[appSystem].Add(entity.Code, state);
                    }
                    if (!dicById.ContainsKey(entity.Id))
                    {
                        dicById.Add(entity.Id, state);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            resourceRepository.Add(entity);
                            resourceRepository.Context.Commit();
                        }
                        catch
                        {
                            if (dicByCode[appSystem].ContainsKey(entity.Code))
                            {
                                dicByCode[appSystem].Remove(entity.Code);
                            }
                            if (dicById.ContainsKey(entity.Id))
                            {
                                dicById.Remove(entity.Id);
                            }
                            resourceRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateResourceAddedEvent(entity, input));
                }
            }

            private class PrivateResourceAddedEvent : ResourceTypeAddedEvent
            {
                public PrivateResourceAddedEvent(ResourceTypeBase source, IResourceTypeCreateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(UpdateResourceCommand message)
            {
                this.Handle(message.Output, true);
            }

            public void Handle(ResourceTypeUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateResourceUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.Input, false);
            }

            private void Handle(IResourceTypeUpdateIo input, bool isCommand)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var resourceRepository = host.GetRequiredService<IRepository<ResourceType>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                ResourceTypeState bkState;
                if (!host.ResourceTypeSet.TryGetResource(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                AppSystemState appSystem;
                if (!host.AppSystemSet.TryGetAppSystem(bkState.AppSystemId, out appSystem))
                {
                    throw new ValidationException("意外的应用系统标识" + bkState.AppSystemId);
                }
                ResourceType entity;
                bool stateChanged = false;
                lock (bkState)
                {
                    ResourceTypeState oldState;
                    if (!host.ResourceTypeSet.TryGetResource(input.Id, out oldState))
                    {
                        throw new NotExistException();
                    }
                    ResourceTypeState resource;
                    if (host.ResourceTypeSet.TryGetResource(appSystem, input.Code, out resource) && resource.Id != input.Id)
                    {
                        throw new ValidationException("重复的资源编码" + input.Code);
                    }
                    entity = resourceRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = ResourceTypeState.Create(entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            resourceRepository.Update(entity);
                            resourceRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            resourceRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateResourceUpdatedEvent(entity, input));
                }
            }

            private void Update(ResourceTypeState state)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                AppSystemState appSystem;
                if (!host.AppSystemSet.TryGetAppSystem(state.AppSystemId, out appSystem))
                {
                    throw new ValidationException("意外的应用系统标识" + state.AppSystemId);
                }
                if (!dicByCode.ContainsKey(appSystem))
                {
                    dicByCode.Add(appSystem, new Dictionary<string, ResourceTypeState>(StringComparer.OrdinalIgnoreCase));
                }
                var oldResource = dicById[state.Id];
                var oldKey = oldResource.Code;
                var newKey = state.Code;
                dicById[state.Id] = state;
                if (!dicByCode[appSystem].ContainsKey(newKey))
                {
                    dicByCode[appSystem].Remove(oldKey);
                    dicByCode[appSystem].Add(newKey, state);
                }
                else
                {
                    dicByCode[appSystem][newKey] = state;
                }
            }

            private class PrivateResourceUpdatedEvent : ResourceTypeUpdatedEvent
            {
                public PrivateResourceUpdatedEvent(ResourceTypeBase source, IResourceTypeUpdateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(RemoveResourceTypeCommand message)
            {
                this.Handle(message.EntityId, true);
            }

            public void Handle(ResourceTypeRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateResourceRemovedEvent))
                {
                    return;
                }
                this.Handle(message.Source.Id, false);
            }

            private void Handle(Guid resourceTypeId, bool isCommand)
            {
                var host = set._host;
                var dicByCode = set._dicByCode;
                var dicById = set._dicById;
                var resourceRepository = host.GetRequiredService<IRepository<ResourceType>>();
                ResourceTypeState bkState;
                if (!host.ResourceTypeSet.TryGetResource(resourceTypeId, out bkState))
                {
                    return;
                }
                ResourceType entity;
                lock (bkState)
                {
                    ResourceTypeState state;
                    if (!host.ResourceTypeSet.TryGetResource(resourceTypeId, out state))
                    {
                        return;
                    }
                    if (host.FunctionSet.Any(a => a.ResourceTypeId == resourceTypeId))
                    {
                        throw new ValidationException("资源下定义有功能时不能删除");
                    }
                    entity = resourceRepository.GetByKey(resourceTypeId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (dicById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            host.MessageDispatcher.DispatchMessage(new ResourceTypeRemovingEvent(entity));
                        }
                        dicById.Remove(bkState.Id);
                    }
                    AppSystemState appSystem;
                    if (!host.AppSystemSet.TryGetAppSystem(state.AppSystemId, out appSystem))
                    {
                        throw new ValidationException("意外的应用系统标识" + state.AppSystemId);
                    }
                    if (dicByCode[appSystem].ContainsKey(bkState.Code))
                    {
                        dicByCode[appSystem].Remove(bkState.Code);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            resourceRepository.Remove(entity);
                            resourceRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!dicById.ContainsKey(bkState.Id))
                            {
                                dicById.Add(bkState.Id, bkState);
                            }
                            if (!dicByCode[appSystem].ContainsKey(bkState.Code))
                            {
                                dicByCode[appSystem].Add(bkState.Code, bkState);
                            }
                            resourceRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateResourceRemovedEvent(entity));
                }
            }

            private class PrivateResourceRemovedEvent : ResourceTypeRemovedEvent
            {
                public PrivateResourceRemovedEvent(ResourceTypeBase source)
                    : base(source)
                {

                }
            }
        }
        #endregion
    }
}