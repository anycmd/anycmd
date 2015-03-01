
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Engine.Messages;
    using System;

    public sealed class UpdateNodeElementCareCommand : Command, IAnycmdCommand
    {
        public UpdateNodeElementCareCommand(IAcSession acSession, Guid nodeElementCareId, bool isInfoIdItem)
        {
            this.AcSession = acSession;
            this.NodeElementCareId = nodeElementCareId;
            this.IsInfoIdItem = isInfoIdItem;
        }

        public IAcSession AcSession { get; private set; }
        public Guid NodeElementCareId { get; private set; }
        public bool IsInfoIdItem { get; private set; }
    }
}
