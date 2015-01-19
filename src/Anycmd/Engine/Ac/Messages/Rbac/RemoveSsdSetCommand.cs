
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveSsdSetCommand: RemoveEntityCommand
    {
        public RemoveSsdSetCommand(IUserSession userSession, Guid ssdSetId)
            : base(userSession, ssdSetId)
        {

        }
    }
}
