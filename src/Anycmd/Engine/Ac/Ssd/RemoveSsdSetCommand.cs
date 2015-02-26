
namespace Anycmd.Engine.Ac.Ssd
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
