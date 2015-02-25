
namespace Anycmd.Engine.Ac.Dsd
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
