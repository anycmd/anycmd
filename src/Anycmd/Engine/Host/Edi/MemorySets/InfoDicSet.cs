
namespace Anycmd.Engine.Host.Edi.MemorySets
{
    using Bus;
    using Engine.Edi;
    using Engine.Edi.Abstractions;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Entities;
    using Exceptions;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Util;
    using dicCode = System.String;
    using dicId = System.Guid;
    using dicItemCode = System.String;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class InfoDicSet : IInfoDicSet, IMemorySet
    {
        public static readonly IInfoDicSet Empty = new InfoDicSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<dicId, InfoDicState> _infoDicDicById = new Dictionary<dicId, InfoDicState>();
        private readonly Dictionary<dicCode, InfoDicState> _infoDicDicByCode = new Dictionary<dicCode, InfoDicState>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<InfoDicState, Dictionary<dicItemCode, InfoDicItemState>> _infoDicItemByDic = new Dictionary<InfoDicState, Dictionary<dicItemCode, InfoDicItemState>>();
        private readonly Dictionary<Guid, InfoDicItemState> _infoDicItemDic = new Dictionary<dicId, InfoDicItemState>();
        private readonly List<InfoDicItemState> _emptyInfoDicItems = new List<InfoDicItemState>();
        private bool _initialized = false;
        private static readonly object Locker = new object();

        private readonly Guid _id = Guid.NewGuid();
        private readonly IAcDomain _acDomain;

        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 构造并接入总线
        /// </summary>
        internal InfoDicSet(IAcDomain acDomain)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicId"></param>
        /// <param name="infoDic"></param>
        /// <returns></returns>
        public bool TryGetInfoDic(dicId dicId, out InfoDicState infoDic)
        {
            if (!_initialized)
            {
                Init();
            }
            return _infoDicDicById.TryGetValue(dicId, out infoDic);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicCode"></param>
        /// <param name="infoDic"></param>
        /// <returns></returns>
        public bool TryGetInfoDic(dicCode dicCode, out InfoDicState infoDic)
        {
            if (!_initialized)
            {
                Init();
            }
            if (dicCode != null) return _infoDicDicByCode.TryGetValue(dicCode, out infoDic);
            infoDic = null;
            return false;
        }

        /// <summary>
        /// 根据字典Id索引字典项集合
        /// </summary>
        /// <param name="infoDic">字典Id</param>
        /// <returns>字典项集合</returns>
        public IReadOnlyCollection<InfoDicItemState> GetInfoDicItems(InfoDicState infoDic)
        {
            if (!_initialized)
            {
                Init();
            }
            return !_infoDicItemByDic.ContainsKey(infoDic) ? _emptyInfoDicItems : new List<InfoDicItemState>(_infoDicItemByDic[infoDic].Values.OrderBy(item => item.SortCode));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoDic"></param>
        /// <param name="itemCode"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryGetInfoDicItem(InfoDicState infoDic, dicItemCode itemCode, out InfoDicItemState item)
        {
            if (!_initialized)
            {
                Init();
            }
            if (_infoDicItemByDic.ContainsKey(infoDic))
                return _infoDicItemByDic[infoDic].TryGetValue(itemCode, out item);
            item = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicItemId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryGetInfoDicItem(Guid dicItemId, out InfoDicItemState item)
        {
            if (!_initialized)
            {
                Init();
            }
            return _infoDicItemDic.TryGetValue(dicItemId, out item);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Refresh()
        {
            if (_initialized)
            {
                _initialized = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<InfoDicState> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _infoDicDicById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _infoDicDicById.Values.GetEnumerator();
        }

        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _infoDicDicById.Clear();
                _infoDicDicByCode.Clear();
                _infoDicItemByDic.Clear();
                _infoDicItemDic.Clear();
                var allInfoDics = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetInfoDics();
                foreach (var infoDic in allInfoDics)
                {
                    var infoDicState = InfoDicState.Create(_acDomain, infoDic);
                    _infoDicDicById.Add(infoDic.Id, infoDicState);
                    _infoDicDicByCode.Add(infoDic.Code, infoDicState);
                    _infoDicItemByDic.Add(infoDicState, new Dictionary<string, InfoDicItemState>(StringComparer.OrdinalIgnoreCase));
                }
                var allDicItems = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetInfoDicItems();

                foreach (var infoDicItem in allDicItems)
                {
                    var infoDicItemState = InfoDicItemState.Create(infoDicItem);
                    var infoDic = _infoDicDicById[infoDicItem.InfoDicId];
                    _infoDicItemByDic[infoDic].Add(infoDicItem.Code, infoDicItemState);
                    _infoDicItemDic.Add(infoDicItem.Id, infoDicItemState);
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #region MessageHandler
        private class MessageHandler:
            IHandler<AddInfoDicCommand>,
            IHandler<InfoDicAddedEvent>,
            IHandler<UpdateInfoDicCommand>,
            IHandler<InfoDicUpdatedEvent>,
            IHandler<RemoveInfoDicCommand>,
            IHandler<InfoDicRemovedEvent>,
            IHandler<AddInfoDicItemCommand>,
            IHandler<InfoDicItemAddedEvent>,
            IHandler<UpdateInfoDicItemCommand>,
            IHandler<InfoDicItemUpdatedEvent>,
            IHandler<RemoveInfoDicItemCommand>,
            IHandler<InfoDicItemRemovedEvent>
        {
            private readonly InfoDicSet _set;

            internal MessageHandler(InfoDicSet set)
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
                messageDispatcher.Register((IHandler<AddInfoDicCommand>)this);
                messageDispatcher.Register((IHandler<InfoDicAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateInfoDicCommand>)this);
                messageDispatcher.Register((IHandler<InfoDicUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveInfoDicCommand>)this);
                messageDispatcher.Register((IHandler<InfoDicRemovedEvent>)this);
                messageDispatcher.Register((IHandler<AddInfoDicItemCommand>)this);
                messageDispatcher.Register((IHandler<InfoDicItemAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateInfoDicItemCommand>)this);
                messageDispatcher.Register((IHandler<InfoDicItemUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveInfoDicItemCommand>)this);
                messageDispatcher.Register((IHandler<InfoDicItemRemovedEvent>)this);
            }

            public void Handle(AddInfoDicCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(InfoDicAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateInfoDicAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IInfoDicCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var infoDicDicById = _set._infoDicDicById;
                var infoDicDicByCode = _set._infoDicDicByCode;
                var infoDicRepository = acDomain.RetrieveRequiredService<IRepository<InfoDic>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                InfoDic entity;
                lock (Locker)
                {
                    InfoDicState infoDic;
                    if (acDomain.NodeHost.InfoDics.TryGetInfoDic(input.Id.Value, out infoDic))
                    {
                        throw new ValidationException("给定标识标识的记录已经存在");
                    }
                    if (acDomain.NodeHost.InfoDics.TryGetInfoDic(input.Code, out infoDic) && infoDic.Id != input.Id.Value)
                    {
                        throw new ValidationException("重复的编码");
                    }

                    entity = InfoDic.Create(input);

                    var state = InfoDicState.Create(acDomain, entity);
                    if (!infoDicDicById.ContainsKey(entity.Id))
                    {
                        infoDicDicById.Add(entity.Id, state);
                    }
                    if (!infoDicDicByCode.ContainsKey(entity.Code))
                    {
                        infoDicDicByCode.Add(entity.Code, state);
                    }
                    if (isCommand)
                    {
                        try
                        {

                            infoDicRepository.Add(entity);
                            infoDicRepository.Context.Commit();
                        }
                        catch
                        {
                            if (infoDicDicById.ContainsKey(entity.Id))
                            {
                                infoDicDicById.Remove(entity.Id);
                            }
                            if (infoDicDicByCode.ContainsKey(entity.Code))
                            {
                                infoDicDicByCode.Remove(entity.Code);
                            }
                            infoDicRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateInfoDicAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateInfoDicAddedEvent : InfoDicAddedEvent, IPrivateEvent
            {
                public PrivateInfoDicAddedEvent(IAcSession acSession, InfoDicBase source, IInfoDicCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }
            public void Handle(UpdateInfoDicCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(InfoDicUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateInfoDicUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IInfoDicUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var infoDicRepository = acDomain.RetrieveRequiredService<IRepository<InfoDic>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                InfoDicState infoDic;
                if (!acDomain.NodeHost.InfoDics.TryGetInfoDic(input.Id, out infoDic))
                {
                    throw new NotExistException();
                }
                if (acDomain.NodeHost.InfoDics.TryGetInfoDic(input.Code, out infoDic) && infoDic.Id != input.Id)
                {
                    throw new ValidationException("重复的编码");
                }
                var entity = infoDicRepository.GetByKey(input.Id);
                if (entity == null)
                {
                    throw new NotExistException();
                }
                var bkState = InfoDicState.Create(acDomain, entity);

                entity.Update(input);

                var newState = InfoDicState.Create(acDomain, entity);
                bool stateChanged = newState != bkState;
                lock (Locker)
                {
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            infoDicRepository.Update(entity);
                            infoDicRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            infoDicRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateInfoDicUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(InfoDicState state)
            {
                var infoDicDicById = _set._infoDicDicById;
                var infoDicDicByCode = _set._infoDicDicByCode;
                var newKey = state.Code;
                var oldKey = infoDicDicById[state.Id].Code;
                infoDicDicById[state.Id] = state;
                if (!infoDicDicByCode.ContainsKey(newKey))
                {
                    infoDicDicByCode.Add(newKey, state);
                    infoDicDicByCode.Remove(oldKey);
                }
                else
                {
                    infoDicDicByCode[newKey] = state;
                }
            }

            private class PrivateInfoDicUpdatedEvent : InfoDicUpdatedEvent, IPrivateEvent
            {
                public PrivateInfoDicUpdatedEvent(IAcSession acSession, InfoDicBase source, IInfoDicUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }
            public void Handle(RemoveInfoDicCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(InfoDicRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateInfoDicRemovedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid infoDicId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var infoDicDicById = _set._infoDicDicById;
                var infoDicDicByCode = _set._infoDicDicByCode;
                var infoDicRepository = acDomain.RetrieveRequiredService<IRepository<InfoDic>>();
                InfoDicState infoDic;
                if (!acDomain.NodeHost.InfoDics.TryGetInfoDic(infoDicId, out infoDic))
                {
                    return;
                }
                var infoDicItems = acDomain.NodeHost.InfoDics.GetInfoDicItems(infoDic);
                if (infoDicItems != null && infoDicItems.Count > 0)
                {
                    throw new ValidationException("信息字典下有信息字典项时不能删除");
                }
                InfoDic entity = infoDicRepository.GetByKey(infoDicId);
                if (entity == null)
                {
                    return;
                }
                var bkState = InfoDicState.Create(acDomain, entity);
                lock (Locker)
                {
                    if (infoDicDicById.ContainsKey(entity.Id))
                    {
                        infoDicDicById.Remove(entity.Id);
                    }
                    if (infoDicDicByCode.ContainsKey(entity.Code))
                    {
                        infoDicDicByCode.Remove(entity.Code);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            infoDicRepository.Remove(entity);
                            infoDicRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!infoDicDicById.ContainsKey(entity.Id))
                            {
                                infoDicDicById.Add(entity.Id, bkState);
                            }
                            if (!infoDicDicByCode.ContainsKey(entity.Code))
                            {
                                infoDicDicByCode.Add(entity.Code, bkState);
                            }
                            infoDicRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateInfoDicRemovedEvent(acSession, entity));
                }
            }

            private class PrivateInfoDicRemovedEvent : InfoDicRemovedEvent, IPrivateEvent
            {
                internal PrivateInfoDicRemovedEvent(IAcSession acSession, InfoDicBase source)
                    : base(acSession, source)
                {

                }
            }
            public void Handle(AddInfoDicItemCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(InfoDicItemAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateInfoDicItemAddedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IInfoDicItemCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var infoDicItemDic = _set._infoDicItemDic;
                var infoDicItemByDic = _set._infoDicItemByDic;
                var infoDicItemRepository = acDomain.RetrieveRequiredService<IRepository<InfoDicItem>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                InfoDicState infoDic;
                if (!acDomain.NodeHost.InfoDics.TryGetInfoDic(input.InfoDicId, out infoDic))
                {
                    throw new ValidationException("意外的信息字典标识");
                }
                InfoDicItemState infoDicItem;
                if (acDomain.NodeHost.InfoDics.TryGetInfoDicItem(input.Id.Value, out infoDicItem))
                {
                    throw new ValidationException("给定的标识标识的记录已经存在");
                }
                if (acDomain.NodeHost.InfoDics.TryGetInfoDicItem(infoDic, input.Code, out infoDicItem))
                {
                    throw new ValidationException("重复的编码");
                }

                var entity = InfoDicItem.Create(input);

                lock (Locker)
                {
                    var state = InfoDicItemState.Create(entity);
                    if (!infoDicItemDic.ContainsKey(entity.Id))
                    {
                        infoDicItemDic.Add(entity.Id, state);
                    }
                    if (!infoDicItemByDic.ContainsKey(infoDic))
                    {
                        infoDicItemByDic.Add(infoDic, new Dictionary<dicItemCode, InfoDicItemState>(StringComparer.OrdinalIgnoreCase));
                    }
                    if (!infoDicItemByDic[infoDic].ContainsKey(entity.Code))
                    {
                        infoDicItemByDic[infoDic].Add(entity.Code, state);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            infoDicItemRepository.Add(entity);
                            infoDicItemRepository.Context.Commit();
                        }
                        catch
                        {
                            if (infoDicItemDic.ContainsKey(entity.Id))
                            {
                                infoDicItemDic.Remove(entity.Id);
                            }
                            if (infoDicItemByDic.ContainsKey(infoDic) && infoDicItemByDic[infoDic].ContainsKey(entity.Code))
                            {
                                infoDicItemByDic[infoDic].Remove(entity.Code);
                            }
                            infoDicItemRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateInfoDicItemAddedEvent(acSession, entity, input));
                }
            }

            private class PrivateInfoDicItemAddedEvent : InfoDicItemAddedEvent, IPrivateEvent
            {
                internal PrivateInfoDicItemAddedEvent(IAcSession acSession, InfoDicItemBase source, IInfoDicItemCreateIo input)
                    : base(acSession, source, input)
                {

                }
            }
            public void Handle(UpdateInfoDicItemCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(InfoDicItemUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateInfoDicItemUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IInfoDicItemUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var infoDicItemRepository = acDomain.RetrieveRequiredService<IRepository<InfoDicItem>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                InfoDicState infoDic;
                if (!acDomain.NodeHost.InfoDics.TryGetInfoDic(input.InfoDicId, out infoDic))
                {
                    throw new ValidationException("意外的信息字典项字典标识");
                }
                InfoDicItemState infoDicItem;
                if (!acDomain.NodeHost.InfoDics.TryGetInfoDicItem(input.Id, out infoDicItem))
                {
                    throw new NotExistException();
                }
                if (acDomain.NodeHost.InfoDics.TryGetInfoDicItem(infoDic, input.Code, out infoDicItem) && infoDicItem.Id != input.Id)
                {
                    throw new ValidationException("重复的编码");
                }
                var entity = infoDicItemRepository.GetByKey(input.Id);
                if (entity == null)
                {
                    throw new NotExistException();
                }
                var bkState = InfoDicItemState.Create(entity);

                entity.Update(input);

                var newState = InfoDicItemState.Create(entity);
                bool stateChanged = newState != bkState;
                lock (Locker)
                {
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            infoDicItemRepository.Update(entity);
                            infoDicItemRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            infoDicItemRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateInfoDicItemUpdatedEvent(acSession, entity, input));
                }
            }

            private void Update(InfoDicItemState state)
            {
                var acDomain = _set._acDomain;
                var infoDicItemDic = _set._infoDicItemDic;
                var infoDicItemByDic = _set._infoDicItemByDic;
                var oldState = infoDicItemDic[state.Id];
                var newKey = state.Code;
                var oldKey = oldState.Code;
                InfoDicState infoDic;
                if (!acDomain.NodeHost.InfoDics.TryGetInfoDic(oldState.InfoDicId, out infoDic))
                {
                    throw new AnycmdException("意外的信息字典标识" + oldState.InfoDicId);
                }
                infoDicItemDic[state.Id] = state;
                if (!infoDicItemByDic[infoDic].ContainsKey(newKey))
                {
                    infoDicItemByDic[infoDic].Add(newKey, state);
                    infoDicItemByDic[infoDic].Remove(oldKey);
                }
                else
                {
                    infoDicItemByDic[infoDic][newKey] = state;
                }
            }

            private class PrivateInfoDicItemUpdatedEvent : InfoDicItemUpdatedEvent, IPrivateEvent
            {
                internal PrivateInfoDicItemUpdatedEvent(IAcSession acSession, InfoDicItemBase source, IInfoDicItemUpdateIo input)
                    : base(acSession, source, input)
                {

                }
            }

            public void Handle(RemoveInfoDicItemCommand message)
            {
                this.HandleItem(message.AcSession, message.EntityId, true);
            }

            public void Handle(InfoDicItemRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateInfoDicItemRemovedEvent))
                {
                    return;
                }
                this.HandleItem(message.AcSession, message.Source.Id, false);
            }

            private void HandleItem(IAcSession acSession, Guid infoDicItemId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var infoDicItemDic = _set._infoDicItemDic;
                var infoDicItemByDic = _set._infoDicItemByDic;
                var infoDicItemRepository = acDomain.RetrieveRequiredService<IRepository<InfoDicItem>>();
                InfoDicItemState infoDicItem;
                if (!acDomain.NodeHost.InfoDics.TryGetInfoDicItem(infoDicItemId, out infoDicItem))
                {
                    return;
                }
                InfoDicState infoDic;
                if (!acDomain.NodeHost.InfoDics.TryGetInfoDic(infoDicItem.InfoDicId, out infoDic))
                {
                    throw new AnycmdException("意外的信息字典项字典标识");
                }
                InfoDicItem entity = infoDicItemRepository.GetByKey(infoDicItemId);
                if (entity == null)
                {
                    return;
                }
                var bkState = InfoDicItemState.Create(entity);
                lock (Locker)
                {
                    if (infoDicItemDic.ContainsKey(entity.Id))
                    {
                        infoDicItemDic.Remove(entity.Id);
                    }
                    if (infoDicItemByDic.ContainsKey(infoDic) && infoDicItemByDic[infoDic].ContainsKey(entity.Code))
                    {
                        infoDicItemByDic[infoDic].Remove(entity.Code);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            infoDicItemRepository.Remove(entity);
                            infoDicItemRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!infoDicItemDic.ContainsKey(entity.Id))
                            {
                                infoDicItemDic.Add(entity.Id, bkState);
                            }
                            if (!infoDicItemByDic.ContainsKey(infoDic))
                            {
                                infoDicItemByDic.Add(infoDic, new Dictionary<dicItemCode, InfoDicItemState>(StringComparer.OrdinalIgnoreCase));
                            }
                            if (!infoDicItemByDic[infoDic].ContainsKey(entity.Code))
                            {
                                infoDicItemByDic[infoDic].Add(entity.Code, bkState);
                            }
                            infoDicItemRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new PrivateInfoDicItemRemovedEvent(acSession, entity));
                }
            }

            private class PrivateInfoDicItemRemovedEvent : InfoDicItemRemovedEvent, IPrivateEvent
            {
                internal PrivateInfoDicItemRemovedEvent(IAcSession acSession, InfoDicItemBase source)
                    : base(acSession, source)
                {

                }
            }
        }
        #endregion
    }
}
