
namespace Anycmd.Engine.Ac.Messages.Infra
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
