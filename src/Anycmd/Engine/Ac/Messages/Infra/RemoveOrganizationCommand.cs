
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveOrganizationCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveOrganizationCommand(Guid organizationId)
            : base(organizationId)
        {

        }
    }
}
