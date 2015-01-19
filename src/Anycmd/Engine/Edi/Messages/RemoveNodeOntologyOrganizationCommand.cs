
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class RemoveNodeOntologyOrganizationCommand : Command, IAnycmdCommand
    {
        public RemoveNodeOntologyOrganizationCommand(IUserSession userSession, Guid nodeId, Guid ontologyId, Guid organizationId)
        {
            this.UserSession = userSession;
            this.NodeId = nodeId;
            this.OntologyId = ontologyId;
            this.OrganizationId = organizationId;
        }

        public IUserSession UserSession { get; private set; }
        public Guid NodeId { get; private set; }
        public Guid OntologyId { get; private set; }
        public Guid OrganizationId { get; private set; }
    }
}
