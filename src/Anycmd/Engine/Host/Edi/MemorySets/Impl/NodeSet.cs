
namespace Anycmd.Engine.Host.Edi.MemorySets.Impl
{
    using Bus;
    using Engine.Ac;
    using Engine.Edi;
    using Engine.Edi.Abstractions;
    using Entities;
    using Exceptions;
    using Extensions;
    using Hecp;
    using InOuts;
    using Messages;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using elementId = System.Guid;
    using isCare = System.Boolean;
    using ontologyId = System.Guid;

    /// <summary>
    /// 节点上下文访问接口默认实现
    /// </summary>
    public sealed class NodeSet : INodeSet
    {
        public static readonly INodeSet Empty = new NodeSet(EmptyAcDomain.SingleInstance);

        private readonly Dictionary<string, NodeDescriptor>
            _allNodesById = new Dictionary<string, NodeDescriptor>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, NodeDescriptor>
            _allNodesByPublicKey = new Dictionary<string, NodeDescriptor>(StringComparer.OrdinalIgnoreCase);
        private NodeDescriptor _selfNode = null;
        private NodeDescriptor _centerNode = null;
        private bool _initialized = false;
        private readonly object _locker = new object();

        private readonly Guid _id = Guid.NewGuid();
        private readonly NodeCareSet _nodeCareSet;
        private readonly NodeElementActionSet _actionSet;
        private readonly OrganizationSet _organizationSet;

        private readonly IAcDomain _host;

        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// 构造并接入总线
        /// </summary>
        public NodeSet(IAcDomain host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            this._nodeCareSet = new NodeCareSet(host);
            this._actionSet = new NodeElementActionSet(host);
            this._organizationSet = new OrganizationSet(host);
            new MessageHandler(this).Register();
        }

        public NodeDescriptor CenterNode
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (_centerNode == null)
                {
                    throw new CoreException("尚没有设定中心节点，请先设定中心节点");
                }

                return _centerNode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NodeDescriptor ThisNode
        {
            get
            {
                if (!_initialized)
                {
                    Init();
                }
                if (_selfNode == null)
                {
                    throw new CoreException("尚没有设定这个节点，请先设定这个节点");
                }

                return _selfNode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool TryGetNodeById(string nodeId, out NodeDescriptor node)
        {
            if (nodeId == null)
            {
                throw new ArgumentNullException("nodeId");
            }
            if (!_initialized)
            {
                Init();
            }
            return _allNodesById.TryGetValue(nodeId, out node);
        }

        public bool TryGetNodeByPublicKey(string publicKey, out NodeDescriptor node)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException("publicKey");
            }
            if (!_initialized)
            {
                Init();
            }
            return _allNodesByPublicKey.TryGetValue(publicKey, out node);
        }

        #region GetNodeElementActions
        public IReadOnlyDictionary<Verb, NodeElementActionState> GetNodeElementActions(NodeDescriptor node, ElementDescriptor element)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (!_initialized)
            {
                Init();
            }
            return _actionSet[node, element];
        }
        #endregion

        public IEnumerable<ElementDescriptor> GetInfoIdElements(NodeDescriptor node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (!_initialized)
            {
                Init();
            }
            return _nodeCareSet.GetInfoIdElements(node);
        }

