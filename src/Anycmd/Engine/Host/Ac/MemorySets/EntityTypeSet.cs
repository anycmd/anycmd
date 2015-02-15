
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
    using InOuts;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Util;
    using codespace = System.String;
    using entityTypeCode = System.String;
    using entityTypeId = System.Guid;
    using propertyCode = System.String;
    using propertyId = System.Guid;

    internal sealed class EntityTypeSet : IEntityTypeSet, IMemorySet
    {
        public static readonly IEntityTypeSet Empty = new EntityTypeSet(EmptyAcDomain.SingleInstance);
        private static readonly object Locker = new object();

        private readonly Dictionary<entityTypeId, EntityTypeState> _dicById = new Dictionary<entityTypeId, EntityTypeState>();
        private readonly Dictionary<codespace, Dictionary<entityTypeCode, EntityTypeState>> _dicByCode = new Dictionary<codespace, Dictionary<entityTypeCode, EntityTypeState>>(StringComparer.OrdinalIgnoreCase);
        private bool _initialized;

        private readonly IAcDomain _acDomain;
        private readonly Guid _id = Guid.NewGuid();
        private readonly PropertySet _propertySet;

        public Guid Id
        {
            get { return _id; }
        }

        #region Ctor
        internal EntityTypeSet(IAcDomain acDomain)
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
            _propertySet = new PropertySet(acDomain);
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

        public bool TryGetEntityType(Coder code, out EntityTypeState entityType)
        {
            if (code == null)
            {
                throw new ArgumentNullException("code");
            }
            if (!_initialized)
            {
                Init();
            }
            if (!_dicByCode.ContainsKey(code.Codespace))
            {
                entityType = EntityTypeState.Empty;
                return false;
            }
            return _dicByCode[code.Codespace].TryGetValue(code.Code, out entityType);
        }

        public bool TryGetProperty(Guid propertyId, out PropertyState property)
        {
            if (!_initialized)
            {
                Init();
            }
            Debug.Assert(propertyId != Guid.Empty);

            return _propertySet.TryGetProperty(propertyId, out property);
        }

        public bool TryGetProperty(EntityTypeState entityType, string propertyCode, out PropertyState property)
        {
            if (!_initialized)
            {
                Init();
            }
            if (entityType == null || entityType == EntityTypeState.Empty)
            {
                throw new ArgumentNullException("entityType");
            }
            if (string.IsNullOrEmpty(propertyCode))
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
            if (entityType == null || entityType == EntityTypeState.Empty)
            {
                throw new ArgumentNullException("entityType");
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
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dicById.Clear();
                _dicByCode.Clear();
                var entityTypes = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllEntityTypes().OrderBy(a => a.SortCode);
                foreach (var entityType in entityTypes)
                {
                    if (_dicById.ContainsKey(entityType.Id))
                    {
                        throw new AnycmdException("意外的重复的实体类型标识" + entityType.Id);
                    }
                    if (!_dicByCode.ContainsKey(entityType.Codespace))
                    {
                        _dicByCode.Add(entityType.Codespace, new Dictionary<entityTypeCode, EntityTypeState>(StringComparer.OrdinalIgnoreCase));
                    }
                    if (_dicByCode[entityType.Codespace].ContainsKey(entityType.Code))
                    {
                        throw new AnycmdException("意外的重复的实体类型编码" + entityType.Codespace + "." + entityType.Code);
                    }
                    var map = _acDomain.GetEntityTypeMaps().FirstOrDefault(a => a.Codespace.Equals(entityType.Codespace, StringComparison.OrdinalIgnoreCase) && a.Code.Equals(entityType.Code, StringComparison.OrdinalIgnoreCase));
                    var entityTypeState = EntityTypeState.Create(_acDomain, entityType, map);
                    _dicByCode[entityType.Codespace].Add(entityType.Code, entityTypeState);
                    _dicById.Add(entityType.Id, entityTypeState);
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
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
            private readonly EntityTypeSet _set;

            internal MessageHandler(EntityTypeSet set)
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
                messageDispatcher.Register((IHandler<AddEntityTypeCommand>)this);
                messageDispatcher.Register((IHandler<EntityTypeAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateEntityTypeCommand>)this);
                messageDispatcher.Register((IHandler<EntityTypeUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveEntityTypeCommand>)this);
                messageDispatcher.Register((IHandler<EntityTypeRemovedEvent>)this);
            }

            public void Handle(AddEntityTypeCommand message)
            {
                Handle(message.AcSession, message.Input, true);
            }

            public void Handle(EntityTypeAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateEntityTypeAddedEvent))
                {
                    return;
                }
                Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IEntityTypeCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
                var entityTypeRepository = acDomain.RetrieveRequiredService<IRepository<EntityType>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识不能为空");
                }
                EntityType entity;
                lock (Locker)
                {
                    EntityTypeState entityType;
                    if (acDomain.EntityTypeSet.TryGetEntityType(input.Id.Value, out entityType))
                    {
                        throw new AnycmdException("重复的实体类型标识" + input.Id);
                    }
                    if (acDomain.EntityTypeSet.TryGetEntityType(new Coder(input.Codespace, input.Code), out entityType))
                    {
                        throw new ValidationException("重复的编码");
                    }

                    entity = EntityType.Create(input);

                    var map = acDomain.GetEntityTypeMaps().FirstOrDefault(a => a.Codespace.Equals(entity.Codespace, StringComparison.OrdinalIgnoreCase) && a.Code.Equals(entity.Code, StringComparison.OrdinalIgnoreCase));
                    var state = EntityTypeState.Create(acDomain, entity, map);
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
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateEntityTypeAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateEntityTypeAddedEvent : EntityTypeAddedEvent, IPrivateEvent
            {
                internal PrivateEntityTypeAddedEvent(IAcSession acSession, EntityTypeBase source, IEntityTypeCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(UpdateEntityTypeCommand message)
            {
                Handle(message.AcSession, message.Input, true);
            }

            public void Handle(EntityTypeUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateEntityTypeUpdatedEvent))
                {
                    return;
                }
                Handle(message.AcSession, message.Input, false);
            }

            private void Handle(IAcSession acSession, IEntityTypeUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var entityTypeRepository = acDomain.RetrieveRequiredService<IRepository<EntityType>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                EntityTypeState bkState;
                if (!acDomain.EntityTypeSet.TryGetEntityType(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                EntityType entity;
                bool stateChanged;
                lock (Locker)
                {
                    EntityTypeState entityType;
                    if (acDomain.EntityTypeSet.TryGetEntityType(new Coder(input.Codespace, input.Code), out entityType) && entityType.Id != input.Id)
                    {
                        throw new ValidationException("重复的编码");
                    }
                    entity = entityTypeRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new ValidationException("更新的实体不存在");
                    }
                    var map = acDomain.GetEntityTypeMaps().FirstOrDefault(a => a.Codespace.Equals(entity.Codespace, StringComparison.OrdinalIgnoreCase) && a.Code.Equals(entity.Code, StringComparison.OrdinalIgnoreCase));

                    entity.Update(input);

                    var newState = EntityTypeState.Create(acDomain, entity, map);
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
                }
                if (isCommand && stateChanged)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateEntityTypeUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(EntityTypeState state)
            {
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
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

            private class PrivateEntityTypeUpdatedEvent : EntityTypeUpdatedEvent, IPrivateEvent
            {
                internal PrivateEntityTypeUpdatedEvent(IAcSession acSession, EntityTypeBase source, IEntityTypeUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveEntityTypeCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(EntityTypeRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateEntityTypeRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid entityTypeId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
                var entityTypeRepository = acDomain.RetrieveRequiredService<IRepository<EntityType>>();
                EntityTypeState bkState;
                if (!acDomain.EntityTypeSet.TryGetEntityType(entityTypeId, out bkState))
                {
                    return;
                }
                EntityType entity;
                lock (Locker)
                {
                    entity = entityTypeRepository.GetByKey(entityTypeId);
                    if (entity == null)
                    {
                        return;
                    }
                    var properties = acDomain.EntityTypeSet.GetProperties(bkState);
                    if (properties != null && properties.Count > 0)
                    {
                        throw new ValidationException("实体类型有属性后不能删除");
                    }
                    if (dicById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            acDomain.MessageDispatcher.DispatchMessage(new EntityTypeRemovingEvent(acSession, entity));
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
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateEntityTypeRemovedEvent(acSession, entity));
                }
            }

            private class PrivateEntityTypeRemovedEvent : EntityTypeRemovedEvent, IPrivateEvent
            {
                internal PrivateEntityTypeRemovedEvent(IAcSession acSession, EntityTypeBase source)
                    : base(acSession, source)
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
            private static readonly object PropertyLocker = new object();
            private readonly Dictionary<EntityTypeState, Dictionary<propertyCode, PropertyState>> _dicByCode = new Dictionary<EntityTypeState, Dictionary<propertyCode, PropertyState>>();
            private readonly Dictionary<propertyId, PropertyState> _dicById = new Dictionary<propertyId, PropertyState>();
            private bool _initialized = false;
            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _acDomain;

            public Guid Id
            {
                get { return _id; }
            }

            #region Ctor
            internal PropertySet(IAcDomain acDomain)
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
                new PropertyMessageHandler(this).Register();
            }
            #endregion

            public bool TryGetProperty(EntityTypeState entityType, string propertyCode, out PropertyState property)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (entityType == null || entityType == EntityTypeState.Empty)
                {
                    throw new ArgumentNullException("entityType");
                }
                if (string.IsNullOrEmpty(propertyCode))
                {
                    throw new AnycmdException("属性编码为空");
                }
                if (!_dicByCode.ContainsKey(entityType)
                    || !_dicByCode[entityType].ContainsKey(propertyCode))
                {
                    property = PropertyState.CreateNoneProperty(propertyCode);
                    return false;
                }

                return _dicByCode[entityType].TryGetValue(propertyCode, out property);
            }

            public bool TryGetProperty(Guid propertyId, out PropertyState property)
            {
                if (!_initialized)
                {
                    Init();
                }
                Debug.Assert(propertyId != Guid.Empty);

                return _dicById.TryGetValue(propertyId, out property);
            }

            public IReadOnlyDictionary<string, PropertyState> GetProperties(EntityTypeState entityType)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (entityType == null || entityType == EntityTypeState.Empty)
                {
                    throw new ArgumentNullException("entityType");
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
                if (_initialized) return;
                lock (PropertyLocker)
                {
                    if (_initialized) return;
                    _dicByCode.Clear();
                    _dicById.Clear();
                    var properties = _acDomain.RetrieveRequiredService<IOriginalHostStateReader>().GetAllProperties().OrderBy(a => a.SortCode);
                    foreach (var property in properties)
                    {
                        EntityTypeState entityType;
                        if (!_acDomain.EntityTypeSet.TryGetEntityType(property.EntityTypeId, out entityType))
                        {
                            throw new AnycmdException("意外的实体属性类型标识" + property.EntityTypeId);
                        }
                        var propertyState = PropertyState.Create(_acDomain, property);
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
                            throw new AnycmdException("意外的重复实体属性标识" + property.Id);
                        }
                        _dicById.Add(property.Id, propertyState);
                    }
                    _initialized = true;
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
                private readonly PropertySet _set;

                internal PropertyMessageHandler(PropertySet set)
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
                    var acDomain = _set._acDomain;
                    var dicByCode = _set._dicByCode;
                    EntityTypeState newKey;
                    if (!acDomain.EntityTypeSet.TryGetEntityType(message.Source.Id, out newKey))
                    {
                        throw new AnycmdException("意外的实体类型标识" + message.Source.Id);
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
                    var dicByCode = _set._dicByCode;
                    var key = dicByCode.Keys.FirstOrDefault(a => a.Id == message.Source.Id);
                    if (key != null)
                    {
                        dicByCode.Remove(key);
                    }
                }

                public void Handle(AddPropertyCommand message)
                {
                    Handle(message.AcSession, message.Input, true);
                }

                public void Handle(PropertyAddedEvent message)
                {
                    if (message.GetType() == typeof(PrivatePropertyAddedEvent))
                    {
                        return;
                    }
                    Handle(message.AcSession, message.Output, false);
                }

                private void Handle(IAcSession acSession, IPropertyCreateIo input, bool isCommand)
                {
                    var acDomain = _set._acDomain;
                    var dicByCode = _set._dicByCode;
                    var dicById = _set._dicById;
                    var propertyRepository = acDomain.RetrieveRequiredService<IRepository<Property>>();
                    if (string.IsNullOrEmpty(input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    EntityTypeState entityType;
                    if (!acDomain.EntityTypeSet.TryGetEntityType(input.EntityTypeId, out entityType))
                    {
                        throw new AnycmdException("记录已经存在" + input.EntityTypeId);
                    }
                    Property entity;
                    lock (PropertyLocker)
                    {
                        PropertyState property;
                        if (acDomain.EntityTypeSet.TryGetProperty(entityType, input.Code, out property))
                        {
                            throw new ValidationException("编码为" + input.Code + "的属性已经存在");
                        }
                        if (!input.Id.HasValue)
                        {
                            throw new ValidationException("标识是必须的");
                        }
                        if (acDomain.EntityTypeSet.TryGetProperty(input.Id.Value, out property))
                        {
                            throw new AnycmdException("记录已经存在");
                        }

                        entity = Property.Create(input);

                        var state = PropertyState.Create(acDomain, entity);
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
                        acDomain.MessageDispatcher.DispatchMessage(new PrivatePropertyAddedEvent(acSession, entity, input));
                    }
                }

                private class PrivatePropertyAddedEvent : PropertyAddedEvent, IPrivateEvent
                {
                    internal PrivatePropertyAddedEvent(IAcSession acSession, PropertyBase source, IPropertyCreateIo input)
                        : base(acSession, source, input)
                    {

                    }
                }

                public void Handle(AddCommonPropertiesCommand message)
                {
                    var acDomain = _set._acDomain;
                    EntityTypeState entityType;
                    if (!acDomain.EntityTypeSet.TryGetEntityType(message.EntityTypeId, out entityType))
                    {
                        throw new ValidationException("意外的实体类型标识" + message.EntityTypeId);
                    }
                    PropertyState property;
                    #region createIDProperty
                    if (!acDomain.EntityTypeSet.TryGetProperty(entityType, "Id", out property))
                    {
                        var createIdProperty = new AddPropertyCommand(message.AcSession, new PropertyCreateInput
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
                        acDomain.Handle(createIdProperty);
                    }
                    #endregion
                    #region createCreateOnProperty
                    if (!acDomain.EntityTypeSet.TryGetProperty(entityType, "CreateOn", out property))
                    {
                        var createCreateOnProperty = new AddPropertyCommand(message.AcSession, new PropertyCreateInput
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
                        acDomain.Handle(createCreateOnProperty);
                    }
                    #endregion
                    #region createCreateByProperty
                    if (!acDomain.EntityTypeSet.TryGetProperty(entityType, "CreateBy", out property))
                    {
                        var createCreateByProperty = new AddPropertyCommand(message.AcSession, new PropertyCreateInput
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
                        acDomain.Handle(createCreateByProperty);
                    }
                    #endregion
                    #region createCreateUserIdProperty
                    if (!acDomain.EntityTypeSet.TryGetProperty(entityType, "CreateUserId", out property))
                    {
                        var createCreateUserIdProperty = new AddPropertyCommand(message.AcSession, new PropertyCreateInput
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
                        acDomain.Handle(createCreateUserIdProperty);
                    }
                    #endregion
                    #region createModifiedOnProperty
                    if (!acDomain.EntityTypeSet.TryGetProperty(entityType, "ModifiedOn", out property))
                    {
                        var createModifiedOnProperty = new AddPropertyCommand(message.AcSession, new PropertyCreateInput
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
                        acDomain.Handle(createModifiedOnProperty);
                    }
                    #endregion
                    #region createModifiedByProperty
                    if (!acDomain.EntityTypeSet.TryGetProperty(entityType, "ModifiedBy", out property))
                    {
                        var createModifiedByProperty = new AddPropertyCommand(message.AcSession, new PropertyCreateInput
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
                        acDomain.Handle(createModifiedByProperty);
                    }
                    #endregion
                    #region createModifiedUserIDProperty
                    if (!acDomain.EntityTypeSet.TryGetProperty(entityType, "ModifiedUserId", out property))
                    {
                        var createModifiedUserIdProperty = new AddPropertyCommand(message.AcSession, new PropertyCreateInput
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
                        acDomain.Handle(createModifiedUserIdProperty);
                    }
                    #endregion
                }

                private class PropertyCreateInput : EntityCreateInput, IPropertyCreateIo
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

                    public override IAnycmdCommand ToCommand(IAcSession acSession)
                    {
                        return new AddPropertyCommand(acSession, this);
                    }
                }

                public void Handle(UpdatePropertyCommand message)
                {
                    Handle(message.AcSession, message.Input, true);
                }

                public void Handle(PropertyUpdatedEvent message)
                {
                    if (message.GetType() == typeof(PrivatePropertyUpdatedEvent))
                    {
                        return;
                    }
                    Handle(message.AcSession, message.Input, false);
                }

                private void Handle(IAcSession acSession, IPropertyUpdateIo input, bool isCommand)
                {
                    var acDomain = _set._acDomain;
                    var propertyRepository = acDomain.RetrieveRequiredService<IRepository<Property>>();
                    if (string.IsNullOrEmpty(input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    PropertyState bkState;
                    if (!acDomain.EntityTypeSet.TryGetProperty(input.Id, out bkState))
                    {
                        throw new ValidationException("标识" + input.Id + "的属性不存在");
                    }
                    Property entity;
                    bool stateChanged = false;
                    lock (PropertyLocker)
                    {
                        EntityTypeState entityType;
                        PropertyState property;
                        if (!acDomain.EntityTypeSet.TryGetEntityType(bkState.EntityTypeId, out entityType))
                        {
                            throw new ValidationException("意外的实体类型标识" + bkState.EntityTypeId);
                        }
                        if (acDomain.EntityTypeSet.TryGetProperty(entityType, input.Code, out property) && property.Id != input.Id)
                        {
                            throw new ValidationException("编码为" + input.Code + "的属性在" + entityType.Name + "下已经存在");
                        }
                        entity = propertyRepository.GetByKey(input.Id);
                        if (entity == null)
                        {
                            throw new NotExistException();
                        }

                        entity.Update(input);

                        var newState = PropertyState.Create(acDomain, entity);

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
                    }
                    if (isCommand && stateChanged)
                    {
                        acDomain.MessageDispatcher.DispatchMessage(new PrivatePropertyUpdatedEvent(acSession, entity, input));
                    }
                }

                private void Update(PropertyState state)
                {
                    var acDomain = _set._acDomain;
                    var dicByCode = _set._dicByCode;
                    var dicById = _set._dicById;
                    var oldState = dicById[state.Id];
                    EntityTypeState entityType;
                    if (!acDomain.EntityTypeSet.TryGetEntityType(oldState.EntityTypeId, out entityType))
                    {
                        throw new AnycmdException("意外的实体属性类型标识" + oldState.EntityTypeId);
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

                private class PrivatePropertyUpdatedEvent : PropertyUpdatedEvent, IPrivateEvent
                {
                    internal PrivatePropertyUpdatedEvent(IAcSession acSession, PropertyBase source, IPropertyUpdateIo input)
                        : base(acSession, source, input)
                    {

                    }
                }

                public void Handle(RemovePropertyCommand message)
                {
                    this.Handle(message.AcSession, message.EntityId, true);
                }

                public void Handle(PropertyRemovedEvent message)
                {
                    if (message.GetType() == typeof(PrivatePropertyRemovedEvent))
                    {
                        return;
                    }
                    this.Handle(message.AcSession, message.Source.Id, false);
                }

                private void Handle(IAcSession acSession, Guid propertyId, bool isCommand)
                {
                    var acDomain = _set._acDomain;
                    var dicByCode = _set._dicByCode;
                    var dicById = _set._dicById;
                    var propertyRepository = acDomain.RetrieveRequiredService<IRepository<Property>>();
                    PropertyState bkState;
                    if (!acDomain.EntityTypeSet.TryGetProperty(propertyId, out bkState))
                    {
                        return;
                    }
                    Property entity;
                    lock (PropertyLocker)
                    {
                        entity = propertyRepository.GetByKey(propertyId);
                        if (entity == null)
                        {
                            return;
                        }
                        EntityTypeState entityType;
                        if (!acDomain.EntityTypeSet.TryGetEntityType(bkState.EntityTypeId, out entityType))
                        {
                            throw new AnycmdException("意外的实体属性类型标识" + bkState.EntityTypeId);
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
                        acDomain.MessageDispatcher.DispatchMessage(new PrivatePropertyRemovedEvent(acSession, entity));
                    }
                }

                private class PrivatePropertyRemovedEvent : PropertyRemovedEvent, IPrivateEvent
                {
                    internal PrivatePropertyRemovedEvent(IAcSession acSession, PropertyBase source)
                        : base(acSession, source)
                    {

                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
