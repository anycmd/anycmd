
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class RemoveOntologyOrganizationCommand : Command, ISysCommand
    {
        public RemoveOntologyOrganizationCommand(Guid ontologyId, Guid organizationId)
        {
            this.OntologyId = ontologyId;
            this.OrganizationId = organizationId;
        }

        public Guid OntologyId { get; private set; }

        public Guid OrganizationId { get; private set; }
    }
}
