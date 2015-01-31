
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using System;
    using System.Collections.Generic;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public partial class OntologyInfo : Dictionary<string, object>
    {
        public OntologyInfo() { }

        public OntologyInfo(IAcDomain host, Dictionary<string, object> dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }
            foreach (var item in dic)
            {
                this.Add(item.Key, item.Value);
            }
            if (!this.ContainsKey("DeletionStateName"))
            {
                this.Add("DeletionStateName", host.Translate("Edi", "Ontology", "DeletionStateName", (int)this["DeletionStateCode"]));
            }
            if (!this.ContainsKey("IsEnabledName"))
            {
                this.Add("IsEnabledName", host.Translate("Edi", "Ontology", "IsEnabledName", (int)this["IsEnabled"]));
            }
            if (!this.ContainsKey("IsCataloguedEntityName"))
            {
                this.Add("IsCataloguedEntityName", host.Translate("Edi", "Ontology", "IsCataloguedEntityName", (bool)this["IsCataloguedEntity"]));
            }
            if (!this.ContainsKey("IsLogicalDeletionEntityName"))
            {
                this.Add("IsLogicalDeletionEntityName", host.Translate("Edi", "Ontology", "IsLogicalDeletionEntityName", (bool)this["IsLogicalDeletionEntity"]));
            }
        }
    }
}
