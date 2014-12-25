using Anycmd.Commands;
using System;

namespace Anycmd.Engine.Host.Edi.Messages
{
    public class ChangeProcessOrganizationCommand : Command, ISysCommand
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
