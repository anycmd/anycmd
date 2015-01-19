
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine.Edi.InOuts;
    using System;

    public class NodeOntologyOrganizationCreateInput : INodeOntologyOrganizationCreateIo
    {
        public NodeOntologyOrganizationCreateInput()
        {
            this.HecpOntology = "NodeOntologyOrganization";
            this.HecpVerb = "Create";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid NodeId { get; set; }

        public Guid OntologyId { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid? Id { get; set; }
    }
}
