
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class ChangeProcessOrganizationCommand : Command, IAnycmdCommand
    {
        public ChangeProcessOrganizationCommand(IUserSession userSession, Guid processId, string organizationCode)
        {
            this.UserSession = userSession;
            this.ProcessId = processId;
            this.OrganizationCode = organizationCode;
        }

        public IUserSession UserSession { get; private set; }

        public Guid ProcessId { get; private set; }
        public string OrganizationCode { get; private set; }
    }
}
