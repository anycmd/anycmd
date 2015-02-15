
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Ac;
    using Exceptions;
    using Hecp;
    using Model;
    using Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class NodeOntologyCatalogState : StateObject<NodeOntologyCatalogState>, INodeOntologyCatalog
    {
        private Dictionary<Verb, INodeCatalogAction> _nodeOrgActionDic;
        private readonly IAcDomain _acDomain;

        private NodeOntologyCatalogState(IAcDomain acDomain, Guid id) : base(id)
        {
            this._acDomain = acDomain;
        }

        public static NodeOntologyCatalogState Create(IAcDomain acDomain, INodeOntologyCatalog nodeOntologyOrg)
        {
            if (nodeOntologyOrg == null)
            {
                throw new ArgumentNullException("nodeOntologyOrg");
            }
            var data = new NodeOntologyCatalogState(acDomain, nodeOntologyOrg.Id)
            {
                Actions = nodeOntologyOrg.Actions,
                NodeId = nodeOntologyOrg.NodeId,
                OntologyId = nodeOntologyOrg.OntologyId,
                CatalogId = nodeOntologyOrg.CatalogId
            };
            var nodeOrgActionDic = new Dictionary<Verb, INodeCatalogAction>();
            data._nodeOrgActionDic = nodeOrgActionDic;
            if (data.Actions != null)
            {
                var nodeOrgActions = acDomain.JsonSerializer.Deserialize<NodeCatalogAction[]>(data.Actions);
                if (nodeOrgActions != null)
                {
                    foreach (var orgAction in nodeOrgActions)
                    {
                        var action = acDomain.NodeHost.Ontologies.GetAction(orgAction.ActionId);
                        if (action == null)
                        {
                            throw new AnycmdException("意外的目录动作标识" + orgAction.ActionId);
                        }
                        OntologyDescriptor ontology;
                        if (!acDomain.NodeHost.Ontologies.TryGetOntology(action.OntologyId, out ontology))
                        {
                            throw new AnycmdException("意外的本体元素本体标识" + action.OntologyId);
                        }
                        CatalogState org;
                        if (!acDomain.CatalogSet.TryGetCatalog(orgAction.CatalogId, out org))
                        {
                            throw new AnycmdException("意外的目录动作目录标识" + orgAction.CatalogId);
                        }
                        var actionDic = acDomain.NodeHost.Ontologies.GetActons(ontology);
                        var verb = actionDic.Where(a => a.Value.Id == orgAction.ActionId).Select(a => a.Key).FirstOrDefault();
                        if (verb == null)
                        {
                            throw new AnycmdException("意外的本体动作标识" + orgAction.ActionId);
                        }
                        nodeOrgActionDic.Add(verb, orgAction);
                    }
                }
            }
            return data;
        }

        public Guid NodeId { get; private set; }

        public Guid OntologyId { get; private set; }

        public Guid CatalogId { get; private set; }

        public string Actions { get; private set; }

        public IReadOnlyDictionary<Verb, INodeCatalogAction> NodeCatalogActions
        {
            get { return _nodeOrgActionDic; }
        }

        protected override bool DoEquals(NodeOntologyCatalogState other)
        {
            return
                Id == other.Id &&
                NodeId == other.NodeId &&
                OntologyId == other.OntologyId &&
                CatalogId == other.CatalogId;
        }
    }
}
