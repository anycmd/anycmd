
namespace Anycmd.Engine.Edi.Abstractions
{
    using Model;
    using System;

    public class NodeOntologyOrganizationBase : EntityBase, IAggregateRoot, INodeOntologyOrganization
    {
        public Guid NodeId { get; set; }

        public Guid OntologyId { get; set; }

        public Guid OrganizationId { get; set; }

        public string Actions { get; set; }
    }
}
