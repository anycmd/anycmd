
namespace Anycmd.Engine.Ac.EntityTypes
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
