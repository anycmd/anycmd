
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class RemoveNodeOntologyOrganizationCommand : Command, ISysCommand
    {
        public RemoveNodeOntologyOrganizationCommand(Guid nodeId, Guid ontologyId, Guid organizationId)
        {
            this.NodeId = nodeId;
            this.OntologyId = ontologyId;
            this.OrganizationId = organizationId;
        }
        
        public Guid NodeId { get; private set; }
        public Guid OntologyId { get; private set; }
        public Guid OrganizationId { get; private set; }
    }
}
