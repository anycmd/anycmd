
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using System;

    public class AddCommonPropertiesCommand : Command, IAnycmdCommand
    {
        public AddCommonPropertiesCommand(IUserSession userSession, Guid entityTypeId)
        {
            this.UserSession = userSession;
            this.EntityTypeId = entityTypeId;
        }

        public IUserSession UserSession { get; private set; }

        public Guid EntityTypeId { get; private set; }
    }
}
