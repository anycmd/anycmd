
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Commands;
    using Messages;
    using System;

    public sealed class AddCommonPropertiesCommand : Command, IAnycmdCommand
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
