
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveOrganizationActionCommand : RemoveEntityCommand
    {
        public RemoveOrganizationActionCommand(IUserSession userSession, Guid ontologyOrganizationActionId)
            : base(userSession, ontologyOrganizationActionId)
        {

        }
    }
}
