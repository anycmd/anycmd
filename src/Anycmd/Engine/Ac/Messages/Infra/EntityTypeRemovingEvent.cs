
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class EntityTypeRemovingEvent: DomainEvent
    {
        public EntityTypeRemovingEvent(IAcSession acSession, EntityTypeBase source)
            : base(acSession, source)
        {
        }
    }
}
