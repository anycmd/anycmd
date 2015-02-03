
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

    public sealed class OntologyCatalogState : StateObject<OntologyCatalogState>, IOntologyCatalog
    {
        private Dictionary<Verb, ICatalogAction> _orgActionDic;
        private readonly IAcDomain _acDomain;

        private OntologyCatalogState(IAcDomain acDomain, Guid id)
            : base(id)
        {
            this._acDomain = acDomain;
        }

        public static OntologyCatalogState Create(IAcDomain acDomain, IOntologyCatalog ontologyCatalog)
        {
            if (ontologyCatalog == null)
            {
                throw new ArgumentNullException("ontologyCatalog");
            }
            var data = new OntologyCatalogState(acDomain, ontologyCatalog.Id)
            {
                Actions = ontologyCatalog.Actions,
                OntologyId = ontologyCatalog.OntologyId,
                CatalogId = ontologyCatalog.CatalogId
            };
            var orgActionDic = new Dictionary<Verb, ICatalogAction>();
            data._orgActionDic = orgActionDic;
            if (data.Actions != null)
            {
                var orgActions = acDomain.JsonSerializer.Deserialize<CatalogAction[]>(data.Actions);
                if (orgActions != null)
                {
                    foreach (var orgAction in orgActions)
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
                        orgActionDic.Add(verb, orgAction);
                    }
                }
            }
            return data;
        }

        public Guid OntologyId { get; private set; }

        public Guid CatalogId { get; private set; }

        public string Actions { get; private set; }

        public IReadOnlyDictionary<Verb, ICatalogAction> CatalogActions
        {
            get { return _orgActionDic; }
        }

        protected override bool DoEquals(OntologyCatalogState other)
        {
            return
                Id == other.Id &&
                OntologyId == other.OntologyId &&
                CatalogId == other.CatalogId;
        }
    }
}
