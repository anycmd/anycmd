
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Ac;
    using Exceptions;
    using Hecp;
    using Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class OntologyOrganizationState : StateObject<OntologyOrganizationState>, IOntologyOrganization, IStateObject
    {
        private Dictionary<Verb, IOrganizationAction> _orgActionDic;
        private readonly IAcDomain _host;

        private OntologyOrganizationState(IAcDomain host, Guid id)
            : base(id)
        {
            this._host = host;
        }

        public static OntologyOrganizationState Create(IAcDomain host, IOntologyOrganization ontologyOrganization)
        {
            if (ontologyOrganization == null)
            {
                throw new ArgumentNullException("ontologyOrganization");
            }
            var data = new OntologyOrganizationState(host, ontologyOrganization.Id)
            {
                Actions = ontologyOrganization.Actions,
                OntologyId = ontologyOrganization.OntologyId,
                OrganizationId = ontologyOrganization.OrganizationId
            };
            var orgActionDic = new Dictionary<Verb, IOrganizationAction>();
            data._orgActionDic = orgActionDic;
            if (data.Actions != null)
            {
                var orgActions = host.JsonSerializer.Deserialize<OrganizationAction[]>(data.Actions);
                if (orgActions != null)
                {
                    foreach (var orgAction in orgActions)
                    {
                        var action = host.NodeHost.Ontologies.GetAction(orgAction.ActionId);
                        if (action == null)
                        {
                            throw new AnycmdException("意外的组织结构动作标识" + orgAction.ActionId);
                        }
                        OntologyDescriptor ontology;
                        if (!host.NodeHost.Ontologies.TryGetOntology(action.OntologyId, out ontology))
                        {
                            throw new AnycmdException("意外的本体元素本体标识" + action.OntologyId);
                        }
                        OrganizationState org;
                        if (!host.OrganizationSet.TryGetOrganization(orgAction.OrganizationId, out org))
                        {
                            throw new AnycmdException("意外的组织结构动作组织结构标识" + orgAction.OrganizationId);
                        }
                        var actionDic = host.NodeHost.Ontologies.GetActons(ontology);
                        var verb = actionDic.Where(a => a.Value.Id == orgAction.ActionId).Select(a => a.Key).FirstOrDefault();
                        if (verb == null)
                        {
                            throw new AnycmdException("意外的本体动作标识" + orgAction.ActionId);
                        }
                        orgActionDic.Add(verb, orgAction);
                    }
                }
            }
            return data;
        }

        public Guid OntologyId { get; private set; }

        public Guid OrganizationId { get; private set; }

        public string Actions { get; private set; }

        public IReadOnlyDictionary<Verb, IOrganizationAction> OrganizationActions
        {
            get { return _orgActionDic; }
        }

        protected override bool DoEquals(OntologyOrganizationState other)
        {
            return
                Id == other.Id &&
                OntologyId == other.OntologyId &&
                OrganizationId == other.OrganizationId;
        }
    }
}
