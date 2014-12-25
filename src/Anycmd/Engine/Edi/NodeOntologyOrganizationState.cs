
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Engine.Ac;
    using Exceptions;
    using Host.Hecp;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class NodeOntologyOrganizationState : StateObject<NodeOntologyOrganizationState>, INodeOntologyOrganization, IStateObject
    {
        private Dictionary<Verb, INodeOrganizationAction> _nodeOrgActionDic;
        private readonly IAcDomain _host;

        private NodeOntologyOrganizationState(IAcDomain host, Guid id) : base(id)
        {
            this._host = host;
        }

        public static NodeOntologyOrganizationState Create(IAcDomain host, INodeOntologyOrganization nodeOntologyOrg)
        {
            if (nodeOntologyOrg == null)
            {
                throw new ArgumentNullException("nodeOntologyOrg");
            }
            var data = new NodeOntologyOrganizationState(host, nodeOntologyOrg.Id)
            {
                Actions = nodeOntologyOrg.Actions,
                NodeId = nodeOntologyOrg.NodeId,
                OntologyId = nodeOntologyOrg.OntologyId,
                OrganizationId = nodeOntologyOrg.OrganizationId
            };
            var nodeOrgActionDic = new Dictionary<Verb, INodeOrganizationAction>();
            data._nodeOrgActionDic = nodeOrgActionDic;
            if (data.Actions != null)
            {
                var nodeOrgActions = host.DeserializeFromString<NodeOrganizationAction[]>(data.Actions);
                if (nodeOrgActions != null)
                {
                    foreach (var orgAction in nodeOrgActions)
                    {
                        var action = host.NodeHost.Ontologies.GetAction(orgAction.ActionId);
                        if (action == null)
                        {
                            throw new CoreException("意外的组织结构动作标识" + orgAction.ActionId);
                        }
                        OntologyDescriptor ontology;
                        if (!host.NodeHost.Ontologies.TryGetOntology(action.OntologyId, out ontology))
                        {
                            throw new CoreException("意外的本体元素本体标识" + action.OntologyId);
                        }
                        OrganizationState org;
                        if (!host.OrganizationSet.TryGetOrganization(orgAction.OrganizationId, out org))
                        {
                            throw new CoreException("意外的组织结构动作组织结构标识" + orgAction.OrganizationId);
                        }
                        var actionDic = host.NodeHost.Ontologies.GetActons(ontology);
                        var verb = actionDic.Where(a => a.Value.Id == orgAction.ActionId).Select(a => a.Key).FirstOrDefault();
                        if (verb == null)
                        {
                            throw new CoreException("意外的本体动作标识" + orgAction.ActionId);
                        }
                        nodeOrgActionDic.Add(verb, orgAction);
                    }
                }
            }
            return data;
        }

        public Guid NodeId { get; private set; }

        public Guid OntologyId { get; private set; }

        public Guid OrganizationId { get; private set; }

        public string Actions { get; private set; }

        public IReadOnlyDictionary<Verb, INodeOrganizationAction> NodeOrganizationActions
        {
            get { return _nodeOrgActionDic; }
        }

        protected override bool DoEquals(NodeOntologyOrganizationState other)
        {
            return
                Id == other.Id &&
                NodeId == other.NodeId &&
                OntologyId == other.OntologyId &&
                OrganizationId == other.OrganizationId;
        }
    }
}
