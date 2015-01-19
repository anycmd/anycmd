
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
    using System.Linq;
    using System.Text;
    using Util;

    internal sealed class DicSet : IDicSet, IMemorySet
    {
        public static readonly IDicSet Empty = new DicSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<Guid, DicState> _dicById = new Dictionary<Guid, DicState>();
        private readonly Dictionary<string, DicState> _dicByCode = new Dictionary<string, DicState>(StringComparer.OrdinalIgnoreCase);
        private bool _initialized;

        private readonly IAcDomain _host;
        private readonly DicItemSet _dicItemSet;
        private readonly Guid _id = Guid.NewGuid();

        public Guid Id
        {
            get { return _id; }
        }

        #region Ctor
        internal DicSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (host.Equals(EmptyAcDomain.SingleInstance))
            {
                _initialized = true;
            }
            _host = host;
            _dicItemSet = new DicItemSet(host);
            new DicMessageHandler(this).Register();
        }
        #endregion

        public bool ContainsDic(Guid dicId)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicById.ContainsKey(dicId);
        }

        public bool ContainsDic(string dicCode)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicByCode.ContainsKey(dicCode);
        }

        public bool TryGetDic(Guid dicId, out DicState dic)
        {
            if (!_initialized)
            {
                Init();
            }

            return _dicById.TryGetValue(dicId, out dic);
        }

        public bool TryGetDic(string dicCode, out DicState dic)
        {
            if (dicCode == null)
            {
                throw new ArgumentNullException("dicCode");
            }
            if (!_initialized)
            {
                Init();
            }
            return _dicByCode.TryGetValue(dicCode, out dic);
        }

        public bool ContainsDicItem(Guid dicItemId)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicItemSet.ContainsDicItem(dicItemId);
        }

        public bool ContainsDicItem(DicState dic, string dicItemCode)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicItemSet.ContainsDicItem(dic, dicItemCode);
        }

        public IReadOnlyDictionary<string, DicItemState> GetDicItems(DicState dic)
        {
            if (!_initialized)
            {
                Init();
            }
            Dictionary<string, DicItemState> dicItems;
            if (!_dicItemSet.TryGetDicItems(dic, out dicItems))
            {
                return new Dictionary<string, DicItemState>(StringComparer.OrdinalIgnoreCase);
            }
            return dicItems;
        }

        public bool TryGetDicItem(Guid dicItemId, out DicItemState dicItem)
        {
            if (!_initialized)
            {
                Init();
            }
            return _dicItemSet.TryGetDicItem(dicItemId, out dicItem);
        }

        public bool TryGetDicItem(DicState dicState, string dicItemCode, out DicItemState dicItem)
        {
            if (!_initialized)
            {
                Init();
            }
            dicItem = DicItemState.Empty;
            if (dicState == DicState.Empty)
            {
                return false;
            }
            return _dicItemSet.TryGetDicItem(dicState, dicItemCode, out dicItem);
        }

        public IEnumerator<DicState> GetEnumerator()
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
            lock (this)
            {
                if (_initialized) return;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _dicById.Clear();
                _dicByCode.Clear();
                var dics = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetAllDics().ToList();
                foreach (var dic in dics)
                {
                    if (_dicById.ContainsKey(dic.Id))
                    {
                        throw new AnycmdException("意外重复的字典标识" + dic.Id);
                    }
                    if (_dicByCode.ContainsKey(dic.Code))
                    {
                        throw new AnycmdException("意外重复的字典编码" + dic.Code);
                    }
                    var dicState = DicState.Create(dic);
                    _dicById.Add(dic.Id, dicState);
                    _dicByCode.Add(dic.Code, dicState);
                }
                _initialized = true;
                _host.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #endregion

        #region DicMessageHandler
        private class DicMessageHandler :
            IHandler<AddDicCommand>,
            IHandler<DicAddedEvent>,
            IHandler<UpdateDicCommand>,
            IHandler<DicUpdatedEvent>,
            IHandler<RemoveDicCommand>,
            IHandler<DicRemovedEvent>
        {
            private readonly DicSet _set;

            internal DicMessageHandler(DicSet set)
            {
                _set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(_set._host.Name));
                }
                messageDispatcher.Register((IHandler<AddDicCommand>)this);
                messageDispatcher.Register((IHandler<DicAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateDicCommand>)this);
                messageDispatcher.Register((IHandler<DicUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveDicCommand>)this);
                messageDispatcher.Register((IHandler<DicRemovedEvent>)this);
            }

            public void Handle(AddDicCommand message)
            {
                Handle(message.UserSession, message.Input, isCommand: true);
            }

            public void Handle(DicAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateDicAddedEvent))
                {
                    return;
                }
                Handle(message.UserSession, message.Output, isCommand: false);
            }

            private void Handle(IUserSession userSession, IDicCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
                var dicRepository = host.RetrieveRequiredService<IRepository<Dic>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new AnycmdException("意外的字典标识" + input.Id);
                }
                Dic entity;
                lock (this)
                {
                    if (host.DicSet.ContainsDic(input.Id.Value))
                    {
                        throw new AnycmdException("记录已经存在");
                    }
                    if (host.DicSet.ContainsDic(input.Code))
                    {
                        throw new ValidationException("重复的字典编码" + input.Code);
                    }

                    entity = Dic.Create(input);

                    var dicState = DicState.Create(entity);
                    if (!dicById.ContainsKey(entity.Id))
                    {
                        dicById.Add(entity.Id, dicState);
                    }
                    if (!dicByCode.ContainsKey(entity.Code))
                    {
                        dicByCode.Add(entity.Code, dicState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            dicRepository.Add(entity);
                            dicRepository.Context.Commit();
                        }
                        catch
                        {
                            if (dicById.ContainsKey(entity.Id))
                            {
                                dicById.Remove(entity.Id);
                            }
                            if (dicByCode.ContainsKey(entity.Code))
                            {
                                dicByCode.Remove(entity.Code);
                            }
                            dicRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateDicAddedEvent(userSession, entity, input));
                }
            }

            private class PrivateDicAddedEvent : DicAddedEvent
            {
                internal PrivateDicAddedEvent(IUserSession userSession, DicBase source, IDicCreateIo input)
                    : base(userSession, source, input)
                {

                }
            }
            public void Handle(UpdateDicCommand message)
            {
                Handle(message.UserSession, message.Output, true);
            }

            public void Handle(DicUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateDicUpdatedEvent))
                {
                    return;
                }
                Handle(message.UserSession, message.Input, false);
            }

            private void Handle(IUserSession userSession, IDicUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var dicRepository = host.RetrieveRequiredService<IRepository<Dic>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                DicState dic;
                if (host.DicSet.TryGetDic(input.Code, out dic) && dic.Id != input.Id)
                {
                    throw new ValidationException("重复的字典编码" + input.Code);
                }
                DicState bkState;
                if (!host.DicSet.TryGetDic(input.Id, out bkState))
                {
                    throw new NotExistException("记录在内存数据集中不存在" + input.Id);
                }
                Dic entity;
                var stateChanged = false;
                lock (bkState)
                {
                    entity = dicRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException("记录在持久库中不存在");
                    }

                    entity.Update(input);

                    var newState = DicState.Create(entity);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            dicRepository.Update(entity);
                            dicRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            dicRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateDicUpdatedEvent(userSession, entity, input));
                }
            }

            private void Update(DicState state)
            {
                var host = _set._host;
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
                var oldKey = dicById[state.Id].Code;
                var newKey = state.Code;
                dicById[state.Id] = state;
                // 如果字典编码有改变
                if (!dicByCode.ContainsKey(newKey))
                {
                    dicByCode.Add(newKey, state);
                    dicByCode.Remove(oldKey);
                }
                else
                {
                    dicByCode[newKey] = state;
                }
            }

            private class PrivateDicUpdatedEvent : DicUpdatedEvent
            {
                internal PrivateDicUpdatedEvent(IUserSession userSession, DicBase source, IDicUpdateIo input)
                    : base(userSession, source, input)
                {

                }
            }
            public void Handle(RemoveDicCommand message)
            {
                Handle(message.UserSession, message.EntityId, true);
            }

            public void Handle(DicRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateDicRemovedEvent))
                {
                    return;
                }
                Handle(message.UserSession, message.Source.Id, false);
            }

            private void Handle(IUserSession userSession, Guid dicId, bool isCommand)
            {
                var host = _set._host;
                var dicById = _set._dicById;
                var dicByCode = _set._dicByCode;
                var dicRepository = host.RetrieveRequiredService<IRepository<Dic>>();
                DicState bkState;
                if (!host.DicSet.TryGetDic(dicId, out bkState))
                {
                    return;
                }
                var dicItems = host.DicSet.GetDicItems(bkState);
                if (dicItems != null && dicItems.Count > 0)
                {
                    throw new ValidationException("系统字典下有字典项时不能删除");
                }
                var properties = host.EntityTypeSet.GetProperties().Where(a => a.DicId.HasValue && a.DicId.Value == dicId).ToList();
                if (properties.Count > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var property in properties)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append("、");
                        }
                        EntityTypeState entityType;
                        host.EntityTypeSet.TryGetEntityType(property.EntityTypeId, out entityType);
                        sb.Append(entityType.Code + "." + property.Code);
                    }
                    throw new ValidationException("系统字典被实体属性关联后不能删除：" + sb.ToString());
                }
                Dic entity;
                lock (this)
                {
                    entity = dicRepository.GetByKey(dicId);
                    if (entity == null)
                    {
                        return;
                    }
                    if (dicById.ContainsKey(bkState.Id))
                    {
                        if (isCommand)
                        {
                            host.MessageDispatcher.DispatchMessage(new DicRemovingEvent(userSession, entity));
                        }
                        dicById.Remove(bkState.Id);
                    }
                    if (dicByCode.ContainsKey(bkState.Code))
                    {
                        dicByCode.Remove(bkState.Code);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            dicRepository.Remove(entity);
                            dicRepository.Context.Commit();
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
                            dicRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateDicRemovedEvent(userSession, entity));
                }
            }

            private class PrivateDicRemovedEvent : DicRemovedEvent
            {
                internal PrivateDicRemovedEvent(IUserSession userSession, DicBase dic) : base(userSession, dic) { }
            }
        }
        #endregion

        // 内部类
        private sealed class DicItemSet
        {
            private readonly Dictionary<DicState, Dictionary<string, DicItemState>> _dicItemsByCode = new Dictionary<DicState, Dictionary<string, DicItemState>>();
            private readonly Dictionary<Guid, DicItemState> _dicItemById = new Dictionary<Guid, DicItemState>();
            private bool _initialized = false;
            private readonly IAcDomain _host;

            #region Ctor
            internal DicItemSet(IAcDomain host)
            {
                if (host == null)
                {
                    throw new ArgumentNullException("host");
                }
                if (host.Equals(EmptyAcDomain.SingleInstance))
                {
                    _initialized = true;
                }
                _host = host;
                var messageDispatcher = host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(host.Name));
                }
                var handles = new DicItemMessageHandler(this);
                messageDispatcher.Register((IHandler<AddDicItemCommand>)handles);
                messageDispatcher.Register((IHandler<DicItemAddedEvent>)handles);
                messageDispatcher.Register((IHandler<UpdateDicItemCommand>)handles);
                messageDispatcher.Register((IHandler<DicItemUpdatedEvent>)handles);
                messageDispatcher.Register((IHandler<RemoveDicItemCommand>)handles);
                messageDispatcher.Register((IHandler<DicItemRemovedEvent>)handles);
                messageDispatcher.Register((IHandler<DicUpdatedEvent>)handles);
                messageDispatcher.Register((IHandler<DicRemovedEvent>)handles);
            }
            #endregion

            public bool ContainsDicItem(Guid dicItemId)
            {
                if (!_initialized)
                {
                    Init();
                }
                return _dicItemById.ContainsKey(dicItemId);
            }

            public bool ContainsDicItem(DicState dic, string dicItemCode)
            {
                if (!_initialized)
                {
                    Init();
                }
                return _dicItemsByCode.ContainsKey(dic) && _dicItemsByCode[dic].ContainsKey(dicItemCode);
            }

            public bool TryGetDicItems(DicState dicState, out Dictionary<string, DicItemState> dicItems)
            {
                if (!_initialized)
                {
                    Init();
                }
                return _dicItemsByCode.TryGetValue(dicState, out dicItems);
            }

            public bool TryGetDicItem(Guid dicItemId, out DicItemState dicItem)
            {
                if (!_initialized)
                {
                    Init();
                }
                return _dicItemById.TryGetValue(dicItemId, out dicItem);
            }

            public bool TryGetDicItem(DicState dicState, string dicItemCode, out DicItemState dicItem)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_dicItemsByCode.ContainsKey(dicState))
                {
                    dicItem = DicItemState.Empty;
                    return false;
                }
                return _dicItemsByCode[dicState].TryGetValue(dicItemCode, out dicItem);
            }

            #region Init
            private void Init()
            {
                if (_initialized) return;
                lock (this)
                {
                    if (_initialized) return;
                    _dicItemsByCode.Clear();
                    _dicItemById.Clear();
                    var dicItems = _host.RetrieveRequiredService<IOriginalHostStateReader>().GetAllDicItems().OrderBy(di => di.SortCode);
                    foreach (var dicItem in dicItems)
                    {
                        DicState dic;
                        if (!_host.DicSet.TryGetDic(dicItem.DicId, out dic))
                        {
                            throw new AnycmdException("意外的字典项字典标识" + dicItem.DicId);
                        }
                        if (_dicItemById.ContainsKey(dicItem.Id))
                        {
                            throw new AnycmdException("意外重复的字典项标识" + dicItem.Id);
                        }
                        Dictionary<string, DicItemState> dicItemDic;
                        if (!_dicItemsByCode.TryGetValue(dic, out dicItemDic))
                        {
                            _dicItemsByCode.Add(dic, dicItemDic = new Dictionary<string, DicItemState>(StringComparer.OrdinalIgnoreCase));
                        }
                        if (dicItemDic.ContainsKey(dicItem.Code))
                        {
                            throw new AnycmdException("意外重复的字典项编码" + dicItem.Code);
                        }
                        var dicItemState = DicItemState.Create(_host, dicItem);
                        _dicItemsByCode[dic].Add(dicItem.Code, dicItemState);
                        _dicItemById.Add(dicItem.Id, dicItemState);
                    }
                    _initialized = true;
                }
            }

            #endregion

            #region DicItemMessageHandler
            private class DicItemMessageHandler :
                IHandler<AddDicItemCommand>,
                IHandler<DicItemAddedEvent>,
                IHandler<UpdateDicItemCommand>,
                IHandler<DicItemUpdatedEvent>,
                IHandler<RemoveDicItemCommand>,
                IHandler<DicItemRemovedEvent>,
                IHandler<DicUpdatedEvent>,
                IHandler<DicRemovedEvent>
            {
                private readonly DicItemSet _set;

                internal DicItemMessageHandler(DicItemSet set)
                {
                    _set = set;
                }

                public void Handle(DicUpdatedEvent message)
                {
                    var host = _set._host;
                    var dicItemsByCode = _set._dicItemsByCode;
                    DicState newKey;
                    if (!host.DicSet.TryGetDic(message.Source.Id, out newKey))
                    {
                        throw new AnycmdException("意外的字典标识" + message.Source.Id);
                    }
                    var oldKey = dicItemsByCode.Keys.FirstOrDefault(a => a.Id == newKey.Id);
                    if (oldKey != null && !dicItemsByCode.ContainsKey(newKey))
                    {
                        dicItemsByCode.Add(newKey, dicItemsByCode[oldKey]);
                        dicItemsByCode.Remove(oldKey);
                    }
                }

                public void Handle(DicRemovedEvent message)
                {
                    var dicItemsByCode = _set._dicItemsByCode;
                    var dicState = dicItemsByCode.Keys.FirstOrDefault(a => a.Id == message.Source.Id);
                    if (dicState != null)
                    {
                        dicItemsByCode.Remove(dicState);
                    }
                }

                public void Handle(AddDicItemCommand message)
                {
                    Handle(message.UserSession, message.Input, true);
                }

                public void Handle(DicItemAddedEvent message)
                {
                    if (message.GetType() == typeof(PrivateDicItemAddedEvent))
                    {
                        return;
                    }
                    Handle(message.UserSession, message.Output, false);
                }

                private void Handle(IUserSession userSession, IDicItemCreateIo input, bool isCommand)
                {
                    var host = _set._host;
                    var dicItemsByCode = _set._dicItemsByCode;
                    var dicItemById = _set._dicItemById;
                    var dicItemRepository = host.RetrieveRequiredService<IRepository<DicItem>>();
                    if (string.IsNullOrEmpty(input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    DicItem entity;
                    lock (this)
                    {
                        DicState dicState;
                        if (!host.DicSet.TryGetDic(input.DicId, out dicState))
                        {
                            throw new ValidationException("意外的字典项字典标识" + input.DicId);
                        }
                        if (host.DicSet.ContainsDicItem(dicState, input.Code))
                        {
                            throw new ValidationException("重复的字典项编码" + input.Code);
                        }
                        if (!input.Id.HasValue)
                        {
                            throw new ValidationException("标识为空");
                        }
                        if (host.DicSet.ContainsDicItem(input.Id.Value))
                        {
                            throw new AnycmdException("重复的字典项标识" + input.Id);
                        }

                        entity = DicItem.Create(input);

                        var dicItemState = DicItemState.Create(host, entity);
                        if (!dicItemById.ContainsKey(dicItemState.Id))
                        {
                            dicItemById.Add(dicItemState.Id, dicItemState);
                        }
                        Dictionary<string, DicItemState> dicItemDic;
                        if (!dicItemsByCode.TryGetValue(dicState, out dicItemDic))
                        {
                            dicItemsByCode.Add(dicState, dicItemDic = new Dictionary<string, DicItemState>(StringComparer.OrdinalIgnoreCase));
                        }
                        if (!dicItemDic.ContainsKey(dicItemState.Code))
                        {
                            dicItemDic.Add(dicItemState.Code, dicItemState);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                dicItemRepository.Add(entity);
                                dicItemRepository.Context.Commit();
                            }
                            catch
                            {
                                if (dicItemById.ContainsKey(entity.Id))
                                {
                                    dicItemById.Remove(entity.Id);
                                }
                                if (dicItemsByCode.TryGetValue(dicState, out dicItemDic) && dicItemDic.ContainsKey(entity.Code))
                                {
                                    dicItemDic.Remove(entity.Code);
                                }
                                dicItemRepository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateDicItemAddedEvent(userSession, entity, input));
                    }
                }

                private class PrivateDicItemAddedEvent : DicItemAddedEvent
                {
                    internal PrivateDicItemAddedEvent(IUserSession userSession, DicItemBase source, IDicItemCreateIo input)
                        : base(userSession, source, input)
                    {

                    }
                }

                public void Handle(UpdateDicItemCommand message)
                {
                    Handle(message.UserSession, message.Output, true);
                }

                public void Handle(DicItemUpdatedEvent message)
                {
                    if (message.GetType() == typeof(PrivateDicItemUpdatedEvent))
                    {
                        return;
                    }
                    Handle(message.UserSession, message.Input, false);
                }

                private void Handle(IUserSession userSession, IDicItemUpdateIo input, bool isCommand)
                {
                    var host = _set._host;
                    var dicItemRepository = host.RetrieveRequiredService<IRepository<DicItem>>();
                    if (string.IsNullOrEmpty(input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    DicItemState bkState;
                    if (!host.DicSet.TryGetDicItem(input.Id, out bkState))
                    {
                        throw new NotExistException();
                    }
                    DicItem entity;
                    bool stateChanged = false;
                    lock (bkState)
                    {
                        DicState dicState;
                        if (!host.DicSet.TryGetDic(bkState.DicId, out dicState))
                        {
                            throw new AnycmdException("意外的字典项字典标识" + bkState.DicId);
                        }
                        DicItemState dicItemState;
                        if (host.DicSet.TryGetDicItem(dicState, input.Code, out dicItemState) && dicItemState.Id != input.Id)
                        {
                            throw new ValidationException("重复的字典项编码" + input.Code);
                        }
                        entity = dicItemRepository.GetByKey(input.Id);
                        if (entity == null)
                        {
                            throw new NotExistException();
                        }

                        entity.Update(input);

                        var newState = DicItemState.Create(host, entity);
                        stateChanged = newState != bkState;
                        if (stateChanged)
                        {
                            Update(newState);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                dicItemRepository.Update(entity);
                                dicItemRepository.Context.Commit();
                            }
                            catch
                            {
                                if (stateChanged)
                                {
                                    Update(bkState);
                                }
                                dicItemRepository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand && stateChanged)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateDicItemUpdatedEvent(userSession, entity, input));
                    }
                }

                private void Update(DicItemState state)
                {
                    var host = _set._host;
                    var dicItemsByCode = _set._dicItemsByCode;
                    var dicItemById = _set._dicItemById;
                    DicState dicState;
                    if (!host.DicSet.TryGetDic(dicItemById[state.Id].DicId, out dicState))
                    {
                        throw new ValidationException("意外的字典项字典标识");
                    }
                    var oldKey = dicItemById[state.Id].Code;
                    var newKey = state.Code;
                    dicItemById[state.Id] = state;
                    // 如果字典项编码有改变
                    if (!dicItemsByCode[dicState].ContainsKey(newKey))
                    {
                        dicItemsByCode[dicState].Remove(oldKey);
                        dicItemsByCode[dicState].Add(newKey, state);
                    }
                    else
                    {
                        dicItemsByCode[dicState][newKey] = state;
                    }
                }

                private class PrivateDicItemUpdatedEvent : DicItemUpdatedEvent
                {
                    internal PrivateDicItemUpdatedEvent(IUserSession userSession, DicItemBase source, IDicItemUpdateIo input)
                        : base(userSession, source, input)
                    {

                    }
                }

                public void Handle(RemoveDicItemCommand message)
                {
                    Handle(message.UserSession, message.EntityId, true);
                }

                public void Handle(DicItemRemovedEvent message)
                {
                    if (message.GetType() == typeof(PrivateDicItemRemovedEvent))
                    {
                        return;
                    }
                    Handle(message.UserSession, message.Source.Id, false);
                }

                private void Handle(IUserSession userSession, Guid dicItemId, bool isCommand)
                {
                    var host = _set._host;
                    var dicItemsByCode = _set._dicItemsByCode;
                    var dicItemById = _set._dicItemById;
                    var dicItemRepository = host.RetrieveRequiredService<IRepository<DicItem>>();
                    DicItemState bkState;
                    if (!host.DicSet.TryGetDicItem(dicItemId, out bkState))
                    {
                        return;
                    }
                    DicItem entity;
                    lock (bkState)
                    {
                        entity = dicItemRepository.GetByKey(dicItemId);
                        if (entity == null)
                        {
                            return;
                        }
                        if (dicItemById.ContainsKey(bkState.Id))
                        {
                            DicState dic;
                            if (!host.DicSet.TryGetDic(bkState.DicId, out dic))
                            {
                                throw new AnycmdException("意外的字典标识" + bkState.DicId);
                            }
                            if (dicItemsByCode.ContainsKey(dic) && dicItemsByCode[dic].ContainsKey(bkState.Code))
                            {
                                dicItemsByCode[dic].Remove(bkState.Code);
                            }
                            dicItemById.Remove(bkState.Id);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                dicItemRepository.Remove(entity);
                                dicItemRepository.Context.Commit();
                            }
                            catch
                            {
                                if (!dicItemById.ContainsKey(bkState.Id))
                                {
                                    DicState dic;
                                    if (!host.DicSet.TryGetDic(bkState.DicId, out dic))
                                    {
                                        throw new AnycmdException("意外的字典标识" + bkState.DicId);
                                    }
                                    Dictionary<string, DicItemState> dicItemDic;
                                    if (!dicItemsByCode.TryGetValue(dic, out dicItemDic))
                                    {
                                        dicItemsByCode.Add(dic, dicItemDic = new Dictionary<string, DicItemState>(StringComparer.OrdinalIgnoreCase));
                                    }
                                    if (!dicItemDic.ContainsKey(bkState.Code))
                                    {
                                        dicItemDic.Add(bkState.Code, bkState);
                                    }
                                    dicItemById.Add(bkState.Id, bkState);
                                }
                                dicItemRepository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateDicItemRemovedEvent(userSession, entity));
                    }
                }

                private class PrivateDicItemRemovedEvent : DicItemRemovedEvent
                {
                    internal PrivateDicItemRemovedEvent(IUserSession userSession, DicItemBase source)
                        : base(userSession, source)
                    {

                    }
                }
            }
            #endregion
        }
    }
}
