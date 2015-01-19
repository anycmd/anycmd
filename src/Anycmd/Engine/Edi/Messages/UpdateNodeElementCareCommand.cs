
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class UpdateNodeElementCareCommand : Command, IAnycmdCommand
    {
        public UpdateNodeElementCareCommand(IUserSession userSession, Guid nodeElementCareId, bool isInfoIdItem)
        {
            this.UserSession = userSession;
            this.NodeElementCareId = nodeElementCareId;
            this.IsInfoIdItem = isInfoIdItem;
        }

        public IUserSession UserSession { get; private set; }
        public Guid NodeElementCareId { get; private set; }
        public bool IsInfoIdItem { get; private set; }
    }
}
