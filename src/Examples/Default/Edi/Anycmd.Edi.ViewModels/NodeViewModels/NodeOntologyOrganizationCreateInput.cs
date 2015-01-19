
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine.Edi.InOuts;
    using System;

    public class NodeOntologyOrganizationCreateInput : INodeOntologyOrganizationCreateIo
    {
        public NodeOntologyOrganizationCreateInput()
        {
            this.OntologyCode = "NodeOntologyOrganization";
            this.Verb = "Create";
        }

        public string OntologyCode { get; private set; }

        public string Verb { get; private set; }

        public Guid NodeId { get; set; }

        public Guid OntologyId { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid? Id { get; set; }
    }
}
