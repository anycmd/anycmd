
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveDsdSetCommand : RemoveEntityCommand
    {
        public RemoveDsdSetCommand(IAcSession userSession, Guid dsdSetId)
            : base(userSession, dsdSetId)
        {

        }
    }
}
