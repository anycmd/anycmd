
namespace Anycmd.Engine.Host.Ac.MemorySets.Impl
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
    using Model;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using codespace = System.String;
    using entityTypeCode = System.String;
    using entityTypeId = System.Guid;
    using propertyCode = System.String;
    using propertyId = System.Guid;

    public sealed class EntityTypeSet : IEntityTypeSet
    {
        public static readonly IEntityTypeSet Empty = new EntityTypeSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<entityTypeId, EntityTypeState> _dicById = new Dictionary<entityTypeId, EntityTypeState>();
        private readonly Dictionary<codespace, Dictionary<entityTypeCode, EntityTypeState>> _dicByCode = new Dictionary<codespace, Dictionary<entityTypeCode, EntityTypeState>>(StringComparer.OrdinalIgnoreCase);
        private bool _initialized = false;

        private readonly IAcDomain _host;
        private readonly Guid _id = Guid.NewGuid();
        private readonly PropertySet _propertySet;

        public Guid Id
        {
            get { return _id; }
        }

        #region Ctor
        public EntityTypeSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            _propertySet = new PropertySet(host);
            new MessageHandler(this).Register();
        }
        #endregion

        public bool TryGetEntityType(Guid entityTypeId, out EntityTypeState entityType)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.TryGetValue(entityTypeId, out entityType);
        }

        public bool TryGetEntityType(string codespace, string entityTypeCode, out EntityTypeState entityType)
        {
            if (codespace == null)
            {
                throw new ArgumentNullException("codespace");
            }
            if (entityTypeCode == null)
            {
                throw new ArgumentNullException("entityTypeCode");
            }
            if (!_initialized)
            {
                Init();
            }
            if (!_dicByCode.ContainsKey(codespace))
            {
                entityType = EntityTypeState.Empty;
                return false;
            }
            return _dicByCode[codespace].TryGetValue(entityTypeCode, out entityType);
        }

        public bool TryGetProperty(Guid propertyId, out PropertyState property)
        {
            if (!_initialized)
            {
                Init();
            }
            return _propertySet.TryGetProperty(propertyId, out property);
        }

        public bool TryGetProperty(EntityTypeState entityType, string propertyCode, out PropertyState property)
        {
            if (!_initialized)
            {
                Init();
            }
            if (propertyCode == null)
            {
                throw new ArgumentNullException("propertyCode");
            }
            return _propertySet.TryGetProperty(entityType, propertyCode, out property);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<string, PropertyState> GetProperties(EntityTypeState entityType)
        {
            if (!_initialized)
            {
                Init();
            }
            return _propertySet.GetProperties(entityType);
        }

        public IEnumerable<PropertyState> GetProperties()
        {
            if (!_initialized)
            {
                Init();
            }
            return _propertySet;
        }

        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        public IEnumerator<EntityTypeState> GetEnumerator()
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
                    if (!_initialized)
                    {
                        _dicById.Clear();
                        _dicByCode.Clear();
                        var entityTypes = _host.GetRequiredService<IOriginalHostStateReader>().GetAllEntityTypes().OrderBy(a => a.SortCode);
                        foreach (var entityType in entityTypes)
                        {
                            if (_dicById.ContainsKey(entityType.Id))
                            {
                                throw new CoreException("意外的重复的实体类型标识" + entityType.Id);
                            }
                            if (!_dicByCode.ContainsKey(entityType.Codespace))
                            {
                                _dicByCode.Add(entityType.Codespace, new Dictionary<entityTypeCode, EntityTypeState>(StringComparer.OrdinalIgnoreCase));
                            }
                            if (_dicByCode[entityType.Codespace].ContainsKey(entityType.Code))
                            {
                                throw new CoreException("意外的重复的实体类型编码" + entityType.Codespace + "." + entityType.Code);
                            }
                            var map = _host.GetEntityTypeMaps().FirstOrDefault(a => a.Codespace.Equals(entityType.Codespace, StringComparison.OrdinalIgnoreCase) && a.Code.Equals(entityType.Code, StringComparison.OrdinalIgnoreCase));
                            var entityTypeState = EntityTypeState.Create(_host, entityType, map);
                            _dicByCode[entityType.Codespace].Add(entityType.Code, entityTypeState);
                            _dicById.Add(entityType.Id, entityTypeState);
                        }
                        _initialized = true;
                    }
                }
            }
        }
        #endregion

        #region MessageHandler
        private class MessageHandler :
            IHandler<EntityTypeRemovedEvent>, 
            IHandler<AddEntityTypeCommand>, 
            IHandler<EntityTypeAddedEvent>, 
            IHandler<UpdateEntityTypeCommand>, 
            IHandler<EntityTypeUpdatedEvent>, 
            IHandler<RemoveEntityTypeCommand>
        {
            private readonly EntityTypeSet set;

            public MessageHandler(EntityTypeSet set)
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
                messageDispatcher.Register((IHandler<AddEntityTypeCommand>)this);
                messageDispatcher.Register((IHandler<EntityTypeAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateEntityTypeCommand>)this);
                messageDispatcher.Register((IHandler<EntityTypeUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveEntityTypeCommand>)this);
                messageDispatcher.Register((IHandler<EntityTypeRemovedEvent>)this);
            }

            public void Handle(AddEntityTypeCommand message)
            {
                this.Handle(message.Input, true);
            }

            public void Handle(EntityTypeAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateEntityTypeAddedEvent))
                {
                    return;
                }
                this.Handle(message.Output, false);
            }

            private void Handle(IEntityTypeCreateIo input, bool isCommand)
            {
                var host = set._host;
                var dicById = set._dicById;
                var dicByCode = set._dicByCode;
                var entityTypeRepository = host.GetRequiredService<IRepository<EntityType>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识不能为空");
                }
                EntityType entity;
                lock (this)
                {
                    EntityTypeState entityType;
                    if (host.EntityTypeSet.TryGetEntityType(input.Id.Value, out entityType))
                    {
                        throw new CoreException("重复的实体类型标识" + input.Id);
                    }
                    if (host.EntityTypeSet.TryGetEntityType(input.Codespace, input.Code, out entityType))
                    {
                        throw new ValidationException("重复的编码");
                    }

                    entity = EntityType.Create(input);

                    var map = host.GetEntityTypeMaps().FirstOrDefault(a => a.Codespace.Equals(entity.Codespace, StringComparison.OrdinalIgnoreCase) && a.Code.Equals(entity.Code, StringComparison.OrdinalIgnoreCase));
                    var state = EntityTypeState.Create(host, entity, map);
                    if (!dicById.ContainsKey(entity.Id))
                    {
                        dicById.Add(entity.Id, state);
                    }
                    if (!dicByCode.ContainsKey(state.Codespace))
                    {
                        dicByCode.Add(state.Codespace, new Dictionary<entityTypeCode, EntityTypeState>(StringComparer.OrdinalIgnoreCase));
                    }
                    if (!dicByCode[state.Codespace].ContainsKey(state.Code))
                    {
                        dicByCode[state.Codespace].Add(state.Code, state);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            entityTypeRepository.Add(entity);
                            entityTypeRepository.Context.Commit();
                        }
                        catch
                        {
                            if (dicById.ContainsKey(entity.Id))
                            {
                                dicById.Remove(entity.Id);
                            }
                            if (dicByCode.ContainsKey(entity.Codespace) && dicByCode[entity.Codespace].ContainsKey(entity.Code))
                            {
                                dicByCode[entity.Codespace].Remove(entity.Code);
                            }
                            entityTypeRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateEntityTypeAddedEvent(entity, input));
                }
            }

            private class PrivateEntityTypeAddedEvent : EntityTypeAddedEvent
            {
                public PrivateEntityTypeAddedEvent(EntityTypeBase source, IEntityTypeCreateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(UpdateEntityTypeCommand message)
            {
                this.Handle(message.Output, true);
            }

            public void Handle(EntityTypeUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateEntityTypeUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.Input, false);
            }

            private void Handle(IEntityTypeUpdateIo input, bool isCommand)
            {
                var host = set._host;
                var dicById = set._dicById;
                var dicByCode = set._dicByCode;
                var entityTypeRepository = host.GetRequiredService<IRepository<EntityType>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                EntityTypeState bkState;
                if (!host.EntityTypeSet.TryGetEntityType(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                EntityType entity;
                bool stateChanged = false;
                lock (bkState)
                {
                    EntityTypeState entityType;
                    if (host.EntityTypeSet.TryGetEntityType(input.Codespace, input.Code, out entityType) && entityType.Id != input.Id)
                    {
                        throw new ValidationException("重复的编码");
                    }
                    entity = entityTypeRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new ValidationException("更新的实体不存在");
                    }
                    var map = host.GetEntityTypeMaps().FirstOrDefault(a => a.Codespace.Equals(entity.Codespace, StringComparison.OrdinalIgnoreCase) && a.Code.Equals(entity.Code, StringComparison.OrdinalIgnoreCase));

                    entity.Update(input);

                    var newState = EntityTypeState.Create(host, entity, map);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            entityTypeRepository.Update(entity);
                            entityTypeRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            entityTypeRepository.Context.Rollback();
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
                    host.MessageDispatcher.DispatchMessage(new PrivateEntityTypeUpdatedEvent(entity, input));
                }
            }

            private void Update(EntityTypeState state)
            {
                var host = set._host;
                var dicById = set._dicById;
                var dicByCode = set._dicByCode;
                var oldState = dicById[state.Id];
                string oldCodespace = oldState.Codespace;
                string newCodespace = state.Codespace;
                string oldKey = oldState.Code;
                string newKey = state.Code;
                dicById[state.Id] = state;
                if (!dicByCode[oldCodespace].ContainsKey(newKey))
                {
                    dicByCode[oldCodespace].Add(newKey, state);
                    dicByCode[oldCodespace].Remove(oldKey);
                }
                else
                {
                    dicByCode[oldCodespace][newKey] = state;
                }
                if (!dicByCode.ContainsKey(newCodespace))
                {
                    dicByCode.Add(newCodespace, new Dictionary<entityTypeCode, EntityTypeState>(StringComparer.OrdinalIgnoreCase));
                    foreach (var item in dicByCode[oldCodespace])
                    {
                        dicByCode[newCodespace].Add(item.Key, item.Value);
                    }
                    dicByCode.Remove(oldCodespace);
                }
            }

            private class PrivateEntityTypeUpdatedEvent : EntityTypeUpdatedEvent
            {
                public PrivateEntityTypeUpdatedEvent(EntityTypeBase source, IEntityTypeUpdateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(RemoveEntityTypeCommand message)
            {
                this.Handle(message.EntityId, true);
            }

            public void Handle(EntityTypeRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateEntityTypeRemovedEvent))
                {
                    return;
                }
                this.Handle(message.Source.Id, false);
            }

            private void Handle(Guid entityTypeId, bool isCommand)
            {
                var host = set._host;
                var dicById = set._dicById;
                var dicByCode = set._dicByCode;
                var propertyRepository = host.GetRequiredService<IRepository<Property>>();
                var entityTypeRepository = host.GetRequiredService<IRepository<EntityType>>();
                EntityTypeState bkState;
                if (!host.EntityTypeSet.TryGetEntityType(entityTypeId, out bkState))
                {
                    return;
                }
                EntityType entity;
                lock (bkState)
                {
                    entity = entityTypeRepository.GetByKey(entityTypeId);
                    if (entity == null)
                    {
                        return;
                    }
                    var properties = host.EntityTypeSet.GetProperties(bkState);
                    if (properties != null && properties.Count > 0)
                    {
                        throw new ValidationException("实体类型有属性后不能删除");
                    }
                    if (dicById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            host.MessageDispatcher.DispatchMessage(new EntityTypeRemovingEvent(entity));
                        }
                        var entityType = dicById[bkState.Id];
                        if (dicByCode.ContainsKey(entityType.Codespace) && dicByCode[entityType.Codespace].ContainsKey(entityType.Code))
                        {
                            dicByCode[entityType.Codespace].Remove(entityType.Code);
                        }
                        dicById.Remove(entityType.Id);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            entityTypeRepository.Remove(entity);
                            entityTypeRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!dicById.ContainsKey(bkState.Id))
                            {
                                dicById.Add(bkState.Id, bkState);
                            }
                            if (!dicByCode.ContainsKey(bkState.Codespace))
                            {
                                dicByCode.Add(bkState.Codespace, new Dictionary<propertyCode, EntityTypeState>(StringComparer.OrdinalIgnoreCase));
                            }
                            if (!dicByCode[bkState.Codespace].ContainsKey(bkState.Code))
                            {
                                dicByCode[bkState.Codespace].Add(bkState.Code, bkState);
                            }
                            entityTypeRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateEntityTypeRemovedEvent(entity));
                }
            }

            private class PrivateEntityTypeRemovedEvent : EntityTypeRemovedEvent
            {
                public PrivateEntityTypeRemovedEvent(EntityTypeBase source)
                    : base(source)
                {

                }
            }
        }
        #endregion

        // 内部类
        #region PropertySet
        /// <summary>
        /// 系统字段上下文
        /// </summary>
        private class PropertySet : IEnumerable<PropertyState>
        {
            private readonly Dictionary<EntityTypeState, Dictionary<propertyCode, PropertyState>> _dicByCode = new Dictionary<EntityTypeState, Dictionary<propertyCode, PropertyState>>();
            private readonly Dictionary<propertyId, PropertyState> _dicById = new Dictionary<propertyId, PropertyState>();
            private bool _initialized = false;
            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _host;

            public Guid Id
            {
                get { return _id; }
            }

            #region Ctor
            public PropertySet(IAcDomain host)
            {
                if (host == null)
                {
                    throw new ArgumentNullException("host");
                }
                this._host = host;
                new PropertyMessageHandler(this).Register();
            }
            #endregion

            public bool TryGetProperty(EntityTypeState entityType, string propertyCode, out PropertyState property)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (propertyCode == null)
                {
                    throw new CoreException("属性编码为空");
                }
                if (!_dicByCode.ContainsKey(entityType)
                    || !_dicByCode[entityType].ContainsKey(propertyCode))
                {
                    property = PropertyState.CreateNoneProperty(propertyCode);
                    return false;
                }

                return _dicByCode[entityType].TryGetValue(propertyCode, out property);
            }

            public PropertyState this[Guid propertyId]
            {
                get
                {
                    if (!_initialized)
                    {
                        Init();
                    }
                    return !_dicById.ContainsKey(propertyId) ? PropertyState.CreateNoneProperty(string.Empty) : _dicById[propertyId];
                }
            }

            public bool TryGetProperty(Guid propertyId, out PropertyState property)
            {
                if (!_initialized)
                {
                    Init();
                }
                return _dicById.TryGetValue(propertyId, out property);
            }

            public IReadOnlyDictionary<string, PropertyState> GetProperties(EntityTypeState entityType)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_dicByCode.ContainsKey(entityType))
                {
                    return new Dictionary<string, PropertyState>(StringComparer.OrdinalIgnoreCase);
                }
                return _dicByCode[entityType];
            }

            internal void Refresh()
            {
                if (_initialized)
                {
                    _initialized = false;
                }
            }

            public IEnumerator<PropertyState> GetEnumerator()
            {
                if (!_initialized)
                {
                    Init();
                }
                return ((IEnumerable<PropertyState>) _dicById.Values).GetEnumerator();
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
                        if (!_initialized)
                        {
                            _dicByCode.Clear();
                            _dicById.Clear();
                            var properties = _host.GetRequiredService<IOriginalHostStateReader>().GetAllProperties().OrderBy(a => a.SortCode);
                            foreach (var property in properties)
                            {
                                EntityTypeState entityType;
                                if (!_host.EntityTypeSet.TryGetEntityType(property.EntityTypeId, out entityType))
                                {
                                    throw new CoreException("意外的实体属性类型标识" + property.EntityTypeId);
                                }
                                var propertyState = PropertyState.Create(_host, property);
                                if (!_dicByCode.ContainsKey(entityType))
                                {
                                    _dicByCode.Add(entityType, new Dictionary<propertyCode, PropertyState>(StringComparer.OrdinalIgnoreCase));
                                }
                                if (!_dicByCode[entityType].ContainsKey(property.Code))
                                {
                                    _dicByCode[entityType].Add(property.Code, propertyState);
                                }
                                if (_dicById.ContainsKey(property.Id))
                                {
                                    throw new CoreException("意外的重复实体属性标识" + property.Id);
                                }
                                _dicById.Add(property.Id, propertyState);
                            }
                            _initialized = true;
                        }
                    }
                }
            }
            #endregion

            #region PropertyMessageHandler
            private class PropertyMessageHandler:
                IHandler<AddPropertyCommand>,
                IHandler<PropertyAddedEvent>,
                IHandler<AddCommonPropertiesCommand>,
                IHandler<UpdatePropertyCommand>,
                IHandler<PropertyUpdatedEvent>,
                IHandler<RemovePropertyCommand>,
                IHandler<PropertyRemovedEvent>,
                IHandler<EntityTypeUpdatedEvent>,
                IHandler<EntityTypeRemovedEvent>
            {
                private readonly PropertySet set;

                public PropertyMessageHandler(PropertySet set)
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
                    messageDispatcher.Register((IHandler<AddPropertyCommand>)this);
                    messageDispatcher.Register((IHandler<PropertyAddedEvent>)this);
                    messageDispatcher.Register((IHandler<UpdatePropertyCommand>)this);
                    messageDispatcher.Register((IHandler<PropertyUpdatedEvent>)this);
                    messageDispatcher.Register((IHandler<RemovePropertyCommand>)this);
                    messageDispatcher.Register((IHandler<PropertyRemovedEvent>)this);
                    messageDispatcher.Register((IHandler<AddCommonPropertiesCommand>)this);
                    messageDispatcher.Register((IHandler<EntityTypeUpdatedEvent>)this);
                    messageDispatcher.Register((IHandler<EntityTypeRemovedEvent>)this);
                }

                public void Handle(EntityTypeUpdatedEvent message)
                {
                    var host = set._host;
                    var dicByCode = set._dicByCode;
                    var dicById = set._dicById;
                    EntityTypeState newKey;
                    if (!host.EntityTypeSet.TryGetEntityType(message.Source.Id, out newKey))
                    {
                        throw new CoreException("意外的实体类型标识" + message.Source.Id);
                    }
                    if (!dicByCode.ContainsKey(newKey))
                    {
                        var oldKey = dicByCode.Keys.FirstOrDefault(a => a.Id == newKey.Id);
                        if (oldKey != null)
                        {
                            dicByCode.Add(newKey, dicByCode[oldKey]);
                            dicByCode.Remove(oldKey);
                        }
                    }
                }

                public void Handle(EntityTypeRemovedEvent message)
                {
                    var host = set._host;
                    var dicByCode = set._dicByCode;
                    var dicById = set._dicById;
                    var key = dicByCode.Keys.FirstOrDefault(a => a.Id == message.Source.Id);
                    if (key != null)
                    {
                        dicByCode.Remove(key);
                    }
                }

                public void Handle(AddPropertyCommand message)
                {
                    this.Handle(message.Input, true);
                }

                public void Handle(PropertyAddedEvent message)
                {
                    if (message.GetType() == typeof(PrivatePropertyAddedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Output, false);
                }

                private void Handle(IPropertyCreateIo input, bool isCommand)
                {
                    var host = set._host;
                    var dicByCode = set._dicByCode;
                    var dicById = set._dicById;
                    var propertyRepository = host.GetRequiredService<IRepository<Property>>();
                    if (string.IsNullOrEmpty(input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    EntityTypeState entityType;
                    if (!host.EntityTypeSet.TryGetEntityType(input.EntityTypeId, out entityType))
                    {
                        throw new CoreException("记录已经存在" + input.EntityTypeId);
                    }
                    Property entity;
                    lock (this)
                    {
                        PropertyState property;
                        if (host.EntityTypeSet.TryGetProperty(entityType, input.Code, out property))
                        {
                            throw new ValidationException("编码为" + input.Code + "的属性已经存在");
                        }
                        if (!input.Id.HasValue)
                        {
                            throw new ValidationException("标识是必须的");
                        }
                        if (host.EntityTypeSet.TryGetProperty(input.Id.Value, out property))
                        {
                            throw new CoreException("记录已经存在");
                        }

                        entity = Property.Create(input);

                        var state = PropertyState.Create(host, entity);
                        if (!dicByCode.ContainsKey(entityType))
                        {
                            dicByCode.Add(entityType, new Dictionary<propertyCode, PropertyState>(StringComparer.OrdinalIgnoreCase));
                        }
                        if (!dicByCode[entityType].ContainsKey(entity.Code))
                        {
                            dicByCode[entityType].Add(entity.Code, state);
                        }
                        if (!dicById.ContainsKey(entity.Id))
                        {
                            dicById.Add(entity.Id, state);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                propertyRepository.Add(entity);
                                propertyRepository.Context.Commit();
                            }
                            catch
                            {
                                if (dicByCode.ContainsKey(entityType) && dicByCode[entityType].ContainsKey(entity.Code))
                                {
                                    dicByCode[entityType].Remove(entity.Code);
                                }
                                if (dicById.ContainsKey(entity.Id))
                                {
                                    dicById.Remove(entity.Id);
                                }
                                propertyRepository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivatePropertyAddedEvent(entity, input));
                    }
                }

                private class PrivatePropertyAddedEvent : PropertyAddedEvent
                {
                    public PrivatePropertyAddedEvent(PropertyBase source, IPropertyCreateIo input)
                        : base(source, input)
                    {

                    }
                }

                public void Handle(AddCommonPropertiesCommand message)
                {
                    var host = set._host;
                    var dicByCode = set._dicByCode;
                    var dicById = set._dicById;
                    EntityTypeState entityType;
                    if (!host.EntityTypeSet.TryGetEntityType(message.EntityTypeId, out entityType))
                    {
                        throw new ValidationException("意外的实体类型标识" + message.EntityTypeId);
                    }
                    PropertyState property;
                    #region createIDProperty
                    if (!host.EntityTypeSet.TryGetProperty(entityType, "Id", out property))
                    {
                        var createIdProperty = new AddPropertyCommand(new PropertyCreateInput
                        {
                            Code = "Id",
                            Description = null,
                            DicId = null,
                            EntityTypeId = message.EntityTypeId,
                            GuideWords = null,
                            Icon = null,
                            Id = Guid.NewGuid(),
                            InputType = null,
                            IsDetailsShow = false,
                            IsDeveloperOnly = true,
                            IsInput = false,
                            IsTotalLine = false,
                            IsViewField = false,
                            MaxLength = null,
                            Name = "标识",
                            Nullable = false,
                            OType = "Guid",
                            SortCode = 0
                        });
                        host.Handle(createIdProperty);
                    }
                    #endregion
                    #region createCreateOnProperty
                    if (!host.EntityTypeSet.TryGetProperty(entityType, "CreateOn", out property))
                    {
                        var createCreateOnProperty = new AddPropertyCommand(new PropertyCreateInput
                        {
                            Code = "CreateOn",
                            Description = null,
                            DicId = null,
                            EntityTypeId = message.EntityTypeId,
                            GuideWords = null,
                            Icon = null,
                            Id = Guid.NewGuid(),
                            InputType = null,
                            IsDetailsShow = true,
                            IsDeveloperOnly = false,
                            IsInput = false,
                            IsTotalLine = false,
                            IsViewField = false,
                            MaxLength = null,
                            Name = "创建时间",
                            Nullable = true,
                            OType = "DateTime",
                            SortCode = 1000
                        });
                        host.Handle(createCreateOnProperty);
                    }
                    #endregion
                    #region createCreateByProperty
                    if (!host.EntityTypeSet.TryGetProperty(entityType, "CreateBy", out property))
                    {
                        var createCreateByProperty = new AddPropertyCommand(new PropertyCreateInput
                        {
                            Code = "CreateBy",
                            Description = null,
                            DicId = null,
                            EntityTypeId = message.EntityTypeId,
                            GuideWords = null,
                            Icon = null,
                            Id = Guid.NewGuid(),
                            InputType = null,
                            IsDetailsShow = true,
                            IsDeveloperOnly = false,
                            IsInput = false,
                            IsTotalLine = false,
                            IsViewField = false,
                            MaxLength = null,
                            Name = "创建人",
                            Nullable = true,
                            OType = "String",
                            SortCode = 1002
                        });
                        host.Handle(createCreateByProperty);
                    }
                    #endregion
                    #region createCreateUserIdProperty
                    if (!host.EntityTypeSet.TryGetProperty(entityType, "CreateUserId", out property))
                    {
                        var createCreateUserIdProperty = new AddPropertyCommand(new PropertyCreateInput
                        {
                            Code = "CreateUserId",
                            Description = null,
                            DicId = null,
                            EntityTypeId = message.EntityTypeId,
                            GuideWords = null,
                            Icon = null,
                            Id = Guid.NewGuid(),
                            InputType = null,
                            IsDetailsShow = false,
                            IsDeveloperOnly = true,
                            IsInput = false,
                            IsTotalLine = false,
                            IsViewField = false,
                            MaxLength = null,
                            Name = "创建人标识",
                            Nullable = true,
                            OType = "Guid",
                            SortCode = 1001
                        });
                        host.Handle(createCreateUserIdProperty);
                    }
                    #endregion
                    #region createModifiedOnProperty
                    if (!host.EntityTypeSet.TryGetProperty(entityType, "ModifiedOn", out property))
                    {
                        var createModifiedOnProperty = new AddPropertyCommand(new PropertyCreateInput
                        {
                            Code = "ModifiedOn",
                            Description = null,
                            DicId = null,
                            EntityTypeId = message.EntityTypeId,
                            GuideWords = null,
                            Icon = null,
                            Id = Guid.NewGuid(),
                            InputType = null,
                            IsDetailsShow = true,
                            IsDeveloperOnly = false,
                            IsInput = false,
                            IsTotalLine = false,
                            IsViewField = false,
                            MaxLength = null,
                            Name = "最后修改时间",
                            Nullable = true,
                            OType = "DateTime",
                            SortCode = 1003
                        });
                        host.Handle(createModifiedOnProperty);
                    }
                    #endregion
                    #region createModifiedByProperty
                    if (!host.EntityTypeSet.TryGetProperty(entityType, "ModifiedBy", out property))
                    {
                        var createModifiedByProperty = new AddPropertyCommand(new PropertyCreateInput
                        {
                            Code = "ModifiedBy",
                            Description = null,
                            DicId = null,
                            EntityTypeId = message.EntityTypeId,
                            GuideWords = null,
                            Icon = null,
                            Id = Guid.NewGuid(),
                            InputType = null,
                            IsDetailsShow = true,
                            IsDeveloperOnly = false,
                            IsInput = false,
                            IsTotalLine = false,
                            IsViewField = false,
                            MaxLength = null,
                            Name = "最后修改人",
                            Nullable = true,
                            OType = "String",
                            SortCode = 1005
                        });
                        host.Handle(createModifiedByProperty);
                    }
                    #endregion
                    #region createModifiedUserIDProperty
                    if (!host.EntityTypeSet.TryGetProperty(entityType, "ModifiedUserId", out property))
                    {
                        var createModifiedUserIdProperty = new AddPropertyCommand(new PropertyCreateInput
                        {
                            Code = "ModifiedUserId",
                            Description = null,
                            DicId = null,
                            EntityTypeId = message.EntityTypeId,
                            GuideWords = null,
                            Icon = null,
                            Id = Guid.NewGuid(),
                            InputType = null,
                            IsDetailsShow = false,
                            IsDeveloperOnly = true,
                            IsInput = false,
                            IsTotalLine = false,
                            IsViewField = false,
                            MaxLength = null,
                            Name = "最后修改人标识",
                            Nullable = true,
                            OType = "Guid",
                            SortCode = 1004
                        });
                        host.Handle(createModifiedUserIdProperty);
                    }
                    #endregion
                }

                private class PropertyCreateInput : EntityCreateInput, IInputModel, IPropertyCreateIo
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public Guid EntityTypeId { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public Guid? ForeignPropertyId { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string Code { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string Description { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public int? MaxLength { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public bool Nullable { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string OType { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string Name { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string Icon { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string GuideWords { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public int SortCode { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public Guid? DicId { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public bool IsViewField { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public bool IsDetailsShow { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public bool IsDeveloperOnly { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string InputType { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public bool IsInput { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public bool IsTotalLine { get; set; }


                    public propertyCode GroupCode { get; set; }

                    public propertyCode Tooltip { get; set; }
                }

                public void Handle(UpdatePropertyCommand message)
                {
                    this.Handle(message.Output, true);
                }

                public void Handle(PropertyUpdatedEvent message)
                {
                    if (message.GetType() == typeof(PrivatePropertyUpdatedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Input, false);
                }

                private void Handle(IPropertyUpdateIo input, bool isCommand)
                {
                    var host = set._host;
                    var dicByCode = set._dicByCode;
                    var dicById = set._dicById;
                    var propertyRepository = host.GetRequiredService<IRepository<Property>>();
                    if (string.IsNullOrEmpty(input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    PropertyState bkState;
                    if (!host.EntityTypeSet.TryGetProperty(input.Id, out bkState))
                    {
                        throw new ValidationException("标识" + input.Id + "的属性不存在");
                    }
                    Property entity;
                    bool stateChanged = false;
                    lock (bkState)
                    {
                        EntityTypeState entityType;
                        PropertyState property;
                        if (!host.EntityTypeSet.TryGetEntityType(bkState.EntityTypeId, out entityType))
                        {
                            throw new ValidationException("意外的实体类型标识" + bkState.EntityTypeId);
                        }
                        if (host.EntityTypeSet.TryGetProperty(entityType, input.Code, out property) && property.Id != input.Id)
                        {
                            throw new ValidationException("编码为" + input.Code + "的属性在" + entityType.Name + "下已经存在");
                        }
                        entity = propertyRepository.GetByKey(input.Id);
                        if (entity == null)
                        {
                            throw new NotExistException();
                        }

                        entity.Update(input);

                        var newState = PropertyState.Create(host, entity);

                        stateChanged = newState != bkState;
                        if (stateChanged)
                        {
                            Update(newState);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                propertyRepository.Update(entity);
                                propertyRepository.Context.Commit();
                            }
                            catch
                            {
                                if (stateChanged)
                                {
                                    Update(bkState);
                                }
                                propertyRepository.Context.Rollback();
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
                        host.MessageDispatcher.DispatchMessage(new PrivatePropertyUpdatedEvent(entity, input));
                    }
                }

                private void Update(PropertyState state)
                {
                    var host = set._host;
                    var dicByCode = set._dicByCode;
                    var dicById = set._dicById;
                    var oldState = dicById[state.Id];
                    EntityTypeState entityType;
                    if (!host.EntityTypeSet.TryGetEntityType(oldState.EntityTypeId, out entityType))
                    {
                        throw new CoreException("意外的实体属性类型标识" + oldState.EntityTypeId);
                    }
                    string oldKey = oldState.Code;
                    string newKey = state.Code;
                    dicById[state.Id] = state;
                    if (!dicByCode[entityType].ContainsKey(newKey))
                    {
                        dicByCode[entityType].Remove(oldKey);
                        dicByCode[entityType].Add(newKey, state);
                    }
                    else
                    {
                        dicByCode[entityType][newKey] = state;
                    }
                }

                private class PrivatePropertyUpdatedEvent : PropertyUpdatedEvent
                {
                    public PrivatePropertyUpdatedEvent(PropertyBase source, IPropertyUpdateIo input)
                        : base(source, input)
                    {

                    }
                }
                public void Handle(RemovePropertyCommand message)
                {
                    this.Handle(message.EntityId, true);
                }

                public void Handle(PropertyRemovedEvent message)
                {
                    if (message.GetType() == typeof(PrivatePropertyRemovedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Source.Id, false);
                }

                private void Handle(Guid propertyId, bool isCommand)
                {
                    var host = set._host;
                    var dicByCode = set._dicByCode;
                    var dicById = set._dicById;
                    var propertyRepository = host.GetRequiredService<IRepository<Property>>();
                    PropertyState bkState;
                    if (!host.EntityTypeSet.TryGetProperty(propertyId, out bkState))
                    {
                        return;
                    }
                    Property entity;
                    lock (bkState)
                    {
                        entity = propertyRepository.GetByKey(propertyId);
                        if (entity == null)
                        {
                            return;
                        }
                        EntityTypeState entityType;
                        if (!host.EntityTypeSet.TryGetEntityType(bkState.EntityTypeId, out entityType))
                        {
                            throw new CoreException("意外的实体属性类型标识" + bkState.EntityTypeId);
                        }
                        if (dicById.ContainsKey(bkState.Id))
                        {
                            if (dicByCode.ContainsKey(entityType) && dicByCode[entityType].ContainsKey(bkState.Code))
                            {
                                dicByCode[entityType].Remove(bkState.Code);
                            }
                            dicById.Remove(bkState.Id);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                propertyRepository.Remove(entity);
                                propertyRepository.Context.Commit();
                            }
                            catch
                            {
                                if (!dicById.ContainsKey(bkState.Id))
                                {
                                    dicById.Add(bkState.Id, bkState);
                                }
                                if (!dicByCode.ContainsKey(entityType))
                                {
                                    dicByCode.Add(entityType, new Dictionary<propertyCode, PropertyState>(StringComparer.OrdinalIgnoreCase));
                                }
                                if (!dicByCode[entityType].ContainsKey(bkState.Code))
                                {
                                    dicByCode[entityType].Add(bkState.Code, bkState);
                                }
                                propertyRepository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivatePropertyRemovedEvent(entity));
                    }
                }

                private class PrivatePropertyRemovedEvent : PropertyRemovedEvent
                {
                    public PrivatePropertyRemovedEvent(PropertyBase source)
                        : base(source)
                    {

                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
