using System;
    
namespace Anycmd.Engine.Host.Edi.MemorySets
{
    using Bus;
    using Engine.Ac;
    using Engine.Edi;
    using Engine.Edi.Abstractions;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Entities;
    using Exceptions;
    using Hecp;
    using Host;
    using InOuts;
    using Messages;
    using Repositories;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Util;
    using catalogId = System.Guid;
    using ontologyId = System.Guid;
    using topicCode = System.String;

    /// <summary>
    /// 
    /// </summary>
    internal sealed class OntologySet : IOntologySet, IMemorySet
    {
        public static readonly IOntologySet Empty = new OntologySet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<string, OntologyDescriptor> _ontologyDicByCode = new Dictionary<string, OntologyDescriptor>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<ontologyId, OntologyDescriptor> _ontologyDicById = new Dictionary<Guid, OntologyDescriptor>();
        private bool _initialized = false;
        private static readonly object Locker = new object();

        private readonly Guid _id = Guid.NewGuid();
        private readonly ElementSet _elementSet;
        private readonly ActionSet _actionSet;
        private readonly InfoGroupSet _infoGroupSet;
        private readonly OntologyCatalogSet _catalogSet;
        private readonly TopicSet _topics;
        private readonly ArchiveSet _archiveSet;

        private readonly IAcDomain _acDomain;

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        #region Ctor
        /// <summary>
        /// 构造并接入总线
        /// </summary>
        public OntologySet(IAcDomain acDomain)
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
            this._elementSet = new ElementSet(acDomain);
            this._actionSet = new ActionSet(acDomain);
            this._infoGroupSet = new InfoGroupSet(acDomain);
            this._catalogSet = new OntologyCatalogSet(acDomain);
            this._topics = new TopicSet(acDomain);
            this._archiveSet = new ArchiveSet(acDomain);
        }
        #endregion

        #region this[string ontologyCode]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontologyCode"></param>
        /// <returns></returns>
        /// <exception cref="AnycmdException">当本体码非法时抛出</exception>
        public OntologyDescriptor this[string ontologyCode]
        {
            get
            {
                if (ontologyCode == null)
                {
                    throw new ArgumentNullException("ontologyCode");
                }
                if (!_initialized)
                {
                    Init();
                }
                if (!_ontologyDicByCode.ContainsKey(ontologyCode))
                {
                    throw new AnycmdException("意外的本体码");
                }

                return _ontologyDicByCode[ontologyCode];
            }
        }
        #endregion

