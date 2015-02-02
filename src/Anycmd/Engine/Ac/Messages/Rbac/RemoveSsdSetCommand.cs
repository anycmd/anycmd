
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveSsdSetCommand: RemoveEntityCommand
    {
        public RemoveSsdSetCommand(IAcSession userSession, Guid ssdSetId)
            : base(userSession, ssdSetId)
        {

        }
    }
}
