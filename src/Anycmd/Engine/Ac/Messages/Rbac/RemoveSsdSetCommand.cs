
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveSsdSetCommand: RemoveEntityCommand
    {
        public RemoveSsdSetCommand(IAcSession acSession, Guid ssdSetId)
            : base(acSession, ssdSetId)
        {

        }
    }
}
