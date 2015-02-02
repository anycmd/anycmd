
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using System;

    public class AddCommonPropertiesCommand : Command, IAnycmdCommand
    {
        public AddCommonPropertiesCommand(IAcSession userSession, Guid entityTypeId)
        {
            this.AcSession = userSession;
            this.EntityTypeId = entityTypeId;
        }

        public IAcSession AcSession { get; private set; }

        public Guid EntityTypeId { get; private set; }
    }
}
