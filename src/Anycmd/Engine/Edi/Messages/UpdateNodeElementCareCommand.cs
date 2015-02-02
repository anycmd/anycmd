
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class UpdateNodeElementCareCommand : Command, IAnycmdCommand
    {
        public UpdateNodeElementCareCommand(IAcSession userSession, Guid nodeElementCareId, bool isInfoIdItem)
        {
            this.AcSession = userSession;
            this.NodeElementCareId = nodeElementCareId;
            this.IsInfoIdItem = isInfoIdItem;
        }

        public IAcSession AcSession { get; private set; }
        public Guid NodeElementCareId { get; private set; }
        public bool IsInfoIdItem { get; private set; }
    }
}
