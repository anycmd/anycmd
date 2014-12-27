
namespace Anycmd.Edi.ViewModels.BatchViewModels
{
    using Engine.Edi;
    using Exceptions;
    using Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public partial class BatchTr : Dictionary<string, object>
    {
        public BatchTr() { }

        public BatchTr(DicReader dic)
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
            NodeDescriptor node;
            if (!dic.Host.NodeHost.Nodes.TryGetNodeById(this["NodeId"].ToString(), out node))
            {
                throw new AnycmdException("意外的节点标识" + this["NodeId"]);
            }
            if (!this.ContainsKey("OntologyCode"))
            {
                this.Add("OntologyCode", ontology.Ontology.Code);
            }
            if (!this.ContainsKey("OntologyName"))
            {
                this.Add("OntologyName", ontology.Ontology.Name);
            }
            if (!this.ContainsKey("NodeCode"))
            {
                this.Add("NodeCode", node.Node.Code);
            }
            if (!this.ContainsKey("NodeName"))
            {
                this.Add("NodeName", node.Node.Name);
            }
        }
    }
}