        public bool IsInfoIdElement(NodeDescriptor node, ElementDescriptor element)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (!_initialized)
            {
                Init();
            }
            return _nodeCareSet.IsInfoIdElement(node, element);
        }

        public IReadOnlyCollection<NodeElementCareState> GetNodeElementCares(NodeDescriptor node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (!_initialized)
            {
                Init();
            }
            return _nodeCareSet.GetNodeElementCares(node);
        }

        public IReadOnlyCollection<NodeOntologyCareState> GetNodeOntologyCares(NodeDescriptor node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (!_initialized)
            {
                Init();
            }
            return _nodeCareSet.GetNodeOntologyCares(node);
        }

        public IEnumerable<NodeOntologyCareState> GetNodeOntologyCares()
        {
            if (!_initialized)
            {
                Init();
            }
            return _nodeCareSet.GetNodeOntologyCares();
        }

        public bool IsCareforElement(NodeDescriptor node, ElementDescriptor element)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (!_initialized)
            {
                Init();
            }
            return _nodeCareSet.IsCareforElement(node, element);
        }

        public bool IsCareForOntology(NodeDescriptor node, OntologyDescriptor ontology)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (!_initialized)
            {
                Init();
            }
            return _nodeCareSet.IsCareForOntology(node, ontology);
        }

        public IReadOnlyDictionary<OrganizationState, NodeOntologyOrganizationState> GetNodeOntologyOrganizations(NodeDescriptor node, OntologyDescriptor ontology)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (ontology == null)
            {
                throw new ArgumentNullException("ontology");
            }
            if (!_initialized)
            {
                Init();
            }
            return _organizationSet[node, ontology];
        }

        public IEnumerable<NodeOntologyOrganizationState> GetNodeOntologyOrganizations()
        {
            if (!_initialized)
            {
                Init();
            }
            return _organizationSet.GetNodeOntologyOrganizations();
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
        public IEnumerator<NodeDescriptor> GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _allNodesById.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_initialized)
            {
                Init();
            }
            return _allNodesById.Values.GetEnumerator();
        }

        /// <summary>
        /// 初始化节点上下文
        /// </summary>
        private void Init()
        {
            if (!_initialized)
            {
                lock (_locker)
                {
                    if (!_initialized)
                    {
                        _allNodesById.Clear();
                        _allNodesByPublicKey.Clear();
                        var allNodes = _host.GetRequiredService<INodeHostBootstrap>().GetNodes();
                        foreach (var node in allNodes)
                        {
                            var nodeState = NodeState.Create(_host, node);
                            var descriptor = new NodeDescriptor(_host, nodeState);
                            _allNodesById.Add(node.Id.ToString(), descriptor);
                            if (_allNodesByPublicKey.ContainsKey(node.PublicKey))
                            {
                                throw new CoreException("重复的公钥" + node.PublicKey);
                            }
                            _allNodesByPublicKey.Add(node.PublicKey, descriptor);
                            if (node.Id.ToString().Equals(_host.Config.ThisNodeId, StringComparison.OrdinalIgnoreCase))
                            {
                                _selfNode = descriptor;
                            }
                            if (node.Id.ToString().Equals(_host.Config.CenterNodeId, StringComparison.OrdinalIgnoreCase))
                            {
                                _centerNode = descriptor;
                            }
                        }
                        _initialized = true;
                    }
                }
            }
        }

        #region MessageHandler
        private class MessageHandler :
            IHandler<AddNodeCommand>,
            IHandler<NodeAddedEvent>,
            IHandler<UpdateNodeCommand>,
            IHandler<NodeUpdatedEvent>,
            IHandler<RemoveNodeCommand>,
            IHandler<NodeRemovedEvent>
        {
            private readonly NodeSet _set;

            public MessageHandler(NodeSet set)
            {
                this._set = set;
            }

            public void Register()
            {
                var messageDispatcher = _set._host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(_set._host.Name));
                }
                messageDispatcher.Register((IHandler<AddNodeCommand>)this);
                messageDispatcher.Register((IHandler<NodeAddedEvent>)this);
                messageDispatcher.Register((IHandler<UpdateNodeCommand>)this);
                messageDispatcher.Register((IHandler<NodeUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<RemoveNodeCommand>)this);
                messageDispatcher.Register((IHandler<NodeRemovedEvent>)this);
            }

            public void Handle(AddNodeCommand message)
            {
                this.Handle(message.Input, true);
            }

            public void Handle(NodeAddedEvent message)
            {
                if (message.GetType() == typeof(PrivateNodeAddedEvent))
                {
                    return;
                }
                this.Handle(message.Output, false);
            }

            private void Handle(INodeCreateIo input, bool isCommand)
            {
                var host = _set._host;
                var locker = _set._locker;
                var allNodesById = _set._allNodesById;
                var allNodesByPublicKey = _set._allNodesByPublicKey;
                var nodeRepository = host.GetRequiredService<IRepository<Node>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (input.Id.HasValue)
                {
                    throw new ValidationException("标识是必须的");
                }
                Node entity;
                lock (locker)
                {
                    NodeDescriptor node;
                    if (host.NodeHost.Nodes.TryGetNodeById(input.Id.Value.ToString(), out node))
                    {
                        throw new ValidationException("已经存在");
                    }
                    if (host.NodeHost.Nodes.Any(a => a.Node.Code.Equals(input.Code)))
                    {
                        throw new ValidationException("重复的编码");
                    }

                    entity = Node.Create(input);

                    var state = new NodeDescriptor(host, NodeState.Create(host, entity));
                    allNodesById.Add(entity.Id.ToString(), state);
                    allNodesByPublicKey.Add(entity.PublicKey, state);
                    if (isCommand)
                    {
                        try
                        {
                            nodeRepository.Add(entity);
                            nodeRepository.Context.Commit();
                        }
                        catch
                        {
                            allNodesById.Remove(entity.Id.ToString());
                            allNodesByPublicKey.Remove(entity.PublicKey);
                            nodeRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateNodeAddedEvent(entity, input));
                }
            }

            private class PrivateNodeAddedEvent : NodeAddedEvent
            {
                public PrivateNodeAddedEvent(NodeBase source, INodeCreateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(UpdateNodeCommand message)
            {
                this.Handle(message.Output, true);
            }

            public void Handle(NodeUpdatedEvent message)
            {
                if (message.GetType() == typeof(PrivateNodeUpdatedEvent))
                {
                    return;
                }
                this.Handle(message.Output, false);
            }

            private void Handle(INodeUpdateIo input, bool isCommand)
            {
                var host = _set._host;
                var locker = _set._locker;
                var allNodesById = _set._allNodesById;
                var allNodesByPublicKey = _set._allNodesByPublicKey;
                var nodeRepository = host.GetRequiredService<IRepository<Node>>();
                if (string.IsNullOrEmpty(input.Code))
                {
                    throw new ValidationException("编码不能为空");
                }
                if (host.NodeHost.Nodes.Any(a => a.Node.Code.Equals(input.Code) && a.Node.Id != input.Id))
                {
                    throw new ValidationException("重复的编码");
                }
                Node entity;
                bool stateChanged = false;
                lock (locker)
                {
                    NodeDescriptor node;
                    if (!host.NodeHost.Nodes.TryGetNodeById(input.Id.ToString(), out node))
                    {
                        throw new NotExistException();
                    }
                    entity = nodeRepository.GetByKey(input.Id);
                    if (entity == null)
                    {
                        throw new NotExistException();
                    }
                    var bkState = new NodeDescriptor(host, NodeState.Create(host, entity));

                    entity.Update(input);

                    var newState = new NodeDescriptor(host, NodeState.Create(host, entity));
                    stateChanged = newState != bkState;
                    if (stateChanged)
                    {
                        Update(newState);
                    }
                    if (isCommand)
                    {
                        try
                        {
                            nodeRepository.Update(entity);
                            nodeRepository.Context.Commit();
                        }
                        catch
                        {
                            if (stateChanged)
                            {
                                Update(bkState);
                            }
                            nodeRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand && stateChanged)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateNodeUpdatedEvent(entity, input));
                }
            }

            private void Update(NodeDescriptor state)
            {
                var host = _set._host;
                var locker = _set._locker;
                var allNodesById = _set._allNodesById;
                var allNodesByPublicKey = _set._allNodesByPublicKey;
                var oldState = allNodesById[state.Node.Id.ToString()];
                allNodesById[state.Node.Id.ToString()] = state;
                if (!allNodesByPublicKey.ContainsKey(state.Node.PublicKey))
                {
                    allNodesByPublicKey.Add(state.Node.PublicKey, state);
                    allNodesByPublicKey.Remove(oldState.Node.PublicKey);
                }
                else
                {
                    allNodesByPublicKey[state.Node.PublicKey] = state;
                }
            }

            private class PrivateNodeUpdatedEvent : NodeUpdatedEvent
            {
                public PrivateNodeUpdatedEvent(NodeBase source, INodeUpdateIo input)
                    : base(source, input)
                {

                }
            }
            public void Handle(RemoveNodeCommand message)
            {
                this.Handle(message.EntityId, true);
            }

            public void Handle(NodeRemovedEvent message)
            {
                if (message.GetType() == typeof(PrivateNodeRemovedEvent))
                {
                    return;
                }
                this.Handle(message.Source.Id, false);
            }

            private void Handle(Guid nodeId, bool isCommand)
            {
                var host = _set._host;
                var locker = _set._locker;
                var allNodesById = _set._allNodesById;
                var allNodesByPublicKey = _set._allNodesByPublicKey;
                var nodeRepository = host.GetRequiredService<IRepository<Node>>();
                NodeDescriptor bkState;
                if (!host.NodeHost.Nodes.TryGetNodeById(nodeId.ToString(), out bkState))
                {
                    return;
                }
                Node entity;
                lock (locker)
                {
                    entity = nodeRepository.GetByKey(nodeId);
                    if (entity == null)
                    {
                        return;
                    }
                    allNodesById.Remove(entity.Id.ToString());
                    allNodesByPublicKey.Remove(entity.PublicKey);
                    if (isCommand)
                    {
                        try
                        {
                            nodeRepository.Remove(entity);
                            nodeRepository.Context.Commit();
                        }
                        catch
                        {
                            allNodesById.Add(entity.Id.ToString(), bkState);
                            allNodesByPublicKey.Add(entity.PublicKey, bkState);
                            nodeRepository.Context.Rollback();
                            throw;
                        }
                    }
                }
                if (isCommand)
                {
                    host.MessageDispatcher.DispatchMessage(new PrivateNodeRemovedEvent(entity));
                }
            }

            private class PrivateNodeRemovedEvent : NodeRemovedEvent
            {
                public PrivateNodeRemovedEvent(NodeBase source)
                    : base(source)
                {

                }
            }
        }
        #endregion

        // 内部类
        #region NodeElementActionSet
        private sealed class NodeElementActionSet
        {
            private readonly Dictionary<NodeDescriptor, Dictionary<ElementDescriptor, Dictionary<Verb, NodeElementActionState>>> _nodeElementActionDic = new Dictionary<NodeDescriptor, Dictionary<ElementDescriptor, Dictionary<Verb, NodeElementActionState>>>();
            private bool _initialized = false;

            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _host;

            public Guid Id
            {
                get { return _id; }
            }

            internal NodeElementActionSet(IAcDomain host)
            {
                if (host == null)
                {
                    throw new ArgumentNullException("host");
                }
                this._host = host;
                new NodeElementActionMessageHandler(this).Register();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="node"></param>
            /// <param name="element"></param>
            /// <returns></returns>
            public Dictionary<Verb, NodeElementActionState> this[NodeDescriptor node, ElementDescriptor element]
            {
                get
                {
                    if (!_initialized)
                    {
                        Init();
                    }
                    if (!_nodeElementActionDic.ContainsKey(node))
                    {
                        return new Dictionary<Verb, NodeElementActionState>();
                    }
                    if (!_nodeElementActionDic[node].ContainsKey(element))
                    {
                        return new Dictionary<Verb, NodeElementActionState>();
                    }

                    return _nodeElementActionDic[node][element];
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
                if (_initialized) return;
                lock (this)
                {
                    if (_initialized) return;
                    _nodeElementActionDic.Clear();
                    var nodeElementActions = _host.GetRequiredService<INodeHostBootstrap>().GetNodeElementActions();
                    foreach (var item in nodeElementActions)
                    {
                        NodeDescriptor node;
                        _host.NodeHost.Nodes.TryGetNodeById(item.NodeId.ToString(), out node);
                        ElementDescriptor element = _host.NodeHost.Ontologies.GetElement(item.ElementId);
                        if (!_nodeElementActionDic.ContainsKey(node))
                        {
                            _nodeElementActionDic.Add(node, new Dictionary<ElementDescriptor, Dictionary<Verb, NodeElementActionState>>());
                        }
                        if (!_nodeElementActionDic[node].ContainsKey(element))
                        {
                            _nodeElementActionDic[node].Add(element, new Dictionary<Verb, NodeElementActionState>());
                        }
                        var state = NodeElementActionState.Create(item);
                        var action = element.Ontology.Actions.Values.First(a => a.Id == item.ActionId);
                        _nodeElementActionDic[node][element].Add(action.ActionVerb, state);
                    }
                    _initialized = true;
                }
            }

            #endregion

            #region NodeElementActionMessageHandler
            private class NodeElementActionMessageHandler :
                IHandler<AddNodeElementActionCommand>,
                IHandler<NodeElementActionAddedEvent>,
                IHandler<RemoveNodeElementActionCommand>,
                IHandler<NodeElementActionRemovedEvent>
            {
                private readonly NodeElementActionSet _set;

                public NodeElementActionMessageHandler(NodeElementActionSet set)
                {
                    this._set = set;
                }

                public void Register()
                {
                    var messageDispatcher = _set._host.MessageDispatcher;
                    if (messageDispatcher == null)
                    {
                        throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(_set._host.Name));
                    }
                    messageDispatcher.Register((IHandler<AddNodeElementActionCommand>)this);
                    messageDispatcher.Register((IHandler<NodeElementActionAddedEvent>)this);
                    messageDispatcher.Register((IHandler<RemoveNodeElementActionCommand>)this);
                    messageDispatcher.Register((IHandler<NodeElementActionRemovedEvent>)this);
                }

                public void Handle(AddNodeElementActionCommand message)
                {
                    this.Handle(message.Input, true);
                }

                public void Handle(NodeElementActionAddedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeElementActionAddedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Output, false);
                }

                private void Handle(INodeElementActionCreateIo input, bool isCommand)
                {
                    var host = _set._host;
                    var nodeElementActionDic = _set._nodeElementActionDic;
                    var repository = host.GetRequiredService<IRepository<NodeElementAction>>();
                    NodeElementAction entity;
                    lock (this)
                    {
                        NodeDescriptor node;
                        if (!host.NodeHost.Nodes.TryGetNodeById(input.NodeId.ToString(), out node))
                        {
                            throw new ValidationException("意外的节点标识" + input.NodeId);
                        }
                        ElementDescriptor element;
                        if (!host.NodeHost.Ontologies.TryGetElement(input.ElementId, out element))
                        {
                            throw new ValidationException("意外的本体元素标识" + input.ElementId);
                        }
                        if (!nodeElementActionDic.ContainsKey(node))
                        {
                            nodeElementActionDic.Add(node, new Dictionary<ElementDescriptor, Dictionary<Verb, NodeElementActionState>>());
                        }
                        if (!nodeElementActionDic[node].ContainsKey(element))
                        {
                            nodeElementActionDic[node].Add(element, new Dictionary<Verb, NodeElementActionState>());
                        }
                        entity = new NodeElementAction
                        {
                            Id = input.Id.Value,
                            ActionId = input.ActionId,
                            ElementId = input.ElementId,
                            IsAllowed = input.IsAllowed,
                            IsAudit = input.IsAudit,
                            NodeId = input.NodeId
                        };
                        var state = NodeElementActionState.Create(entity);
                        var action = element.Ontology.Actions.Values.FirstOrDefault(a => a.Id == input.ActionId);
                        if (action == null)
                        {
                            throw new ValidationException("意外的本体动作标识" + input.ActionId);
                        }
                        nodeElementActionDic[node][element].Add(action.ActionVerb, state);
                        if (isCommand)
                        {
                            try
                            {
                                repository.Add(entity);
                                repository.Context.Commit();
                            }
                            catch
                            {
                                if (nodeElementActionDic.ContainsKey(node) && nodeElementActionDic[node].ContainsKey(element) && nodeElementActionDic[node][element].ContainsKey(action.ActionVerb))
                                {
                                    nodeElementActionDic[node][element].Remove(action.ActionVerb);
                                }
                                repository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeElementActionAddedEvent(entity, input));
                    }
                }

                private class PrivateNodeElementActionAddedEvent : NodeElementActionAddedEvent
                {
                    public PrivateNodeElementActionAddedEvent(NodeElementActionBase source, INodeElementActionCreateIo input)
                        : base(source, input)
                    {

                    }
                }

                public void Handle(RemoveNodeElementActionCommand message)
                {
                    this.Handle(message.EntityId, true);
                }

                public void Handle(NodeElementActionRemovedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeElementActionRemovedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Source.Id, false);
                }

                private void Handle(Guid nodeElementActionId, bool isCommand)
                {
                    var host = _set._host;
                    var nodeElementActionDic = _set._nodeElementActionDic;
                    var repository = host.GetRequiredService<IRepository<NodeElementAction>>();
                    NodeElementAction entity;
                    lock (this)
                    {
                        bool exist = false;
                        NodeElementActionState bkState = null;
                        NodeDescriptor node = null;
                        ElementDescriptor element = null;
                        foreach (var item in nodeElementActionDic)
                        {
                            foreach (var item1 in item.Value)
                            {
                                foreach (var item2 in item1.Value)
                                {
                                    if (item2.Value.Id == nodeElementActionId)
                                    {
                                        exist = true;
                                        bkState = item2.Value;
                                        break;
                                    }
                                }
                                if (exist)
                                {
                                    element = item1.Key;
                                    break;
                                }
                            }
                            if (exist)
                            {
                                node = item.Key;
                                break;
                            }
                        }
                        if (!exist)
                        {
                            return;
                        }
                        entity = repository.GetByKey(nodeElementActionId);
                        if (entity == null)
                        {
                            return;
                        }
                        if (nodeElementActionDic.ContainsKey(node) && nodeElementActionDic[node].ContainsKey(element))
                        {
                            var action = element.Ontology.Actions.Values.FirstOrDefault(a => a.Id == entity.ActionId);
                            nodeElementActionDic[node][element].Remove(action.ActionVerb);
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
                                var action = element.Ontology.Actions.Values.FirstOrDefault(a => a.Id == entity.ActionId);
                                if (nodeElementActionDic.ContainsKey(node) && nodeElementActionDic[node].ContainsKey(element) && !nodeElementActionDic[node][element].ContainsKey(action.ActionVerb))
                                {
                                    nodeElementActionDic[node][element].Add(action.ActionVerb, bkState);
                                }
                                repository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeElementActionRemovedEvent(entity));
                    }
                }

                private class PrivateNodeElementActionRemovedEvent : NodeElementActionRemovedEvent
                {
                    public PrivateNodeElementActionRemovedEvent(NodeElementAction source)
                        : base(source)
                    {

                    }
                }
            }
            #endregion
        }
        #endregion

        // 内部类
        #region NodeCareSet
        /// <summary>
        /// 节点关心本体和节点关心本体元素
        /// </summary>
        private sealed class NodeCareSet
        {
            private readonly Dictionary<NodeDescriptor, IDictionary<ontologyId, isCare>> _ontologyCareDic = new Dictionary<NodeDescriptor, IDictionary<ontologyId, isCare>>();
            private readonly Dictionary<NodeDescriptor, IDictionary<elementId, isCare>> _elementCareDic = new Dictionary<NodeDescriptor, IDictionary<elementId, isCare>>();
            private readonly Dictionary<NodeDescriptor, List<NodeOntologyCareState>> _nodeOntologyCareList = new Dictionary<NodeDescriptor, List<NodeOntologyCareState>>();
            private readonly Dictionary<NodeDescriptor, List<NodeElementCareState>> _nodeElementCareList = new Dictionary<NodeDescriptor, List<NodeElementCareState>>();
            private readonly Dictionary<NodeDescriptor, HashSet<ElementDescriptor>> _nodeInfoIdElements = new Dictionary<NodeDescriptor, HashSet<ElementDescriptor>>();
            private bool _initialized = false;
            private readonly object _locker = new object();
            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _host;

            public Guid Id
            {
                get { return _id; }
            }

            internal NodeCareSet(IAcDomain host)
            {
                if (host == null)
                {
                    throw new ArgumentNullException("host");
                }
                this._host = host;
                new MessageHandler(this).Register();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            public IEnumerable<ElementDescriptor> GetInfoIdElements(NodeDescriptor node)
            {
                if (node == null)
                {
                    throw new ArgumentNullException("node");
                }
                if (!_initialized)
                {
                    Init();
                }
                return _nodeInfoIdElements[node];
            }

            public bool IsInfoIdElement(NodeDescriptor node, ElementDescriptor element)
            {
                if (node == null)
                {
                    throw new ArgumentNullException("node");
                }
                if (element == null)
                {
                    throw new ArgumentNullException("element");
                }
                if (!_initialized)
                {
                    Init();
                }
                return _nodeInfoIdElements.ContainsKey(node) && _nodeInfoIdElements[node].Contains(element);
            }

            /// <summary>
            /// 判断本节点是否关心给定的本体元素
            /// </summary>
            /// <param name="node"></param>
            /// <param name="element">本体元素码</param>
            /// <returns>True表示关心，False表示不关心</returns>
            public bool IsCareforElement(NodeDescriptor node, ElementDescriptor element)
            {
                if (node == null)
                {
                    throw new ArgumentNullException("node");
                }
                if (element == null)
                {
                    throw new ArgumentNullException("element");
                }
                if (!_initialized)
                {
                    Init();
                }
                if (!_elementCareDic.ContainsKey(node))
                {
                    return false;
                }
                if (!_ontologyCareDic[node].ContainsKey(element.Element.OntologyId))
                {
                    return false;
                }
                if (!_elementCareDic[node].ContainsKey(element.Element.Id))
                {
                    return false;
                }

                return _elementCareDic[node][element.Element.Id];
            }

            /// <summary>
            /// 判断本节点是否关心给定的本体
            /// </summary>
            /// <param name="node"></param>
            /// <param name="ontology"></param>
            /// <returns>True表示关心，False表示不关心</returns>
            public bool IsCareForOntology(NodeDescriptor node, OntologyDescriptor ontology)
            {
                if (node == null)
                {
                    throw new ArgumentNullException("node");
                }
                if (ontology == null)
                {
                    throw new ArgumentNullException("ontology");
                }
                if (!_initialized)
                {
                    Init();
                }
                if (!_ontologyCareDic.ContainsKey(node))
                {
                    return false;
                }
                if (!_ontologyCareDic[node].ContainsKey(ontology.Ontology.Id))
                {
                    return false;
                }

                return _ontologyCareDic[node][ontology.Ontology.Id];
            }

            public IReadOnlyCollection<NodeOntologyCareState> GetNodeOntologyCares(NodeDescriptor node)
            {
                if (node == null)
                {
                    throw new ArgumentNullException("node");
                }
                if (!_initialized)
                {
                    Init();
                }
                if (!_nodeOntologyCareList.ContainsKey(node))
                {
                    return new List<NodeOntologyCareState>();
                }
                return _nodeOntologyCareList[node];
            }

            public IEnumerable<NodeOntologyCareState> GetNodeOntologyCares()
            {
                if (!_initialized)
                {
                    Init();
                }
                foreach (var g in _nodeOntologyCareList)
                {
                    foreach (var item in g.Value)
                    {
                        yield return item;
                    }
                }
            }

            public IReadOnlyCollection<NodeElementCareState> GetNodeElementCares(NodeDescriptor node)
            {
                if (node == null)
                {
                    throw new ArgumentNullException("node");
                }
                if (!_initialized)
                {
                    Init();
                }
                if (!_nodeElementCareList.ContainsKey(node))
                {
                    return new List<NodeElementCareState>();
                }
                return _nodeElementCareList[node];
            }

            #region Init
            private void Init()
            {
                if (!_initialized)
                {
                    lock (_locker)
                    {
                        if (!_initialized)
                        {
                            _ontologyCareDic.Clear();
                            _elementCareDic.Clear();
                            _nodeOntologyCareList.Clear();
                            _nodeElementCareList.Clear();
                            _nodeInfoIdElements.Clear();
                            var nodeOntologyCareStates = _host.GetRequiredService<INodeHostBootstrap>().GetNodeOntologyCares().Select(NodeOntologyCareState.Create);
                            var nodeElementCareStates = _host.GetRequiredService<INodeHostBootstrap>().GetNodeElementCares().Select(NodeElementCareState.Create);
                            foreach (var node in _host.NodeHost.Nodes)
                            {
                                var node1 = node;
                                _nodeOntologyCareList.Add(node, nodeOntologyCareStates.Where(a => a.NodeId == node1.Node.Id).ToList());
                                var node2 = node;
                                _nodeElementCareList.Add(node, nodeElementCareStates.Where(a => a.NodeId == node2.Node.Id).ToList());
                            }

                            foreach (var ontology in _host.NodeHost.Ontologies)
                            {
                                foreach (var element in _host.NodeHost.Ontologies[ontology.Ontology.Id].Elements.Values)
                                {
                                    foreach (var node in _host.NodeHost.Nodes)
                                    {
                                        if (element == null)
                                        {
                                            return;
                                        }
                                        if (!_ontologyCareDic.ContainsKey(node))
                                        {
                                            _ontologyCareDic.Add(node, new Dictionary<ontologyId, isCare>());
                                        }
                                        if (!_ontologyCareDic[node].ContainsKey(element.Element.OntologyId))
                                        {
                                            var element1 = element;
                                            _ontologyCareDic[node].Add(element.Element.OntologyId, _nodeOntologyCareList[node]
                                                .Any(s => s.OntologyId == element1.Element.OntologyId));
                                        }
                                        if (!_elementCareDic.ContainsKey(node))
                                        {
                                            _elementCareDic.Add(node, new Dictionary<elementId, isCare>());
                                        }
                                        if (!_nodeInfoIdElements.ContainsKey(node))
                                        {
                                            _nodeInfoIdElements.Add(node, new HashSet<ElementDescriptor>());
                                            _nodeInfoIdElements[node].Add(ontology.IdElement);
                                        }
                                        if (!_elementCareDic[node].ContainsKey(element.Element.Id))
                                        {
                                            var element2 = element;
                                            var nodeElementCare = _nodeElementCareList[node].FirstOrDefault(f => f.ElementId == element2.Element.Id);
                                            _elementCareDic[node].Add(element.Element.Id, nodeElementCare != null);
                                            if (nodeElementCare != null && nodeElementCare.IsInfoIdItem)
                                            {
                                                _nodeInfoIdElements[node].Add(element);
                                            }
                                        }
                                    }
                                }
                            }
                            _initialized = true;
                        }
                    }
                }
            }
            #endregion

            private class MessageHandler :
                IHandler<AddNodeOntologyCareCommand>,
                IHandler<NodeOntologyCareAddedEvent>,
                IHandler<RemoveNodeOntologyCareCommand>,
                IHandler<NodeOntologyCareRemovedEvent>,
                IHandler<AddNodeElementCareCommand>,
                IHandler<NodeElementCareAddedEvent>,
                IHandler<UpdateNodeElementCareCommand>,
                IHandler<NodeElementCareUpdatedEvent>,
                IHandler<RemoveNodeElementCareCommand>,
                IHandler<NodeElementCareRemovedEvent>
            {
                private readonly NodeCareSet set;

                public MessageHandler(NodeCareSet set)
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
                    messageDispatcher.Register((IHandler<AddNodeOntologyCareCommand>)this);
                    messageDispatcher.Register((IHandler<NodeOntologyCareAddedEvent>)this);
                    messageDispatcher.Register((IHandler<RemoveNodeOntologyCareCommand>)this);
                    messageDispatcher.Register((IHandler<NodeOntologyCareRemovedEvent>)this);
                    messageDispatcher.Register((IHandler<AddNodeElementCareCommand>)this);
                    messageDispatcher.Register((IHandler<NodeElementCareAddedEvent>)this);
                    messageDispatcher.Register((IHandler<UpdateNodeElementCareCommand>)this);
                    messageDispatcher.Register((IHandler<NodeElementCareUpdatedEvent>)this);
                    messageDispatcher.Register((IHandler<RemoveNodeElementCareCommand>)this);
                    messageDispatcher.Register((IHandler<NodeElementCareRemovedEvent>)this);
                }

                public void Handle(AddNodeOntologyCareCommand message)
                {
                    this.Handle(message.Input, true);
                }

                public void Handle(NodeOntologyCareAddedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeOntologyCareAddedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Output, false);
                }

                private void Handle(INodeOntologyCareCreateIo input, bool isCommand)
                {
                    var host = set._host;
                    var nodeOntologyCareList = set._nodeOntologyCareList;
                    var ontologyCareDic = set._ontologyCareDic;
                    var repository = host.GetRequiredService<IRepository<NodeOntologyCare>>();
                    NodeDescriptor bNode;
                    if (!host.NodeHost.Nodes.TryGetNodeById(input.NodeId.ToString(), out bNode))
                    {
                        throw new ValidationException("意外的节点标识" + input.NodeId);
                    }
                    OntologyDescriptor ontology;
                    if (!host.NodeHost.Ontologies.TryGetOntology(input.OntologyId, out ontology))
                    {
                        throw new ValidationException("意外的本体标识" + input.OntologyId);
                    }
                    NodeOntologyCare entity;
                    lock (this)
                    {
                        if (nodeOntologyCareList[bNode].Any(a => a.OntologyId == input.OntologyId && a.NodeId == input.NodeId))
                        {
                            throw new ValidationException("给定的节点已关心给定的本体，无需重复关心");
                        }
                        entity = NodeOntologyCare.Create(input);
                        var state = NodeOntologyCareState.Create(entity);
                        if (!nodeOntologyCareList.ContainsKey(bNode))
                        {
                            nodeOntologyCareList.Add(bNode, new List<NodeOntologyCareState>());
                        }
                        if (!nodeOntologyCareList[bNode].Contains(state))
                        {
                            nodeOntologyCareList[bNode].Add(state);
                        }
                        if (!ontologyCareDic.ContainsKey(bNode))
                        {
                            ontologyCareDic.Add(bNode, new Dictionary<ontologyId, isCare>());
                        }
                        if (!ontologyCareDic[bNode].ContainsKey(input.OntologyId))
                        {
                            ontologyCareDic[bNode].Add(input.OntologyId, true);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                repository.Add(entity);
                                repository.Context.Commit();
                            }
                            catch
                            {
                                if (nodeOntologyCareList.ContainsKey(bNode) && nodeOntologyCareList[bNode].Contains(state))
                                {
                                    nodeOntologyCareList[bNode].Remove(state);
                                }
                                if (ontologyCareDic.ContainsKey(bNode) && ontologyCareDic[bNode].ContainsKey(input.OntologyId))
                                {
                                    ontologyCareDic[bNode].Remove(input.OntologyId);
                                }
                                repository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeOntologyCareAddedEvent(entity, input));
                    }
                }

                private class PrivateNodeOntologyCareAddedEvent : NodeOntologyCareAddedEvent
                {
                    public PrivateNodeOntologyCareAddedEvent(NodeOntologyCareBase source, INodeOntologyCareCreateIo input)
                        : base(source, input)
                    {

                    }
                }

                public void Handle(RemoveNodeOntologyCareCommand message)
                {
                    this.Handle(message.EntityId, true);
                }

                public void Handle(NodeOntologyCareRemovedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeOntologyCareRemovedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Source.Id, false);
                }

                private void Handle(Guid nodeOntologyCareId, bool isCommand)
                {
                    var host = set._host;
                    var nodeOntologyCareList = set._nodeOntologyCareList;
                    var ontologyCareDic = set._ontologyCareDic;
                    var repository = host.GetRequiredService<IRepository<NodeOntologyCare>>();
                    NodeOntologyCare entity;
                    lock (this)
                    {
                        NodeOntologyCareState bkState = null;
                        NodeDescriptor bNode = null;
                        foreach (var item in nodeOntologyCareList)
                        {
                            foreach (var item1 in item.Value)
                            {
                                if (item1.Id == nodeOntologyCareId)
                                {
                                    bkState = item1;
                                    break;
                                }
                            }
                            if (bkState != null)
                            {
                                bNode = item.Key;
                            }
                        }
                        if (bkState == null)
                        {
                            return;
                        }
                        entity = repository.GetByKey(nodeOntologyCareId);
                        if (entity == null)
                        {
                            return;
                        }
                        nodeOntologyCareList[bNode].Remove(bkState);
                        ontologyCareDic[bNode].Remove(bkState.OntologyId);
                        try
                        {
                            if (isCommand)
                            {
                                repository.Remove(entity);
                                repository.Context.Commit();
                            }
                        }
                        catch
                        {
                            nodeOntologyCareList[bNode].Add(bkState);
                            ontologyCareDic[bNode].Add(bkState.OntologyId, true);
                            repository.Context.Rollback();
                            throw;
                        }
                    }

                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeOntologyCareRemovedEvent(entity));
                    }
                }

                private class PrivateNodeOntologyCareRemovedEvent : NodeOntologyCareRemovedEvent
                {
                    public PrivateNodeOntologyCareRemovedEvent(NodeOntologyCareBase source) : base(source) { }

                }

                public void Handle(AddNodeElementCareCommand message)
                {
                    this.Handle(message.Input, true);
                }

                public void Handle(NodeElementCareAddedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeElementCareAddedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Output, false);
                }

                private void Handle(INodeElementCareCreateIo input, bool isCommand)
                {
                    var host = set._host;
                    var nodeElementCareList = set._nodeElementCareList;
                    var elementCareDic = set._elementCareDic;
                    var repository = host.GetRequiredService<IRepository<NodeElementCare>>();
                    NodeDescriptor bNode;
                    if (!host.NodeHost.Nodes.TryGetNodeById(input.NodeId.ToString(), out bNode))
                    {
                        throw new ValidationException("意外的节点标识" + input.NodeId);
                    }
                    ElementDescriptor element;
                    if (!host.NodeHost.Ontologies.TryGetElement(input.ElementId, out element))
                    {
                        throw new ValidationException("意外的本体元素标识" + input.ElementId);
                    }
                    NodeElementCare entity;
                    lock (this)
                    {
                        if (nodeElementCareList[bNode].Any(a => a.ElementId == input.ElementId && a.NodeId == input.NodeId))
                        {
                            throw new ValidationException("给定的节点已关心给定的本体元素，无需重复关心");
                        }
                        entity = NodeElementCare.Create(input);
                        var state = NodeElementCareState.Create(entity);
                        if (!nodeElementCareList.ContainsKey(bNode))
                        {
                            nodeElementCareList.Add(bNode, new List<NodeElementCareState>());
                        }
                        if (!nodeElementCareList[bNode].Contains(state))
                        {
                            nodeElementCareList[bNode].Add(state);
                        }
                        if (!elementCareDic.ContainsKey(bNode))
                        {
                            elementCareDic.Add(bNode, new Dictionary<elementId, isCare>());
                        }
                        if (!elementCareDic[bNode].ContainsKey(input.ElementId))
                        {
                            elementCareDic[bNode].Add(input.ElementId, true);
                        }
                        if (isCommand)
                        {
                            try
                            {
                                repository.Add(entity);
                                repository.Context.Commit();
                            }
                            catch
                            {
                                if (nodeElementCareList.ContainsKey(bNode) && nodeElementCareList[bNode].Contains(state))
                                {
                                    nodeElementCareList[bNode].Remove(state);
                                }
                                if (elementCareDic.ContainsKey(bNode) && elementCareDic[bNode].ContainsKey(input.ElementId))
                                {
                                    elementCareDic[bNode].Remove(input.ElementId);
                                }
                                repository.Context.Rollback();
                                throw;
                            }
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeElementCareAddedEvent(entity, input));
                    }
                }

                private class PrivateNodeElementCareAddedEvent : NodeElementCareAddedEvent
                {
                    public PrivateNodeElementCareAddedEvent(NodeElementCareBase source, INodeElementCareCreateIo input)
                        : base(source, input)
                    {

                    }
                }

                public void Handle(UpdateNodeElementCareCommand message)
                {
                    this.Handle(message.NodeElementCareId, message.IsInfoIdItem, true);
                }

                public void Handle(NodeElementCareUpdatedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeElementCareUpdatedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Source.Id, message.IsInfoIdItem, false);
                }

                private void Handle(Guid nodeElementCareId, bool isInfoIdItem, bool isCommand)
                {
                    var host = set._host;
                    var nodeElementCareList = set._nodeElementCareList;
                    var elementCareDic = set._elementCareDic;
                    var nodeInfoIdElements = set._nodeInfoIdElements;
                    var repository = host.GetRequiredService<IRepository<NodeElementCare>>();
                    NodeElementCare entity;
                    lock (this)
                    {
                        NodeElementCareState bkState = null;
                        NodeDescriptor bNode = null;
                        foreach (var item in nodeElementCareList)
                        {
                            foreach (var item1 in item.Value)
                            {
                                if (item1.Id == nodeElementCareId)
                                {
                                    bkState = item1;
                                    break;
                                }
                            }
                            if (bkState != null)
                            {
                                bNode = item.Key;
                            }
                        }
                        if (bkState == null)
                        {
                            throw new NotExistException();
                        }
                        entity = repository.GetByKey(nodeElementCareId);
                        if (entity == null)
                        {
                            throw new NotExistException();
                        }
                        ElementDescriptor element;
                        if (!host.NodeHost.Ontologies.TryGetElement(entity.ElementId, out element))
                        {
                            throw new ValidationException("意外的本体元素标识" + entity.ElementId);
                        }
                        entity.IsInfoIdItem = isInfoIdItem;
                        var newState = NodeElementCareState.Create(entity);
                        nodeElementCareList[bNode].Remove(bkState);
                        nodeElementCareList[bNode].Add(newState);
                        nodeInfoIdElements[bNode].Add(element);
                        try
                        {
                            if (isCommand)
                            {
                                repository.Update(entity);
                                repository.Context.Commit();
                            }
                        }
                        catch
                        {
                            nodeElementCareList[bNode].Remove(newState);
                            nodeElementCareList[bNode].Add(bkState);
                            nodeInfoIdElements[bNode].Remove(element);
                            repository.Context.Rollback();
                            throw;
                        }
                    }

                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeElementCareUpdatedEvent(entity));
                    }
                }

                private class PrivateNodeElementCareUpdatedEvent : NodeElementCareUpdatedEvent
                {
                    public PrivateNodeElementCareUpdatedEvent(NodeElementCareBase source)
                        : base(source)
                    {

                    }
                }

                public void Handle(RemoveNodeElementCareCommand message)
                {
                    this.Handle(message.EntityId, true);
                }

                public void Handle(NodeElementCareRemovedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeElementCareRemovedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Source.Id, false);
                }

                private void HandleElement(Guid nodeElementCareId, bool isCommand)
                {
                    var host = set._host;
                    var nodeElementCareList = set._nodeElementCareList;
                    var elementCareDic = set._elementCareDic;
                    var nodeInfoIdElements = set._nodeInfoIdElements;
                    var repository = host.GetRequiredService<IRepository<NodeElementCare>>();
                    NodeElementCare entity;
                    lock (this)
                    {
                        NodeElementCareState bkState = null;
                        NodeDescriptor bNode = null;
                        foreach (var item in nodeElementCareList)
                        {
                            foreach (var item1 in item.Value)
                            {
                                if (item1.Id == nodeElementCareId)
                                {
                                    bkState = item1;
                                    break;
                                }
                            }
                            if (bkState != null)
                            {
                                bNode = item.Key;
                            }
                        }
                        if (bkState == null)
                        {
                            return;
                        }
                        entity = repository.GetByKey(nodeElementCareId);
                        if (entity == null)
                        {
                            return;
                        }
                        ElementDescriptor element;
                        if (!host.NodeHost.Ontologies.TryGetElement(entity.ElementId, out element))
                        {
                            throw new ValidationException("意外的本体元素标识" + entity.ElementId);
                        }
                        nodeElementCareList[bNode].Remove(bkState);
                        elementCareDic[bNode].Remove(bkState.ElementId);
                        bool isInfoIdElement = false;
                        if (nodeInfoIdElements.ContainsKey(bNode) && nodeInfoIdElements[bNode].Contains(element))
                        {
                            isInfoIdElement = true;
                            nodeInfoIdElements[bNode].Remove(element);
                        }
                        try
                        {
                            if (isCommand)
                            {
                                repository.Remove(entity);
                                repository.Context.Commit();
                            }
                        }
                        catch
                        {
                            nodeElementCareList[bNode].Add(bkState);
                            elementCareDic[bNode].Add(bkState.ElementId, true);
                            if (isInfoIdElement)
                            {
                                nodeInfoIdElements[bNode].Add(element);
                            }
                            repository.Context.Rollback();
                            throw;
                        }
                    }

                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeElementCareRemovedEvent(entity));
                    }
                }

                private class PrivateNodeElementCareRemovedEvent : NodeElementCareRemovedEvent
                {
                    public PrivateNodeElementCareRemovedEvent(NodeElementCareBase source) : base(source) { }

                }
            }
        }
        #endregion

        // 内部类
        #region OrganizationSet
        private sealed class OrganizationSet
        {
            private readonly Dictionary<NodeDescriptor, Dictionary<OntologyDescriptor, Dictionary<OrganizationState, NodeOntologyOrganizationState>>>
                _dic = new Dictionary<NodeDescriptor, Dictionary<OntologyDescriptor, Dictionary<OrganizationState, NodeOntologyOrganizationState>>>();
            private bool _initialized = false;
            private readonly object _locker = new object();
            private readonly Guid _id = Guid.NewGuid();
            private readonly IAcDomain _host;

            public Guid Id
            {
                get { return _id; }
            }

            public OrganizationSet(IAcDomain host)
            {
                if (host == null)
                {
                    throw new ArgumentNullException("host");
                }
                this._host = host;
                var messageDispatcher = host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(host.Name));
                }
                new MessageHandler(this).Register();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name = "node"></param>
            /// <param name="ontology"></param>
            /// <returns>key为组织结构码</returns>
            public Dictionary<OrganizationState, NodeOntologyOrganizationState> this[NodeDescriptor node, OntologyDescriptor ontology]
            {
                get
                {
                    if (node == null)
                    {
                        throw new ArgumentNullException("node");
                    }
                    if (ontology == null)
                    {
                        throw new ArgumentNullException("ontology");
                    }
                    if (!_initialized)
                    {
                        Init();
                    }
                    if (!_dic.ContainsKey(node))
                    {
                        return new Dictionary<OrganizationState, NodeOntologyOrganizationState>();
                    }
                    if (!_dic[node].ContainsKey(ontology))
                    {
                        return new Dictionary<OrganizationState, NodeOntologyOrganizationState>();
                    }

                    return _dic[node][ontology];
                }
            }

            public IEnumerable<NodeOntologyOrganizationState> GetNodeOntologyOrganizations()
            {
                if (!_initialized)
                {
                    Init();
                }
                foreach (var gg in _dic.Values)
                {
                    foreach (var g in gg.Values)
                    {
                        foreach (var item in g.Values)
                        {
                            yield return item;
                        }
                    }
                }
            }

            private void Init()
            {
                if (!_initialized)
                {
                    lock (_locker)
                    {
                        if (!_initialized)
                        {
                            _dic.Clear();
                            var ontologyOrgs = _host.GetRequiredService<INodeHostBootstrap>().GetNodeOntologyOrganizations();
                            foreach (var nodeOntologyOrg in ontologyOrgs)
                            {
                                OrganizationState org;
                                NodeDescriptor node;
                                OntologyDescriptor ontology;
                                _host.NodeHost.Nodes.TryGetNodeById(nodeOntologyOrg.NodeId.ToString(), out node);
                                _host.NodeHost.Ontologies.TryGetOntology(nodeOntologyOrg.OntologyId, out ontology);
                                if (_host.OrganizationSet.TryGetOrganization(nodeOntologyOrg.OrganizationId, out org))
                                {
                                    if (!_dic.ContainsKey(node))
                                    {
                                        _dic.Add(node, new Dictionary<OntologyDescriptor, Dictionary<OrganizationState, NodeOntologyOrganizationState>>());
                                    }
                                    if (!_dic[node].ContainsKey(ontology))
                                    {
                                        _dic[node].Add(ontology, new Dictionary<OrganizationState, NodeOntologyOrganizationState>());
                                    }
                                    var nodeOntologyOrgState = NodeOntologyOrganizationState.Create(_host, nodeOntologyOrg);
                                    _dic[node][ontology].Add(org, nodeOntologyOrgState);
                                }
                                else
                                {
                                    // TODO:移除废弃的组织结构
                                }
                            }
                            _initialized = true;
                        }
                    }
                }
            }

            #region MessageHandler
            private class MessageHandler :
                IHandler<AddNodeOntologyOrganizationCommand>,
                IHandler<NodeOntologyOrganizationAddedEvent>,
                IHandler<RemoveNodeOntologyOrganizationCommand>,
                IHandler<NodeOntologyOrganizationRemovedEvent>
            {
                private readonly OrganizationSet set;

                public MessageHandler(OrganizationSet set)
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
                    messageDispatcher.Register((IHandler<AddNodeOntologyOrganizationCommand>)this);
                    messageDispatcher.Register((IHandler<NodeOntologyOrganizationAddedEvent>)this);
                    messageDispatcher.Register((IHandler<RemoveNodeOntologyOrganizationCommand>)this);
                    messageDispatcher.Register((IHandler<NodeOntologyOrganizationRemovedEvent>)this);
                }

                public void Handle(AddNodeOntologyOrganizationCommand message)
                {
                    this.Handle(message.Input, true);
                }

                public void Handle(NodeOntologyOrganizationAddedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeOntologyOrganizationAddedEvent))
                    {
                        return;
                    }
                    this.Handle(message.Output, false);
                }

                private void Handle(INodeOntologyOrganizationCreateIo input, bool isCommand)
                {
                    var host = set._host;
                    var _dic = set._dic;
                    var repository = host.GetRequiredService<IRepository<NodeOntologyOrganization>>();
                    if (!input.Id.HasValue)
                    {
                        throw new ValidationException("标识是必须的");
                    }
                    NodeDescriptor node;
                    if (!host.NodeHost.Nodes.TryGetNodeById(input.NodeId.ToString(), out node))
                    {
                        throw new ValidationException("意外的节点标识" + input.NodeId);
                    }
                    OntologyDescriptor ontology;
                    if (!host.NodeHost.Ontologies.TryGetOntology(input.OntologyId, out ontology))
                    {
                        throw new ValidationException("意外的本体标识" + input.OntologyId);
                    }
                    OrganizationState organization;
                    if (!host.OrganizationSet.TryGetOrganization(input.OrganizationId, out organization))
                    {
                        throw new ValidationException("意外的组织结构标识" + input.OrganizationId);
                    }
                    NodeOntologyOrganization entity;
                    lock (set._locker)
                    {
                        if (_dic.ContainsKey(node) && _dic[node].ContainsKey(ontology) && _dic[node][ontology].ContainsKey(organization))
                        {
                            return;
                        }
                        entity = new NodeOntologyOrganization()
                        {
                            Id = input.Id.Value,
                            NodeId = input.NodeId,
                            OntologyId = input.OntologyId,
                            OrganizationId = input.OrganizationId
                        };
                        try
                        {
                            var state = NodeOntologyOrganizationState.Create(host, entity);
                            if (!_dic.ContainsKey(node))
                            {
                                _dic.Add(node, new Dictionary<OntologyDescriptor, Dictionary<OrganizationState, NodeOntologyOrganizationState>>());
                            }
                            if (!_dic[node].ContainsKey(ontology))
                            {
                                _dic[node].Add(ontology, new Dictionary<OrganizationState, NodeOntologyOrganizationState>());
                            }
                            if (!_dic[node][ontology].ContainsKey(organization))
                            {
                                _dic[node][ontology].Add(organization, state);
                            }
                            repository.Add(entity);
                            repository.Context.Commit();
                        }
                        catch
                        {
                            if (_dic.ContainsKey(node) && _dic[node].ContainsKey(ontology) && _dic[node][ontology].ContainsKey(organization))
                            {
                                _dic[node][ontology].Remove(organization);
                            }
                            throw;
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeOntologyOrganizationAddedEvent(entity, input));
                    }
                }

                private class PrivateNodeOntologyOrganizationAddedEvent : NodeOntologyOrganizationAddedEvent
                {
                    public PrivateNodeOntologyOrganizationAddedEvent(NodeOntologyOrganizationBase source, INodeOntologyOrganizationCreateIo input)
                        : base(source, input)
                    {

                    }
                }

                public void Handle(RemoveNodeOntologyOrganizationCommand message)
                {
                    this.Handle(message.NodeId, message.OntologyId, message.OrganizationId, true);
                }

                public void Handle(NodeOntologyOrganizationRemovedEvent message)
                {
                    if (message.GetType() == typeof(PrivateNodeOntologyOrganizationRemovedEvent))
                    {
                        return;
                    }
                    var entity = message.Source as NodeOntologyOrganizationBase;
                    this.Handle(entity.NodeId, entity.OntologyId, entity.OrganizationId, false);
                }

                private void Handle(Guid nodeId, Guid ontologyId, Guid organizationId, bool isCommand)
                {
                    var host = set._host;
                    var _dic = set._dic;
                    var repository = host.GetRequiredService<IRepository<NodeOntologyOrganization>>();
                    NodeDescriptor node;
                    if (!host.NodeHost.Nodes.TryGetNodeById(nodeId.ToString(), out node))
                    {
                        throw new ValidationException("意外的节点标识" + nodeId);
                    }
                    OntologyDescriptor ontology;
                    if (!host.NodeHost.Ontologies.TryGetOntology(ontologyId, out ontology))
                    {
                        throw new ValidationException("意外的本体标识" + ontologyId);
                    }
                    OrganizationState organization;
                    if (!host.OrganizationSet.TryGetOrganization(organizationId, out organization))
                    {
                        throw new ValidationException("意外的组织结构标识" + organizationId);
                    }
                    if (!_dic.ContainsKey(node) && !_dic[node].ContainsKey(ontology) && !_dic[node][ontology].ContainsKey(organization))
                    {
                        return;
                    }
                    var bkState = _dic[node][ontology][organization];
                    NodeOntologyOrganization entity;
                    lock (bkState)
                    {
                        entity = repository.AsQueryable().FirstOrDefault(a => a.OntologyId == ontologyId && a.NodeId == nodeId && a.OrganizationId == organizationId);
                        if (entity == null)
                        {
                            return;
                        }
                        try
                        {
                            _dic[node][ontology].Remove(organization);
                            repository.Remove(entity);
                        }
                        catch
                        {
                            _dic[node][ontology].Add(organization, bkState);
                            throw;
                        }
                    }
                    if (isCommand)
                    {
                        host.MessageDispatcher.DispatchMessage(new PrivateNodeOntologyOrganizationRemovedEvent(entity));
                    }
                }

                private class PrivateNodeOntologyOrganizationRemovedEvent : NodeOntologyOrganizationRemovedEvent
                {
                    public PrivateNodeOntologyOrganizationRemovedEvent(NodeOntologyOrganizationBase source) : base(source) { }
                }
            }
            #endregion
        }
        #endregion
    }
}