        #region this[Guid ontologyId]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontologyId"></param>
        /// <returns></returns>
        /// <exception cref="AnycmdException">当本体标识非法时抛出</exception>
        public OntologyDescriptor this[ontologyId ontologyId]
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_ontologyDicById.ContainsKey(ontologyId))
                {
                    throw new AnycmdException("意外的本体Id");
                }

                return _ontologyDicById[ontologyId];
            }
        }
        #endregion

        #region TryGetOntology
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontologyCode"></param>
        /// <param name="ontology"></param>
        /// <returns></returns>
        public bool TryGetOntology(string ontologyCode, out OntologyDescriptor ontology)
        {
            if (ontologyCode == null)
            {
                throw new ArgumentNullException("ontologyCode");
            }
            if (!_initialized)
            {
                Init();
            }
            return _ontologyDicByCode.TryGetValue(ontologyCode, out ontology);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontologyId"></param>
        /// <param name="ontology"></param>
        /// <returns></returns>
        public bool TryGetOntology(ontologyId ontologyId, out OntologyDescriptor ontology)
        {
            if (!_initialized)
            {
                Init();
            }

            return _ontologyDicById.TryGetValue(ontologyId, out ontology);
        }
        #endregion

        #region TryGetElement
        public bool TryGetElement(ontologyId elementId, out ElementDescriptor element)
        {
            if (!_initialized)
            {
                Init();
            }
            return _elementSet.TryGetElement(elementId, out element);
        }
        #endregion

        #region GetElement
        public ElementDescriptor GetElement(ontologyId elementId)
        {
            if (!_initialized)
            {
                Init();
            }
            return _elementSet.GetElement(elementId);
        }
        #endregion

        #region GetElements
        public IReadOnlyDictionary<string, ElementDescriptor> GetElements(OntologyDescriptor ontology)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (!_initialized)
            {
                Init();
            }
            return _elementSet.GetElements(ontology);
        }
        #endregion

        #region GetActons
        public IReadOnlyDictionary<Verb, ActionState> GetActons(OntologyDescriptor ontology)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (!_initialized)
            {
                Init();
            }
            return _actionSet.GetActons(ontology);
        }
        #endregion

        #region GetAction
        public ActionState GetAction(Guid actionId)
        {
            if (!_initialized)
            {
                Init();
            }
            return _actionSet.GetAction(actionId);
        }
        #endregion

        #region GetInfoGroups
        public IList<InfoGroupState> GetInfoGroups(OntologyDescriptor ontology)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (!_initialized)
            {
                Init();
            }
            return _infoGroupSet.GetInfoGroups(ontology);
        }
        #endregion

        #region GetOntologyCatalogs
        public IReadOnlyDictionary<CatalogState, OntologyCatalogState> GetOntologyCatalogs(OntologyDescriptor ontology)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (!_initialized)
            {
                Init();
            }
            return _catalogSet[ontology];
        }
        #endregion

        #region GetEventSubjects
        public IReadOnlyDictionary<string, TopicState> GetEventSubjects(OntologyDescriptor ontology)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (!_initialized)
            {
                Init();
            }
            return _topics[ontology];
        }
        #endregion

        #region TryGetArchive
        public bool TryGetArchive(Guid archiveId, out ArchiveState archive)
        {
            if (!_initialized)
            {
                Init();
            }
            return _archiveSet.TryGetArchive(archiveId, out archive);
        }
        #endregion

        #region GetArchives
        public IReadOnlyCollection<ArchiveState> GetArchives(OntologyDescriptor ontology)
        {
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (!_initialized)
            {
                Init();
            }
            return _archiveSet.GetArchives(ontology);
        }
        #endregion

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

        #region IEnumerator
        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<OntologyDescriptor> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _ontologyDicByCode.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _ontologyDicByCode.Values.GetEnumerator();
        }
        #endregion

        #region Init
        private void Init()
        {
            if (_initialized) return;
            lock (Locker)
            {
                if (_initialized) return;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitingEvent(this));
                _ontologyDicByCode.Clear();
                _ontologyDicById.Clear();
                var allOntologies = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetOntologies().OrderBy(s => s.SortCode);
                foreach (var ontology in allOntologies)
                {
                    var ontologyDescriptor = new OntologyDescriptor(_acDomain, OntologyState.Create(ontology), ontology.Id);
                    _ontologyDicByCode.Add(ontology.Code, ontologyDescriptor);
                    _ontologyDicById.Add(ontology.Id, ontologyDescriptor);
                }
                _initialized = true;
                _acDomain.MessageDispatcher.DispatchMessage(new MemorySetInitializedEvent(this));
            }
        }

        #endregion

        #region MessageHandler
        private class MessageHandler :
            IHandler<AddOntologyCommand>,
            IHandler<OntologyAddedEvent>,
            IHandler<UpdateOntologyCommand>,
            IHandler<OntologyUpdatedEvent>,
            IHandler<RemoveOntologyCommand>,
            IHandler<OntologyRemovedEvent>
        {
            private readonly OntologySet _set;

            public MessageHandler(OntologySet set)
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
                messageDispatcher.Register((IHandler<AddOntologyCommand>)this);
                messageDispatcher.Register((IHandler<OntologyAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateOntologyCommand>)this);
                messageDispatcher.Register((IHandler<OntologyUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveOntologyCommand>)this);
                messageDispatcher.Register((IHandler<OntologyRemovedEvent>)this);
            }

            public void Handle(AddOntologyCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(OntologyAddedEvent message)
            {
                if (message.IsPrivate)
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IOntologyCreateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (!input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                Ontology entity;
                lock (Locker)
                {
                    OntologyDescriptor ontology;
                    if (acDomain.NodeHost.Ontologies.TryGetOntology(input.Id.Value, out ontology))
                    {
                        throw new ValidationException("给定的标识标识的记录已经存在");
                    }

                    entity = Ontology.Create(input);

                    var descriptor = new OntologyDescriptor(acDomain, OntologyState.Create(entity), entity.Id);
                    if (!_set._ontologyDicByCode.ContainsKey(entity.Code))
                    {
                        _set._ontologyDicByCode.Add(entity.Code, descriptor);
                    }
                    if (!_set._ontologyDicById.ContainsKey(entity.Id))
                    {
                        _set._ontologyDicById.Add(entity.Id, descriptor);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            ontologyRepository.Add(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            if (_set._ontologyDicByCode.ContainsKey(entity.Code))
                            {
                                _set._ontologyDicByCode.Remove(entity.Code);
                            }
                            if (_set._ontologyDicById.ContainsKey(entity.Id))
                            {
                                _set._ontologyDicById.Remove(entity.Id);
                            }
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new OntologyAddedEvent(acSession, entity, input, isPrivate: true));
                }
            }

            public void Handle(UpdateOntologyCommand message)
            {
                this.Handle(message.AcSession, message.Input, true);
            }

            public void Handle(OntologyUpdatedEvent message)
            {
                if (message.IsPrivate)
                {
                    return;
                }
                this.Handle(message.AcSession, message.Output, false);
            }

            private void Handle(IAcSession acSession, IOntologyUpdateIo input, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                OntologyDescriptor bkState;
                if (!acDomain.NodeHost.Ontologies.TryGetOntology(input.Id, out bkState))
                {
                    throw new NotExistException();
                }
                Ontology entity;
                var stateChanged = false;
                lock (Locker)
                {
                    OntologyDescriptor ontology;
                    if (!acDomain.NodeHost.Ontologies.TryGetOntology(input.Id, out ontology))
                    {
                        throw new NotExistException();
                    }
                    if (acDomain.NodeHost.Ontologies.TryGetOntology(input.Code, out ontology) && ontology.Ontology.Id != input.Id)
                    {
                        throw new ValidationException("重复的编码");
                    }
                    entity = ontologyRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }

                    entity.Update(input);

                    var newState = new OntologyDescriptor(acDomain, OntologyState.Create(entity), entity.Id);
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            ontologyRepository.Update(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new OntologyUpdatedEvent(acSession, entity, input, isPrivate: true));
                }
            }

            private void Update(OntologyDescriptor state)
            {
                var oldState = _set._ontologyDicById[state.Ontology.Id];
                _set._ontologyDicById[state.Ontology.Id] = state;
                if (!_set._ontologyDicByCode.ContainsKey(state.Ontology.Code))
                {
                    _set._ontologyDicByCode.Add(state.Ontology.Code, state);
                    _set._ontologyDicByCode.Remove(oldState.Ontology.Code);
                }
                else
                {
                    _set._ontologyDicByCode[oldState.Ontology.Code] = state;
                }
            }

            public void Handle(RemoveOntologyCommand message)
            {
                this.Handle(message.AcSession, message.EntityId, true);
            }

            public void Handle(OntologyRemovedEvent message)
            {
                if (message.IsPrivate)
                {
                    return;
                }
                this.Handle(message.AcSession, message.Source.Id, false);
            }

            private void Handle(IAcSession acSession, Guid ontologyId, bool isCommand)
            {
                var acDomain = _set._acDomain;
                var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                OntologyDescriptor ontology;
                if (!acDomain.NodeHost.Ontologies.TryGetOntology(ontologyId, out ontology))
                {
                    return;
                }
                var entity = ontologyRepository.GetByKey(ontologyId);
                if (entity == null)
                {
                    return;
                }
                if (ontology.Elements != null && ontology.Elements.Count > 0)
                {
                    throw new ValidationException("本体下有本体元素时不能删除");
                }
                if (ontology.Archives != null && ontology.Archives.Count > 0)
                {
                    throw new ValidationException("本体下有归档记录时不能删除");
                }
                if (ontology.Catalogs != null && ontology.Catalogs.Count > 0)
                {
                    throw new ValidationException("本体下有建立的目录时不能删除");
                }
                if (ontology.Actions != null && ontology.Actions.Count > 0)
                {
                    throw new ValidationException("本体下定义了动作时不能删除");
                }
                if (ontology.Topics != null && ontology.Topics.Count > 0)
                {
                    throw new ValidationException("本体下定义了事件主题时不能删除");
                }
                if (ontology.Processes != null && ontology.Processes.Any())
                {
                    throw new ValidationException("本体下有进程记录时不能删除");
                }
                if (ontology.InfoGroups != null && ontology.InfoGroups.Any())
                {
                    throw new ValidationException("本体下建立了信息组时不能删除");
                }
                if (acDomain.NodeHost.Nodes.GetNodeOntologyCares().Any(a => a.OntologyId == entity.Id))
                {
                    throw new ValidationException("有节点关心了该本体时不能删除");
                }
                if (acDomain.NodeHost.Nodes.GetNodeOntologyCatalogs().Any(a => a.OntologyId == entity.Id))
                {
                    throw new ValidationException("有节点关心了该本体下的目录时不能删除");
                }
                if (acDomain.RetrieveRequiredService<IRepository<Batch>>().AsQueryable().Any(a => a.OntologyId == entity.Id))
                {
                    throw new ValidationException("本体下有批处理记录时不能删除");
                }
                var bkState = ontology;
                lock (Locker)
                {
                    try
                    {
                        if (_set._ontologyDicById.ContainsKey(entity.Id))
                        {
                            _set._ontologyDicById.Remove(entity.Id);
                        }
                        if (_set._ontologyDicByCode.ContainsKey(entity.Code))
                        {
                            _set._ontologyDicByCode.Remove(entity.Code);
                        }
                        ontologyRepository.Remove(entity);
                        ontologyRepository.Context.Commit();
                    }
                    catch
                    {
                        if (!_set._ontologyDicById.ContainsKey(entity.Id))
                        {
                            _set._ontologyDicById.Add(entity.Id, bkState);
                        }
                        if (!_set._ontologyDicByCode.ContainsKey(entity.Code))
                        {
                            _set._ontologyDicByCode.Add(entity.Code, bkState);
                        }
                        ontologyRepository.Context.Rollback();
                        throw;
                    }
                }
                if (isCommand)
                {
                    acDomain.MessageDispatcher.DispatchMessage(new OntologyRemovedEvent(acSession, entity, isPrivate: true));
                }
            }
        }
        #endregion

        // 内部类
        #region ElementSet
        private sealed class ElementSet
        {
            private static readonly object ElementSetLocker = new object();
            #region Private Fields
            private readonly Dictionary<Guid, ElementDescriptor> _elementDicById = new Dictionary<Guid, ElementDescriptor>();
            private readonly Dictionary<OntologyDescriptor, Dictionary<string, ElementDescriptor>> _elementDicByOntology = new Dictionary<OntologyDescriptor, Dictionary<string, ElementDescriptor>>();
            private bool _initialized = false;
            private readonly Guid _id = Guid.NewGuid();
            #endregion
            private readonly IAcDomain _acDomain;

            public Guid Id
            {
                get { return _id; }
            }

            #region Ctor
            internal ElementSet(IAcDomain acDomain)
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
                new ElementMessageHandler(this).Register();
            }
            #endregion

            /// <summary>
            /// 
            /// </summary>
            /// <param name="elementId"></param>
            /// <param name="element"></param>
            /// <returns></returns>
            public bool TryGetElement(Guid elementId, out ElementDescriptor element)
            {
                if (!_initialized)
                {
                    Init();
                }
                return _elementDicById.TryGetValue(elementId, out element);
            }

            /// <summary>
            /// 根据ID获取本体元素，包括启用和禁用的本体元素
            /// </summary>
            /// <param name="elementId"></param>
            /// <returns></returns>
            public ElementDescriptor GetElement(Guid elementId)
            {
                if (!_initialized)
                {
                    Init();
                }
                return !_elementDicById.ContainsKey(elementId) ? null : _elementDicById[elementId];
            }

            /// <summary>
            /// 根据模型码索引字段，不区分大小写
            /// </summary>
            /// <param name="ontology">本体</param>
            /// <returns>不会返回null，返回无元素的空字典</returns>
            public Dictionary<string, ElementDescriptor> GetElements(OntologyDescriptor ontology)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_elementDicByOntology.ContainsKey(ontology))
                {
                    return new Dictionary<string, ElementDescriptor>(StringComparer.OrdinalIgnoreCase);
                }

                return _elementDicByOntology[ontology];
            }

            internal void Refresh()
            {
                if (_initialized)
                {
                    _initialized = false;
                }
            }

            #region Init
            private void Init()
            {
                if (_initialized) return;
                lock (ElementSetLocker)
                {
                    if (_initialized) return;
                    _elementDicByOntology.Clear();
                    _elementDicById.Clear();
                    var allElements = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetElements();
                    foreach (var element in allElements)
                    {
                        if (_elementDicById.ContainsKey(element.Id)) continue;
                        var descriptor = new ElementDescriptor(_acDomain, ElementState.Create(_acDomain, element), element.Id);
                        _elementDicById.Add(element.Id, descriptor);
                        OntologyDescriptor ontology;
                        _acDomain.NodeHost.Ontologies.TryGetOntology(element.OntologyId, out ontology);
                        if (!_elementDicByOntology.ContainsKey(ontology))
                        {
                            _elementDicByOntology.Add(ontology, new Dictionary<string, ElementDescriptor>(StringComparer.OrdinalIgnoreCase));
                        }
                        if (!_elementDicByOntology[ontology].ContainsKey(element.Code))
                        {
                            _elementDicByOntology[ontology].Add(element.Code, descriptor);
                        }
                    }
                    _initialized = true;
                }
            }

            #endregion

            #region ElementMessageHandler
            private class ElementMessageHandler :
                IHandler<AddElementCommand>,
                IHandler<AddSystemElementCommand>,
                IHandler<UpdateElementCommand>,
                IHandler<RemoveElementCommand>
            {
                private readonly ElementSet _set;

                public ElementMessageHandler(ElementSet set)
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
                    messageDispatcher.Register((IHandler<AddElementCommand>)this);
                    messageDispatcher.Register((IHandler<AddSystemElementCommand>)this);
                    messageDispatcher.Register((IHandler<UpdateElementCommand>)this);
                    messageDispatcher.Register((IHandler<RemoveElementCommand>)this);
                }

                public void Handle(AddElementCommand message)
                {
                    var acDomain = _set._acDomain;
                    var elementRepository = acDomain.RetrieveRequiredService<IRepository<Element>>();
                    if (string.IsNullOrEmpty(message.Input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    if (!message.Input.Id.HasValue)
                    {
                        throw new ValidationException("标识是必须的");
                    }
                    ElementDescriptor element;
                    if (acDomain.NodeHost.Ontologies.TryGetElement(message.Input.Id.Value, out element))
                    {
                        throw new ValidationException("给定的标识标识的记录已经存在");
                    }
                    OntologyDescriptor ontology;
                    if (!acDomain.NodeHost.Ontologies.TryGetOntology(message.Input.OntologyId, out ontology))
                    {
                        throw new ValidationException("意外的本体标识" + message.Input.OntologyId);
                    }
                    if (ontology.Elements.ContainsKey(message.Input.Code))
                    {
                        throw new ValidationException("重复的编码");
                    }

                    var entity = Element.Create(message.Input);

                    lock (ElementSetLocker)
                    {
                        var descriptor = new ElementDescriptor(acDomain, ElementState.Create(acDomain, entity), entity.Id);
                        _set._elementDicById.Add(entity.Id, descriptor);
                        if (!_set._elementDicByOntology.ContainsKey(ontology))
                        {
                            _set._elementDicByOntology.Add(ontology, new Dictionary<string, ElementDescriptor>(StringComparer.OrdinalIgnoreCase));
                        }
                        _set._elementDicByOntology[ontology].Add(entity.Code, descriptor);
                        try
                        {
                            elementRepository.Add(entity);
                            elementRepository.Context.Commit();
                        }
                        catch
                        {
                            _set._elementDicById.Remove(entity.Id);
                            if (_set._elementDicByOntology.ContainsKey(ontology) && _set._elementDicByOntology[ontology].ContainsKey(entity.Code))
                            {
                                _set._elementDicByOntology[ontology].Remove(entity.Code);
                            }
                            elementRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.PublishEvent(new ElementAddedEvent(message.AcSession, entity));
                    _set._acDomain.CommitEventBus();
                }

                public void Handle(AddSystemElementCommand message)
                {
                    // 配置运行时本体元素
                    var elementEntities = new List<ElementCreateInput>();
                    foreach (var ontology in _set._acDomain.NodeHost.Ontologies)
                    {
                        foreach (var item in ElementDescriptor.SystemElementCodes)
                        {
                            if (ontology.Elements.ContainsKey(item.Key)) continue;
                            var entity = CreateElement(ontology, item.Value);
                            elementEntities.Add(entity);
                        }
                    }
                    if (elementEntities.Count > 0)
                    {
                        foreach (var entity in elementEntities)
                        {
                            _set._acDomain.Handle(new AddElementCommand(message.AcSession, entity));
                        }
                    }
                }

                private ElementCreateInput CreateElement(OntologyDescriptor ontology, DbField field)
                {
                    var element = new ElementCreateInput()
                    {
                        Id = Guid.NewGuid(),
                        AllowFilter = true,
                        AllowSort = true,
                        Code = field.Name,
                        Description = "系统本体元素",
                        FieldCode = field.Name,
                        GroupId = null,
                        Icon = null,
                        InfoDicId = null,
                        InputHeight = null,
                        InputType = null,
                        InputWidth = null,
                        IsDetailsShow = false,
                        IsEnabled = 1,
                        IsExport = false,
                        IsGridColumn = false,
                        IsImport = false,
                        IsInfoIdItem = false,
                        IsInput = false,
                        IsTotalLine = false,
                        MaxLength = field.MaxLength,
                        Name = field.Name,
                        OntologyId = ontology.Ontology.Id,
                        Ref = null,
                        Regex = null,
                        OType = string.Empty,
                        DbType = string.Empty,
                        Nullable = true,
                        SortCode = 10000,
                        Width = 100
                    };

                    return element;
                }

                #region ElementCreateInput
                private class ElementCreateInput : EntityCreateInput, IElementCreateIo
                {
                    public string OType { get; set; }
                    public bool Nullable { get; set; }

                    public bool AllowFilter { get; set; }

                    public bool AllowSort { get; set; }

                    public topicCode Code { get; set; }

                    public topicCode DbType { get; set; }

                    public topicCode Description { get; set; }

                    public topicCode FieldCode { get; set; }

                    public Guid? GroupId { get; set; }

                    public topicCode Icon { get; set; }

                    public Guid? InfoDicId { get; set; }

                    public int? InputHeight { get; set; }

                    public topicCode InputType { get; set; }

                    public int? InputWidth { get; set; }

                    public bool IsDetailsShow { get; set; }

                    public int IsEnabled { get; set; }

                    public bool IsExport { get; set; }

                    public bool IsGridColumn { get; set; }

                    public bool IsImport { get; set; }

                    public bool IsInfoIdItem { get; set; }

                    public bool IsInput { get; set; }

                    public bool IsTotalLine { get; set; }

                    public int? MaxLength { get; set; }

                    public topicCode Name { get; set; }

                    public Guid OntologyId { get; set; }

                    public topicCode Ref { get; set; }

                    public topicCode Regex { get; set; }

                    public int SortCode { get; set; }

                    public int Width { get; set; }

                    public Guid? ForeignElementId { get; set; }

                    public string Tooltip { get; set; }

                    public override IAnycmdCommand ToCommand(IAcSession acSession)
                    {
                        return new AddElementCommand(acSession, this);
                    }
                }
                #endregion

                public void Handle(UpdateElementCommand message)
                {
                    var acDomain = _set._acDomain;
                    var elementRepository = acDomain.RetrieveRequiredService<IRepository<Element>>();
                    if (string.IsNullOrEmpty(message.Input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    ElementDescriptor element;
                    if (!acDomain.NodeHost.Ontologies.TryGetElement(message.Input.Id, out element))
                    {
                        throw new NotExistException();
                    }
                    if (element.Ontology.Elements.ContainsKey(message.Input.Code) && element.Ontology.Elements[message.Input.Code].Element.Id != message.Input.Id)
                    {
                        throw new ValidationException("重复的编码");
                    }
                    var entity = elementRepository.GetByKey(message.Input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }
                    var bkState = _set._elementDicById[entity.Id];

                    entity.Update(message.Input);

                    var newState = new ElementDescriptor(acDomain, ElementState.Create(acDomain, entity), entity.Id);
                    bool stateChanged = newState != bkState;
                    lock (ElementSetLocker)
                    {
                        try
                        {
                            if (stateChanged)
                            {
                                Update(newState);
                            }
                            elementRepository.Update(entity);
                            elementRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            elementRepository.Context.Rollback();
                            throw;
                        }
                        if (stateChanged)
                        {
                            _set._acDomain.PublishEvent(new ElementUpdatedEvent(message.AcSession, entity));
                            _set._acDomain.CommitEventBus();
                        }
                    }
                }

                private void Update(ElementDescriptor state)
                {
                    var oldKey = _set._elementDicById[state.Element.Id].Element.Code;
                    _set._elementDicById[state.Element.Id] = state;
                    if (!_set._elementDicByOntology[state.Ontology].ContainsKey(state.Element.Code))
                    {
                        _set._elementDicByOntology[state.Ontology].Add(state.Element.Code, state);
                        _set._elementDicByOntology[state.Ontology].Remove(oldKey);
                    }
                    else
                    {
                        _set._elementDicByOntology[state.Ontology][state.Element.Code] = state;
                    }
                }

                public void Handle(RemoveElementCommand message)
                {
                    var acDomain = _set._acDomain;
                    var elementRepository = acDomain.RetrieveRequiredService<IRepository<Element>>();
                    ElementDescriptor element;
                    if (!acDomain.NodeHost.Ontologies.TryGetElement(message.EntityId, out element))
                    {
                        return;
                    }
                    Element entity = elementRepository.GetByKey(message.EntityId);
                    if (entity == null)
                    {
                        return;
                    }
                    var bkState = _set._elementDicById[entity.Id];
                    lock (ElementSetLocker)
                    {
                        try
                        {
                            _set._elementDicById.Remove(entity.Id);
                            _set._elementDicByOntology[bkState.Ontology].Remove(bkState.Element.Code);
                            elementRepository.Remove(entity);
                            elementRepository.Context.Commit();
                        }
                        catch
                        {
                            _set._elementDicById.Add(entity.Id, bkState);
                            _set._elementDicByOntology[bkState.Ontology].Add(bkState.Element.Code, bkState);
                            elementRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.PublishEvent(new ElementRemovedEvent(message.AcSession, entity));
                    _set._acDomain.CommitEventBus();
                }
            }
            #endregion
        }
        #endregion

        // 内部类
        #region ActionSet
        private sealed class ActionSet
        {
            private static readonly object ActionSetLocker = new object();
            private readonly Dictionary<OntologyDescriptor, Dictionary<Verb, ActionState>> _actionDicByVerb = new Dictionary<OntologyDescriptor, Dictionary<Verb, ActionState>>();
            private readonly Dictionary<Guid, ActionState> _actionsById = new Dictionary<catalogId, ActionState>();
            private bool _initialized = false;
            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _acDomain;

            public Guid Id
            {
                get { return _id; }
            }

            internal ActionSet(IAcDomain acDomain)
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
                new ActionMessageHandler(this).Register();
            }

            #region GetActons
            public IReadOnlyDictionary<Verb, ActionState> GetActons(OntologyDescriptor ontology)
            {
                if (ontology == null)
                {
                    throw new ArgumentNullException("ontology");
                }
                if (!_initialized)
                {
                    Init();
                }
                return _actionDicByVerb[ontology];
            }
            #endregion

            #region GetAction
            public ActionState GetAction(Guid actionId)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_actionsById.ContainsKey(actionId))
                {
                    return null;
                }
                return _actionsById[actionId];
            }
            #endregion

            public void Refresh()
            {
                if (_initialized)
                {
                    _initialized = false;
                }
            }

            #region Init
            /// <summary>
            /// 初始化信息分组上下文
            /// </summary>
            private void Init()
            {
                if (_initialized) return;
                lock (ActionSetLocker)
                {
                    if (_initialized) return;
                    _actionDicByVerb.Clear();
                    _actionsById.Clear();
                    var actions = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetActions();
                    foreach (var ontology in _acDomain.NodeHost.Ontologies)
                    {
                        if (!_actionDicByVerb.ContainsKey(ontology))
                        {
                            _actionDicByVerb.Add(ontology, new Dictionary<Verb, ActionState>());
                        }
                        foreach (var item in actions.Where(a => a.OntologyId == ontology.Ontology.Id))
                        {
                            var actionState = ActionState.Create(item);
                            if (_actionDicByVerb[ontology].ContainsKey(actionState.ActionVerb))
                            {
                                throw new AnycmdException("意外重复的本体动作动词" + item.Verb);
                            }
                            _actionsById.Add(item.Id, actionState);
                            _actionDicByVerb[ontology].Add(actionState.ActionVerb, actionState);
                        }
                    }
                    _initialized = true;
                }
            }

            #endregion

            #region ActionMessageHandler
            private class ActionMessageHandler :
                IHandler<AddActionCommand>,
                IHandler<UpdateActionCommand>,
                IHandler<RemoveActionCommand>
            {
                private readonly ActionSet _set;

                public ActionMessageHandler(ActionSet set)
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
                    messageDispatcher.Register((IHandler<AddActionCommand>)this);
                    messageDispatcher.Register((IHandler<UpdateActionCommand>)this);
                    messageDispatcher.Register((IHandler<RemoveActionCommand>)this);
                }

                public void Handle(AddActionCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    if (string.IsNullOrEmpty(message.Input.HecpVerb))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    if (!message.Input.Id.HasValue)
                    {
                        throw new ValidationException("标识是必须的");
                    }
                    if (_set._actionsById.ContainsKey(message.Input.Id.Value))
                    {
                        throw new ValidationException("给定的标识标识的记录已经存在");
                    }
                    OntologyDescriptor ontology;
                    if (!_set._acDomain.NodeHost.Ontologies.TryGetOntology(message.Input.OntologyId, out ontology))
                    {
                        throw new ValidationException("非法的本体标识");
                    }
                    if (ontology.Actions.ContainsKey(new Verb(message.Input.HecpVerb)))
                    {
                        throw new ValidationException("重复的动词");
                    }

                    var entity = Action.Create(message.Input);

                    lock (ActionSetLocker)
                    {
                        try
                        {
                            var state = ActionState.Create(entity);
                            _set._actionsById.Add(entity.Id, state);
                            _set._actionDicByVerb[ontology].Add(new Verb(entity.Verb), state);
                            ontologyRepository.Context.RegisterNew(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            _set._actionsById.Remove(entity.Id);
                            _set._actionDicByVerb[ontology].Remove(new Verb(entity.Verb));
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    acDomain.PublishEvent(new ActionAddedEvent(message.AcSession, entity));
                    acDomain.CommitEventBus();
                }

                public void Handle(UpdateActionCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    if (string.IsNullOrEmpty(message.Input.HecpVerb))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    bool exist = false;
                    OntologyDescriptor ontology = null;
                    foreach (var item in acDomain.NodeHost.Ontologies)
                    {
                        if (item.Actions.Values.Any(a => a.Id == message.Input.Id))
                        {
                            exist = true;
                            ontology = item;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        throw new NotExistException();
                    }
                    var verb = new Verb(message.Input.HecpVerb);
                    if (ontology.Actions.ContainsKey(verb) && ontology.Actions[verb].Id != message.Input.Id)
                    {
                        throw new ValidationException("重复的编码");
                    }
                    var entity = ontologyRepository.Context.Query<Action>().FirstOrDefault(a => a.Id == message.Input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }
                    var bkState = ActionState.Create(entity);

                    entity.Update(message.Input);

                    var newState = ActionState.Create(entity);
                    bool stateChanged = newState != bkState;
                    lock (ActionSetLocker)
                    {
                        try
                        {
                            if (stateChanged)
                            {
                                Update(newState);
                            }
                            ontologyRepository.Context.RegisterModified(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    if (stateChanged)
                    {
                        _set._acDomain.PublishEvent(new ActionUpdatedEvent(message.AcSession, entity));
                        _set._acDomain.CommitEventBus();
                    }
                }

                private void Update(ActionState state)
                {
                    var ontology = _set._acDomain.NodeHost.Ontologies[state.OntologyId];
                    var newVerb = state.ActionVerb;
                    var oldVerb = _set._actionsById[state.Id].ActionVerb;
                    _set._actionsById[state.Id] = state;
                    if (!_set._actionDicByVerb[ontology].ContainsKey(newVerb))
                    {
                        _set._actionDicByVerb[ontology].Add(newVerb, state);
                        _set._actionDicByVerb[ontology].Remove(oldVerb);
                    }
                    else
                    {
                        _set._actionDicByVerb[ontology][newVerb] = state;
                    }
                }

                public void Handle(RemoveActionCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    bool exist = false;
                    OntologyDescriptor ontology = null;
                    foreach (var item in acDomain.NodeHost.Ontologies)
                    {
                        if (item.Actions.Values.Any(a => a.Id == message.EntityId))
                        {
                            ontology = item;
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        return;
                    }
                    var entity = ontologyRepository.Context.Query<Action>().FirstOrDefault(a => a.Id == message.EntityId);
                    if (entity == null)
                    {
                        return;
                    }
                    var bkState = ActionState.Create(entity);
                    lock (ActionSetLocker)
                    {
                        try
                        {
                            _set._actionsById.Remove(entity.Id);
                            _set._actionDicByVerb[ontology].Remove(bkState.ActionVerb);
                            ontologyRepository.Context.RegisterDeleted(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            _set._actionsById.Add(entity.Id, bkState);
                            _set._actionDicByVerb[ontology].Add(bkState.ActionVerb, bkState);
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    acDomain.PublishEvent(new ActionRemovedEvent(message.AcSession, entity));
                    acDomain.CommitEventBus();
                }
            }
            #endregion
        }
        #endregion

        // 内部类
        #region InfoGroupSet
        private sealed class InfoGroupSet
        {
            private static readonly object InfoGroupSetLocker = new object();
            private readonly Dictionary<OntologyDescriptor, IList<InfoGroupState>>
                _dic = new Dictionary<OntologyDescriptor, IList<InfoGroupState>>();
            private bool _initialized = false;
            private readonly Guid _id = new Guid();
            private readonly IAcDomain _acDomain;

            public Guid Id
            {
                get { return _id; }
            }

            internal InfoGroupSet(IAcDomain acDomain)
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
                new InfoGroupMessageHandler(this).Register();
            }

            public IList<InfoGroupState> GetInfoGroups(OntologyDescriptor ontology)
            {
                if (!_initialized)
                {
                    Init();
                }
                return !_dic.ContainsKey(ontology) ? new List<InfoGroupState>() : _dic[ontology];
            }

            public void Refresh()
            {
                if (_initialized)
                {
                    _initialized = false;
                }
            }

            #region Init
            /// <summary>
            /// 初始化信息分组上下文
            /// </summary>
            private void Init()
            {
                if (!_initialized)
                {
                    lock (InfoGroupSetLocker)
                    {
                        if (!_initialized)
                        {
                            _dic.Clear();
                            var infoGroups = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetInfoGroups().OrderBy(a => a.SortCode);
                            foreach (var ontology in _acDomain.NodeHost.Ontologies)
                            {
                                _dic.Add(ontology, new List<InfoGroupState>());
                                foreach (var infoGroup in infoGroups.Where(a => a.OntologyId == ontology.Ontology.Id))
                                {
                                    _dic[ontology].Add(InfoGroupState.Create(infoGroup));
                                }
                            }
                            _initialized = true;
                        }
                    }
                }
            }
            #endregion

            #region InfoGroupMessageHandler
            private class InfoGroupMessageHandler :
                IHandler<AddInfoGroupCommand>,
                IHandler<UpdateInfoGroupCommand>,
                IHandler<RemoveInfoGroupCommand>
            {
                private readonly InfoGroupSet _set;

                public InfoGroupMessageHandler(InfoGroupSet set)
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
                    messageDispatcher.Register((IHandler<AddInfoGroupCommand>)this);
                    messageDispatcher.Register((IHandler<UpdateInfoGroupCommand>)this);
                    messageDispatcher.Register((IHandler<RemoveInfoGroupCommand>)this);
                }

                public void Handle(AddInfoGroupCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    if (string.IsNullOrEmpty(message.Input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    if (!message.Input.Id.HasValue)
                    {
                        throw new ValidationException("标识是必须的");
                    }
                    OntologyDescriptor ontology;
                    if (!acDomain.NodeHost.Ontologies.TryGetOntology(message.Input.OntologyId, out ontology))
                    {
                        throw new ValidationException("非法的本体标识" + message.Input.OntologyId);
                    }
                    if (ontology.InfoGroups.Any(a => a.Id == message.Input.Id))
                    {
                        throw new AnycmdException("给定的标识标识的工作组已经存在");
                    }
                    if (ontology.InfoGroups.Any(a => string.Equals(a.Code, message.Input.Code, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ValidationException("重复的编码");
                    }

                    InfoGroup entity = InfoGroup.Create(message.Input);

                    lock (InfoGroupSetLocker)
                    {
                        try
                        {
                            if (!_set._dic.ContainsKey(ontology))
                            {
                                _set._dic.Add(ontology, new List<InfoGroupState>());
                            }
                            if (_set._dic[ontology].All(a => a.Id != entity.Id))
                            {
                                _set._dic[ontology].Add(InfoGroupState.Create(entity));
                            }
                            ontologyRepository.Context.RegisterNew(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            if (_set._dic.ContainsKey(ontology))
                            {
                                var item = _set._dic[ontology].FirstOrDefault(a => a.Id == entity.Id);
                                if (item != null)
                                {
                                    _set._dic[ontology].Remove(item);
                                }
                            }
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.PublishEvent(new InfoGroupAddedEvent(message.AcSession, entity));
                    _set._acDomain.CommitEventBus();
                }

                public void Handle(UpdateInfoGroupCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    if (string.IsNullOrEmpty(message.Input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    InfoGroup entity;
                    bool stateChanged = false;
                    lock (InfoGroupSetLocker)
                    {
                        entity = ontologyRepository.Context.Query<InfoGroup>().FirstOrDefault(a => a.Id == message.Input.Id);
                        if (entity == null)
                        {
                            throw new NotExistException();
                        }
                        OntologyDescriptor ontology;
                        if (!_set._acDomain.NodeHost.Ontologies.TryGetOntology(entity.OntologyId, out ontology))
                        {
                            throw new ValidationException("非法的本体标识" + entity.OntologyId);
                        }
                        if (ontology.InfoGroups.Any(a => string.Equals(a.Code, message.Input.Code, StringComparison.OrdinalIgnoreCase) && a.Id != entity.OntologyId))
                        {
                            throw new ValidationException("重复的编码");
                        }

                        var bkState = InfoGroupState.Create(entity);

                        entity.Update(message.Input);

                        var newState = InfoGroupState.Create(entity);
                        stateChanged = newState != bkState;
                        try
                        {
                            if (stateChanged)
                            {
                                Update(newState);
                            }
                            ontologyRepository.Context.RegisterModified(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    if (stateChanged)
                    {
                        _set._acDomain.PublishEvent(new InfoGroupUpdatedEvent(message.AcSession, entity));
                        _set._acDomain.CommitEventBus();
                    }
                }

                private void Update(InfoGroupState state)
                {
                    OntologyDescriptor ontology = _set._acDomain.NodeHost.Ontologies[state.OntologyId];
                    var item = _set._dic[ontology].First(a => a.Id == state.Id);
                    _set._dic[ontology].Remove(item);
                    _set._dic[ontology].Add(state);
                }

                public void Handle(RemoveInfoGroupCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    InfoGroup entity = ontologyRepository.Context.Query<InfoGroup>().FirstOrDefault(a => a.Id == message.EntityId);
                    if (entity == null)
                    {
                        return;
                    }
                    var bkState = InfoGroupState.Create(entity);
                    lock (InfoGroupSetLocker)
                    {
                        try
                        {
                            _set._dic[acDomain.NodeHost.Ontologies[bkState.OntologyId]].Remove(bkState);
                            ontologyRepository.Context.RegisterDeleted(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            _set._dic[acDomain.NodeHost.Ontologies[bkState.OntologyId]].Add(bkState);
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.PublishEvent(new InfoGroupRemovedEvent(message.AcSession, entity));
                    _set._acDomain.CommitEventBus();
                }
            }
            #endregion
        }
        #endregion

        // 内部类
        #region OntologyCatalogSet
        private sealed class OntologyCatalogSet
        {
            private readonly Dictionary<OntologyDescriptor, Dictionary<CatalogState, OntologyCatalogState>>
                _dic = new Dictionary<OntologyDescriptor, Dictionary<CatalogState, OntologyCatalogState>>();
            private bool _initialized = false;
            private static readonly object OntologyCatalogSetLocker = new object();
            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _acDomain;

            public Guid Id
            {
                get { return _id; }
            }

            internal OntologyCatalogSet(IAcDomain acDomain)
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
                new OntologyCatalogMessageHandler(this).Register();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ontology"></param>
            /// <returns>key为目录码</returns>
            public Dictionary<CatalogState, OntologyCatalogState> this[OntologyDescriptor ontology]
            {
                get
                {
                    if (!_initialized)
                    {
                        Init();
                    }
                    if (!_dic.ContainsKey(ontology))
                    {
                        return new Dictionary<CatalogState, OntologyCatalogState>();
                    }

                    return _dic[ontology];
                }
            }

            private void Init()
            {
                if (_initialized) return;
                lock (OntologyCatalogSetLocker)
                {
                    if (_initialized) return;
                    _dic.Clear();
                    var ontologyOrgs = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetOntologyCatalogs();
                    foreach (var ontologyOrg in ontologyOrgs)
                    {
                        CatalogState org;
                        OntologyDescriptor ontology;
                        if (!_acDomain.NodeHost.Ontologies.TryGetOntology(ontologyOrg.OntologyId, out ontology))
                        {
                            throw new AnycmdException("意外的本体目录本体标识" + ontologyOrg.OntologyId);
                        }
                        if (_acDomain.CatalogSet.TryGetCatalog(ontologyOrg.CatalogId, out org))
                        {
                            if (!_dic.ContainsKey(ontology))
                            {
                                _dic.Add(ontology, new Dictionary<CatalogState, OntologyCatalogState>());
                            }
                            var ontologyOrgState = OntologyCatalogState.Create(_acDomain, ontologyOrg);
                            if (!_dic[ontology].ContainsKey(org))
                            {
                                _dic[ontology].Add(org, ontologyOrgState);
                            }
                        }
                        else
                        {
                            // TODO:移除废弃的目录
                        }
                    }
                    _initialized = true;
                }
            }

            private class OntologyCatalogMessageHandler :
                IHandler<AddOntologyCatalogCommand>,
                IHandler<OntologyCatalogAddedEvent>,
                IHandler<RemoveOntologyCatalogCommand>,
                IHandler<OntologyCatalogRemovedEvent>,
                IHandler<AddCatalogActionCommand>,
                IHandler<CatalogActionAddedEvent>,
                IHandler<UpdateCatalogActionCommand>,
                IHandler<CatalogActionUpdatedEvent>,
                IHandler<RemoveCatalogActionCommand>,
                IHandler<CatalogActionRemovedEvent>
            {
                private readonly OntologyCatalogSet set;

                public OntologyCatalogMessageHandler(OntologyCatalogSet set)
                {
                    this.set = set;
                }

                public void Register()
                {
                    var messageDispatcher = set._acDomain.MessageDispatcher;
                    if (messageDispatcher == null)
                    {
                        throw new ArgumentNullException("AcDomain对象'{0}'尚未设置MessageDispatcher。".Fmt(set._acDomain.Name));
                    }
                    messageDispatcher.Register((IHandler<AddOntologyCatalogCommand>)this);
                    messageDispatcher.Register((IHandler<OntologyCatalogAddedEvent>)this);
                    messageDispatcher.Register((IHandler<RemoveOntologyCatalogCommand>)this);
                    messageDispatcher.Register((IHandler<OntologyCatalogRemovedEvent>)this);
                    messageDispatcher.Register((IHandler<AddCatalogActionCommand>)this);
                    messageDispatcher.Register((IHandler<CatalogActionAddedEvent>)this);
                    messageDispatcher.Register((IHandler<RemoveCatalogActionCommand>)this);
                    messageDispatcher.Register((IHandler<CatalogActionRemovedEvent>)this);
                }

                public void Handle(AddOntologyCatalogCommand message)
                {
                    this.Handle(message.AcSession, message.Input, true);
                }

                public void Handle(OntologyCatalogAddedEvent message)
                {
                    if (message.IsPrivate)
                    {
                        return;
                    }
                    this.Handle(message.AcSession, message.Output, false);
                }

                private void Handle(IAcSession acSession, IOntologyCatalogCreateIo input, bool isCommand)
                {
                    var _dic = set._dic;
                    var acDomain = set._acDomain;
                    var repository = acDomain.RetrieveRequiredService<IRepository<OntologyCatalog>>();
                    OntologyDescriptor ontology;
                    if (!acDomain.NodeHost.Ontologies.TryGetOntology(input.OntologyId, out ontology))
                    {
                        throw new AnycmdException("意外的本体标识" + input.OntologyId);
                    }
                    CatalogState org;
                    if (!acDomain.CatalogSet.TryGetCatalog(input.CatalogId, out org))
                    {
                        throw new AnycmdException("意外的目录标识" + input.CatalogId);
                    }
                    OntologyCatalog entity;
                    lock (OntologyCatalogSetLocker)
                    {
                        if (_dic.ContainsKey(ontology) && _dic[ontology].ContainsKey(org))
                        {
                            return;
                        }
                        Debug.Assert(input.Id != null, "input.Id != null");
                        entity = new OntologyCatalog
                        {
                            Id = input.Id.Value,
                            OntologyId = input.OntologyId,
                            CatalogId = input.CatalogId,
                            Actions = null
                        };
                        var ontologyOrgState = OntologyCatalogState.Create(acDomain, entity);
                        if (!_dic.ContainsKey(ontology))
                        {
                            _dic.Add(ontology, new Dictionary<CatalogState, OntologyCatalogState>());
                        }
                        _dic[ontology].Add(org, ontologyOrgState);
                        if (isCommand)
                        {
                            try
                            {
                                repository.Add(entity);
                                repository.Context.Commit();
                            }
                            catch
                            {
                                _dic[ontology].Remove(org);
                                repository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        acDomain.MessageDispatcher.DispatchMessage(new OntologyCatalogAddedEvent(acSession, entity, input, isPrivate: true));
                    }
                }

                public void Handle(RemoveOntologyCatalogCommand message)
                {
                    this.Handle(message.AcSession, message.OntologyId, message.CatalogId, true);
                }

                public void Handle(OntologyCatalogRemovedEvent message)
                {
                    if (message.IsPrivate)
                    {
                        return;
                    }
                    var entity = message.Source as OntologyCatalogBase;
                    this.Handle(message.AcSession, entity.OntologyId, entity.CatalogId, false);
                }

                private void Handle(IAcSession acSession, Guid ontologyId, Guid catalogId, bool isCommand)
                {
                    var dic = set._dic;
                    var acDomain = set._acDomain;
                    var repository = acDomain.RetrieveRequiredService<IRepository<OntologyCatalog>>();
                    OntologyDescriptor ontology;
                    if (!acDomain.NodeHost.Ontologies.TryGetOntology(ontologyId, out ontology))
                    {
                        throw new ValidationException("意外的本体标识" + ontologyId);
                    }
                    OntologyCatalog entity;
                    lock (OntologyCatalogSetLocker)
                    {
                        CatalogState org = null;
                        OntologyCatalogState bkState = null;
                        foreach (var item in dic)
                        {
                            foreach (var item1 in item.Value)
                            {
                                if (item1.Value.CatalogId == catalogId)
                                {
                                    bkState = item1.Value;
                                    org = item1.Key;
                                    break;
                                }
                            }
                            if (bkState != null)
                            {
                                break;
                            }
                        }
                        if (bkState == null)
                        {
                            return;
                        }
                        entity = repository.GetByKey(bkState.Id);
                        if (entity == null)
                        {
                            return;
                        }
                        dic[ontology].Remove(org);
                        if (isCommand)
                        {
                            try
                            {
                                repository.Remove(entity);
                                repository.Context.Commit();
                            }
                            catch
                            {
                                dic[ontology].Add(org, bkState);
                                repository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        acDomain.MessageDispatcher.DispatchMessage(new OntologyCatalogRemovedEvent(acSession, entity, isPrivate: true));
                    }
                }

                public void Handle(AddCatalogActionCommand message)
                {
                    // TODO:处理AddCatalogActionCommand命令
                }

                public void Handle(CatalogActionAddedEvent message)
                {
                    // TODO:处理CatalogActionAddedEvent事件
                }

                public void Handle(UpdateCatalogActionCommand message)
                {
                    // TODO:处理UpdateCatalogActionCommand命令
                }

                public void Handle(CatalogActionUpdatedEvent message)
                {
                    // TODO:处理CatalogActionUpdatedEvent事件
                }

                public void Handle(RemoveCatalogActionCommand message)
                {
                    // TODO:处理RemoveCatalogActionCommand命令
                }

                public void Handle(CatalogActionRemovedEvent message)
                {
                    // TODO:处理CatalogActionRemovedEvent事件
                }
            }
        }
        #endregion

        // 内部类
        #region TopicSet
        private sealed class TopicSet
        {
            private readonly Dictionary<OntologyDescriptor, Dictionary<topicCode, TopicState>>
                _dic = new Dictionary<OntologyDescriptor, Dictionary<topicCode, TopicState>>();

            private bool _initialized = false;
            private static readonly object TopicSetLocker = new object();
            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _acDomain;

            public Guid Id
            {
                get { return _id; }
            }

            internal TopicSet(IAcDomain acDomain)
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
                new TopicMessageHandler(this).Register();
            }

            public Dictionary<string, TopicState> this[OntologyDescriptor ontology]
            {
                get
                {
                    if (!_initialized)
                    {
                        Init();
                    }
                    if (!_dic.ContainsKey(ontology))
                    {
                        return new Dictionary<string, TopicState>(StringComparer.OrdinalIgnoreCase);
                    }

                    return _dic[ontology];
                }
            }

            public void Refresh()
            {
                if (_initialized)
                {
                    _initialized = false;
                }
            }

            #region Init
            /// <summary>
            /// 初始化信息分组上下文
            /// </summary>
            private void Init()
            {
                if (!_initialized)
                {
                    lock (TopicSetLocker)
                    {
                        if (!_initialized)
                        {
                            _dic.Clear();
                            var list = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetTopics();
                            foreach (var item in list)
                            {
                                var ontology = _acDomain.NodeHost.Ontologies[item.OntologyId];
                                if (!_dic.ContainsKey(ontology))
                                {
                                    _dic.Add(ontology, new Dictionary<topicCode, TopicState>(StringComparer.OrdinalIgnoreCase));
                                }
                                var state = TopicState.Create(_acDomain, item);
                                _dic[ontology].Add(item.Code, state);
                            }
                            _initialized = true;
                        }
                    }
                }
            }
            #endregion

            #region TopicMessageHandler
            private class TopicMessageHandler :
                IHandler<AddTopicCommand>,
                IHandler<UpdateTopicCommand>,
                IHandler<RemoveTopicCommand>
            {
                private readonly TopicSet _set;

                public TopicMessageHandler(TopicSet set)
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
                    messageDispatcher.Register((IHandler<AddTopicCommand>)this);
                    messageDispatcher.Register((IHandler<UpdateTopicCommand>)this);
                    messageDispatcher.Register((IHandler<RemoveTopicCommand>)this);
                }

                public void Handle(AddTopicCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    if (string.IsNullOrEmpty(message.Input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    if (!message.Input.Id.HasValue)
                    {
                        throw new ValidationException("标识是必须的");
                    }
                    OntologyDescriptor ontology;
                    if (!acDomain.NodeHost.Ontologies.TryGetOntology(message.Input.OntologyId, out ontology))
                    {
                        throw new ValidationException("非法的本体标识" + message.Input.OntologyId);
                    }
                    if (ontology.Topics.ContainsKey(message.Input.Code))
                    {
                        throw new ValidationException("重复的编码");
                    }
                    if (ontology.Topics.Any(a => a.Value.Id == message.Input.Id.Value))
                    {
                        throw new ValidationException("给定标识的记录已经存在");
                    }
                    Topic entity = Topic.Create(message.Input);
                    lock (TopicSetLocker)
                    {
                        try
                        {
                            _set._dic[ontology].Add(entity.Code, TopicState.Create(acDomain, entity));
                            ontologyRepository.Context.RegisterNew(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            _set._dic[ontology].Remove(entity.Code);
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.PublishEvent(new TopicAddedEvent(message.AcSession, entity));
                    _set._acDomain.CommitEventBus();
                }

                public void Handle(UpdateTopicCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    if (string.IsNullOrEmpty(message.Input.Code))
                    {
                        throw new ValidationException("编码不能为空");
                    }
                    TopicState topic = null;
                    OntologyDescriptor ontology = null;
                    foreach (var item in _set._dic)
                    {
                        foreach (var t in item.Value.Values)
                        {
                            if (t.Id == message.Input.Id)
                            {
                                topic = t;
                                ontology = item.Key;
                                break;
                            }
                        }
                    }
                    if (topic == null)
                    {
                        throw new NotExistException();
                    }
                    if (ontology.Topics.ContainsKey(message.Input.Code) && message.Input.Id != ontology.Topics[message.Input.Code].Id)
                    {
                        throw new ValidationException("重复的编码");
                    }
                    var entity = ontologyRepository.Context.Query<Topic>().FirstOrDefault(a => a.Id == message.Input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }
                    var bkState = TopicState.Create(acDomain, entity);
                    entity.Update(message.Input);
                    var newState = TopicState.Create(acDomain, entity);
                    bool stateChanged = newState != bkState;
                    lock (TopicSetLocker)
                    {
                        try
                        {
                            if (stateChanged)
                            {
                                if (!_set._dic[ontology].ContainsKey(newState.Code))
                                {
                                    _set._dic[ontology].Add(newState.Code, newState);
                                    _set._dic[ontology].Remove(bkState.Code);
                                }
                                else
                                {
                                    _set._dic[ontology][newState.Code] = newState;
                                }
                            }
                            ontologyRepository.Context.RegisterModified(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                if (!_set._dic[ontology].ContainsKey(bkState.Code))
                                {
                                    _set._dic[ontology].Add(bkState.Code, bkState);
                                    _set._dic[ontology].Remove(newState.Code);
                                }
                                else
                                {
                                    _set._dic[ontology][bkState.Code] = bkState;
                                }
                            }
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.PublishEvent(new TopicUpdatedEvent(message.AcSession, entity));
                    _set._acDomain.CommitEventBus();
                }

                public void Handle(RemoveTopicCommand message)
                {
                    var acDomain = _set._acDomain;
                    var ontologyRepository = acDomain.RetrieveRequiredService<IRepository<Ontology>>();
                    TopicState topic = null;
                    OntologyDescriptor ontology = null;
                    foreach (var item in _set._dic)
                    {
                        foreach (var t in item.Value.Values)
                        {
                            if (t.Id == message.EntityId)
                            {
                                topic = t;
                                ontology = item.Key;
                                break;
                            }
                        }
                    }
                    if (topic == null)
                    {
                        return;
                    }
                    var entity = ontologyRepository.Context.Query<Topic>().FirstOrDefault(a => a.Id == message.EntityId);
                    if (entity == null)
                    {
                        return;
                    }
                    var bkState = TopicState.Create(acDomain, entity);
                    lock (TopicSetLocker)
                    {
                        try
                        {
                            _set._dic[ontology].Remove(bkState.Code);
                            ontologyRepository.Context.RegisterDeleted(entity);
                            ontologyRepository.Context.Commit();
                        }
                        catch
                        {
                            if (!_set._dic[ontology].ContainsKey(bkState.Code))
                            {
                                _set._dic[ontology].Add(bkState.Code, bkState);
                            }
                            ontologyRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.PublishEvent(new TopicRemovedEvent(message.AcSession, entity));
                    _set._acDomain.CommitEventBus();
                }
            }
            #endregion
        }
        #endregion

        // 内部类
        #region ArchiveSet
        private sealed class ArchiveSet
        {
            private readonly Dictionary<Guid, ArchiveState> _dicById = new Dictionary<Guid, ArchiveState>();
            private readonly Dictionary<OntologyDescriptor, List<ArchiveState>> _dicByOntology = new Dictionary<OntologyDescriptor, List<ArchiveState>>();
            private bool _initialized = false;
            private static readonly object ArchiveSetLocker = new object();
            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _acDomain;

            public Guid Id
            {
                get { return _id; }
            }

            /// <summary>
            /// 构造并接入总线
            /// </summary>
            internal ArchiveSet(IAcDomain acDomain)
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
                new ArchiveMessageHandler(this).Register();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="archiveId"></param>
            /// <param name="archive"></param>
            /// <returns></returns>
            public bool TryGetArchive(Guid archiveId, out ArchiveState archive)
            {
                if (!_initialized)
                {
                    Init();
                }
                return _dicById.TryGetValue(archiveId, out archive);
            }

            public IReadOnlyCollection<ArchiveState> GetArchives(OntologyDescriptor ontology)
            {
                if (!_initialized)
                {
                    Init();
                }
                if (!_dicByOntology.ContainsKey(ontology))
                {
                    return new List<ArchiveState>();
                }
                return _dicByOntology[ontology];
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

            private void Init()
            {
                if (_initialized) return;
                lock (ArchiveSetLocker)
                {
                    if (_initialized) return;
                    _dicById.Clear();
                    _dicByOntology.Clear();
                    var list = _acDomain.RetrieveRequiredService<INodeHostBootstrap>().GetArchives();
                    foreach (var entity in list)
                    {
                        var archive = ArchiveState.Create(_acDomain, entity);
                        _dicById.Add(entity.Id, archive);
                        if (!_dicByOntology.ContainsKey(archive.Ontology))
                        {
                            _dicByOntology.Add(archive.Ontology, new List<ArchiveState>());
                        }
                        _dicByOntology[archive.Ontology].Add(archive);
                    }
                    _initialized = true;
                }
            }

            #region ArchiveMessageHandler
            private class ArchiveMessageHandler :
                IHandler<AddArchiveCommand>,
                IHandler<UpdateArchiveCommand>,
                IHandler<RemoveArchiveCommand>
            {
                private readonly ArchiveSet _set;

                public ArchiveMessageHandler(ArchiveSet set)
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
                    messageDispatcher.Register((IHandler<AddArchiveCommand>)this);
                    messageDispatcher.Register((IHandler<UpdateArchiveCommand>)this);
                    messageDispatcher.Register((IHandler<RemoveArchiveCommand>)this);
                }

                public void Handle(AddArchiveCommand message)
                {
                    var acDomain = _set._acDomain;
                    var archiveRepository = acDomain.RetrieveRequiredService<IRepository<Archive>>();
                    if (!message.Input.Id.HasValue)
                    {
                        throw new ValidationException("标识是必须的");
                    }
                    ArchiveState archive;
                    if (acDomain.NodeHost.Ontologies.TryGetArchive(message.Input.Id.Value, out archive))
                    {
                        throw new ValidationException("给定标识的归档记录已经存在");
                    }
                    OntologyDescriptor ontology;
                    if (!acDomain.NodeHost.Ontologies.TryGetOntology(message.Input.OntologyId, out ontology))
                    {
                        throw new ValidationException("意外的本体标识" + message.Input.OntologyId);
                    }
                    int numberId = archiveRepository.AsQueryable().Where(a => a.OntologyId == message.Input.OntologyId).OrderByDescending(a => a.NumberId).Select(a => a.NumberId).FirstOrDefault() + 1;

                    var entity = Archive.Create(message.Input);

                    if (string.IsNullOrEmpty(entity.RdbmsType))
                    {
                        entity.RdbmsType = "anycmd.RDatabase.rdbms.sqlserver";// 默认为SqlServer数据库
                    }
                    lock (ArchiveSetLocker)
                    {
                        try
                        {
                            var state = ArchiveState.Create(acDomain, entity);
                            state.Archive(numberId);
                            entity.ArchiveOn = state.ArchiveOn;
                            entity.NumberId = state.NumberId;
                            entity.FilePath = state.FilePath;
                            if (!_set._dicById.ContainsKey(entity.Id))
                            {
                                _set._dicById.Add(entity.Id, state);
                            }
                            if (!_set._dicByOntology.ContainsKey(ontology))
                            {
                                _set._dicByOntology.Add(ontology, new List<ArchiveState>());
                            }
                            if (!_set._dicByOntology[ontology].Contains(state))
                            {
                                _set._dicByOntology[ontology].Add(state);
                            }
                            archiveRepository.Add(entity);
                            archiveRepository.Context.Commit();
                        }
                        catch
                        {
                            if (_set._dicById.ContainsKey(entity.Id))
                            {
                                _set._dicById.Remove(entity.Id);
                            }
                            if (_set._dicByOntology.ContainsKey(ontology) && _set._dicByOntology[ontology].Any(a => a.Id == entity.Id))
                            {
                                var item = _set._dicByOntology[ontology].First(a => a.Id == entity.Id);
                                _set._dicByOntology[ontology].Remove(item);
                            }
                            archiveRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.PublishEvent(new ArchivedEvent(message.AcSession, entity));
                    _set._acDomain.CommitEventBus();
                }

                public void Handle(UpdateArchiveCommand message)
                {
                    var acDomain = _set._acDomain;
                    var archiveRepository = acDomain.RetrieveRequiredService<IRepository<Archive>>();
                    ArchiveState archive;
                    if (!acDomain.NodeHost.Ontologies.TryGetArchive(message.Input.Id, out archive))
                    {
                        throw new NotExistException();
                    }
                    var entity = archiveRepository.GetByKey(message.Input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }
                    var bkState = ArchiveState.Create(acDomain, entity);

                    entity.Update(message.Input);

                    var newState = ArchiveState.Create(acDomain, entity);
                    bool stateChanged = newState != bkState;
                    lock (ArchiveSetLocker)
                    {
                        try
                        {
                            if (stateChanged)
                            {
                                Update(newState);
                            }
                            archiveRepository.Update(entity);
                            archiveRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            archiveRepository.Context.Rollback();
                            throw;
                        }
                    }
                    if (stateChanged)
                    {
                        _set._acDomain.PublishEvent(new ArchiveUpdatedEvent(message.AcSession, entity));
                        _set._acDomain.CommitEventBus();
                    }
                }

                private void Update(ArchiveState state)
                {
                    OntologyDescriptor ontology;
                    if (!_set._acDomain.NodeHost.Ontologies.TryGetOntology(state.OntologyId, out ontology))
                    {
                        throw new AnycmdException("意外的归档本体标识" + state.OntologyId);
                    }
                    _set._dicById[state.Id] = state;
                    if (_set._dicByOntology.ContainsKey(ontology) && _set._dicByOntology[ontology].Any(a => a.Id == state.Id))
                    {
                        var item = _set._dicByOntology[ontology].First(a => a.Id == state.Id);
                        _set._dicByOntology[ontology].Remove(item);
                        _set._dicByOntology[ontology].Add(state);
                    }
                }

                public void Handle(RemoveArchiveCommand message)
                {
                    var acDomain = _set._acDomain;
                    var archiveRepository = acDomain.RetrieveRequiredService<IRepository<Archive>>();
                    ArchiveState archive;
                    if (!acDomain.NodeHost.Ontologies.TryGetArchive(message.EntityId, out archive))
                    {
                        return;
                    }
                    Archive entity = archiveRepository.GetByKey(message.EntityId);
                    if (entity == null)
                    {
                        return;
                    }
                    var bkState = ArchiveState.Create(acDomain, entity);
                    lock (ArchiveSetLocker)
                    {
                        try
                        {
                            _set._dicById.Remove(entity.Id);
                            if (_set._dicByOntology.ContainsKey(archive.Ontology))
                            {
                                var item = _set._dicByOntology[archive.Ontology].FirstOrDefault(a => a.Id == archive.Id);
                                if (item != null)
                                {
                                    _set._dicByOntology[archive.Ontology].Remove(item);
                                }
                            }
                            archive.Ontology.EntityProvider.DropArchive(archive);
                            archiveRepository.Remove(entity);
                            archiveRepository.Context.Commit();
                        }
                        catch
                        {
                            _set._dicById.Add(entity.Id, bkState);
                            if (!_set._dicByOntology.ContainsKey(archive.Ontology))
                            {
                                _set._dicByOntology.Add(archive.Ontology, new List<ArchiveState>());
                            }
                            var item = _set._dicByOntology[archive.Ontology].FirstOrDefault(a => a.Id == archive.Id);
                            if (item == null)
                            {
                                _set._dicByOntology[archive.Ontology].Add(bkState);
                            }
                            archiveRepository.Context.Rollback();
                            throw;
                        }
                    }
                    _set._acDomain.CommandBus.Publish(new ArchiveDeletedEvent(message.AcSession, entity));
                    _set._acDomain.CommandBus.Commit();
                }
            }
            #endregion
        }
        #endregion
    }
}