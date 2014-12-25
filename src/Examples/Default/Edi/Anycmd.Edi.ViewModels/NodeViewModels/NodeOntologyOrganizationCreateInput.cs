
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine.Host.Edi.InOuts;
    using System;

    public class NodeOntologyOrganizationCreateInput : INodeOntologyOrganizationCreateIo
    {
        public Guid NodeId { get; set; }

        public Guid OntologyId { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid? Id { get; set; }
    }
}
