
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class UpdateNodeElementCareCommand : Command, IAnycmdCommand
    {
        public UpdateNodeElementCareCommand(Guid nodeElementCareId, bool isInfoIdItem)
        {
            this.NodeElementCareId = nodeElementCareId;
            this.IsInfoIdItem = isInfoIdItem;
        }

        public Guid NodeElementCareId { get; private set; }
        public bool IsInfoIdItem { get; private set; }
    }
}
