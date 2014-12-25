
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveOrganizationCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveOrganizationCommand(Guid organizationId)
            : base(organizationId)
        {

        }
    }
}
