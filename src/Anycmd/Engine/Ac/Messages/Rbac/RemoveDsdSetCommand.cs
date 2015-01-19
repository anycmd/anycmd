
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveDsdSetCommand : RemoveEntityCommand
    {
        public RemoveDsdSetCommand(IUserSession userSession, Guid dsdSetId)
            : base(userSession, dsdSetId)
        {

        }
    }
}
