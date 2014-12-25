
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Exceptions;
    using Host.Hecp;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class NodeState : StateObject<NodeState>, INode, IStateObject
    {
        private Dictionary<OntologyDescriptor, Dictionary<Verb, INodeAction>> _nodeActionDic;
        private readonly IAcDomain _host;

        private NodeState(IAcDomain host, Guid id)
            : base(id)
        {
            this._host = host;
        }

        public static NodeState Create(IAcDomain host, INode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            var data = new NodeState(host, node.Id)
            {
                Abstract = node.Abstract,
                Actions = node.Actions,
                AnycmdApiAddress = node.AnycmdApiAddress,
                AnycmdWsAddress = node.AnycmdWsAddress,
                BeatPeriod = node.BeatPeriod,
                Code = node.Code,
                CreateOn = node.CreateOn,
                Email = node.Email,
                Icon = node.Icon,
                IsDistributeEnabled = node.IsDistributeEnabled,
                IsEnabled = node.IsEnabled,
                IsExecuteEnabled = node.IsExecuteEnabled,
                IsProduceEnabled = node.IsProduceEnabled,
                IsReceiveEnabled = node.IsReceiveEnabled,
                Mobile = node.Mobile,
                Name = node.Name,
                Organization = node.Organization,
                PublicKey = node.PublicKey,
                Qq = node.Qq,
                SecretKey = node.SecretKey,
                SortCode = node.SortCode,
                Steward = node.Steward,
                Telephone = node.Telephone,
                TransferId = node.TransferId
            };
            var nodeActionDic = new Dictionary<OntologyDescriptor, Dictionary<Verb, INodeAction>>();
            data._nodeActionDic = nodeActionDic;
            if (data.Actions != null)
            {
                var nodeActions = host.DeserializeFromString<NodeAction[]>(data.Actions);
                if (nodeActions != null)
                {
                    foreach (var nodeAction in nodeActions)
                    {
                        var action = host.NodeHost.Ontologies.GetAction(nodeAction.ActionId);
                        if (action == null)
                        {
                            throw new CoreException("意外的本体动作标识" + nodeAction.ActionId);
                        }
                        OntologyDescriptor ontology;
                        if (!host.NodeHost.Ontologies.TryGetOntology(action.OntologyId, out ontology))
                        {
                            throw new CoreException("意外的本体元素本体标识" + action.OntologyId);
                        }
                        if (!nodeActionDic.ContainsKey(ontology))
                        {
                            nodeActionDic.Add(ontology, new Dictionary<Verb, INodeAction>());
                        }
                        var actionDic = host.NodeHost.Ontologies.GetActons(ontology);
                        var verb = actionDic.Where(a => a.Value.Id == nodeAction.ActionId).Select(a => a.Key).FirstOrDefault();
                        if (verb == null)
                        {
                            throw new CoreException("意外的本体动作标识" + nodeAction.ActionId);
                        }
                        nodeActionDic[ontology].Add(verb, nodeAction);
                    }
                }
            }
            return data;
        }

        public string Name { get; private set; }

        public string Code { get; private set; }

        public string Actions { get; private set; }

        public IReadOnlyDictionary<OntologyDescriptor, Dictionary<Verb, INodeAction>> NodeActions
        {
            get { return _nodeActionDic; }
        }

        public string Abstract { get; private set; }

        public string Organization { get; private set; }

        public string Steward { get; private set; }

        public string Telephone { get; private set; }

        public string Email { get; private set; }

        public string Mobile { get; private set; }

        public string Qq { get; private set; }

        public string Icon { get; private set; }

        public int IsEnabled { get; private set; }

        public bool IsExecuteEnabled { get; private set; }

        public bool IsProduceEnabled { get; private set; }

        public bool IsReceiveEnabled { get; private set; }

        public bool IsDistributeEnabled { get; private set; }

        public Guid TransferId { get; private set; }

        public string AnycmdApiAddress { get; private set; }

        public string AnycmdWsAddress { get; private set; }

        public int? BeatPeriod { get; private set; }

        public string PublicKey { get; private set; }

        public string SecretKey { get; private set; }

        public int SortCode { get; private set; }

        public DateTime? CreateOn { get; private set; }

        protected override bool DoEquals(NodeState other)
        {
            return Id == other.Id &&
                Code == other.Code &&
                Name == other.Name &&
                Abstract == other.Abstract &&
                Organization == other.Organization &&
                Steward == other.Steward &&
                Telephone == other.Telephone &&
                Email == other.Email &&
                Mobile == other.Mobile &&
                Qq == other.Qq &&
                Icon == other.Icon &&
                IsEnabled == other.IsEnabled &&
                IsExecuteEnabled == other.IsExecuteEnabled &&
                IsProduceEnabled == other.IsProduceEnabled &&
                IsReceiveEnabled == other.IsReceiveEnabled &&
                IsDistributeEnabled == other.IsDistributeEnabled &&
                TransferId == other.TransferId &&
                AnycmdApiAddress == other.AnycmdApiAddress &&
                AnycmdWsAddress == other.AnycmdWsAddress &&
                BeatPeriod == other.BeatPeriod &&
                PublicKey == other.PublicKey &&
                SortCode == other.SortCode;
        }
    }
}
