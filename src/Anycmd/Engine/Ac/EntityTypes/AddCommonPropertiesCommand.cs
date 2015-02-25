
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using System;

    public class AddCommonPropertiesCommand : Command, IAnycmdCommand
    {
        public AddCommonPropertiesCommand(IAcSession acSession, Guid entityTypeId)
        {
            this.AcSession = acSession;
            this.EntityTypeId = entityTypeId;
        }

        public IAcSession AcSession { get; private set; }

        public Guid EntityTypeId { get; private set; }
    }
}
