
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveOrganizationCommand : RemoveEntityCommand
    {
        public RemoveOrganizationCommand(IUserSession userSession, Guid organizationId)
            : base(userSession, organizationId)
        {

        }
    }
}
