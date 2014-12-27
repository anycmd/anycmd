
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveOrganizationCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveOrganizationCommand(Guid organizationId)
            : base(organizationId)
        {

        }
    }
}
