
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class ChangeProcessOrganizationCommand : Command, IAnycmdCommand
    {
        public ChangeProcessOrganizationCommand(Guid processId, string organizationCode)
        {
            this.ProcessId = processId;
            this.OrganizationCode = organizationCode;
        }

        public Guid ProcessId { get; private set; }
        public string OrganizationCode { get; private set; }
    }
}
