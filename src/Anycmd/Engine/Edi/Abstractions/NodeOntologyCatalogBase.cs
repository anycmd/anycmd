
namespace Anycmd.Engine.Edi.Abstractions
{
    using Model;
    using System;

    public class NodeOntologyCatalogBase : EntityBase, IAggregateRoot, INodeOntologyCatalog
    {
        public Guid NodeId { get; set; }

        public Guid OntologyId { get; set; }

        public Guid CatalogId { get; set; }

        public string Actions { get; set; }
    }
}
