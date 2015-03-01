
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;
    using System;

    public sealed class RemoveSsdSetCommand : RemoveEntityCommand
    {
        public RemoveSsdSetCommand(IAcSession acSession, Guid ssdSetId)
            : base(acSession, ssdSetId)
        {

        }
    }
}
