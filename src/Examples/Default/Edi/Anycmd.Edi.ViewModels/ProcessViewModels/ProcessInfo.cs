
namespace Anycmd.Edi.ViewModels.ProcessViewModels
{
    using Engine.Edi;
    using Exceptions;
    using Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessInfo : Dictionary<string, object>
    {
        private ProcessInfo() { }

        public ProcessInfo(DicReader dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }
            foreach (var item in dic)
            {
                this.Add(item.Key, item.Value);
            }
            OntologyDescriptor ontology;
            if (!dic.Host.NodeHost.Ontologies.TryGetOntology((Guid)this["OntologyId"], out ontology))
            {
                throw new AnycmdException("意外的本体标识" + this["OntologyId"]);
            }
            if (!this.ContainsKey("OntologyCode"))
            {
                this.Add("OntologyCode", ontology.Ontology.Code);
            }
            if (!this.ContainsKey("OntologyName"))
            {
                this.Add("OntologyName", ontology.Ontology.Name);
            }
            ProcessDescriptor process;
            if (!dic.Host.NodeHost.Processs.TryGetProcess((Guid)this["Id"], out process))
            {
                throw new AnycmdException("意外的进程标识" + this["Id"]);
            }
            if (!this.ContainsKey("WebApiBaseAddress"))
            {
                this.Add("WebApiBaseAddress", process.WebApiBaseAddress);
            }
        }
    }
}
