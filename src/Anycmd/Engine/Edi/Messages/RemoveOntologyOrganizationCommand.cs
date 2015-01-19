
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class RemoveOntologyOrganizationCommand : Command, IAnycmdCommand
    {
        public RemoveOntologyOrganizationCommand(IUserSession userSession, Guid ontologyId, Guid organizationId)
        {
            this.UserSession = userSession;
            this.OntologyId = ontologyId;
            this.OrganizationId = organizationId;
        }

        public IUserSession UserSession { get; private set; }

        public Guid OntologyId { get; private set; }

        public Guid OrganizationId { get; private set; }
    }
}
