
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveDsdSetCommand : RemoveEntityCommand
    {
        public RemoveDsdSetCommand(IAcSession acSession, Guid dsdSetId)
            : base(acSession, dsdSetId)
        {

        }
    }
}
