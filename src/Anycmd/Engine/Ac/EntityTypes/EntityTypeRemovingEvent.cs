
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Events;

    public sealed class EntityTypeRemovingEvent : DomainEvent
    {
        public EntityTypeRemovingEvent(IAcSession acSession, EntityTypeBase source)
            : base(acSession, source)
        {
        }
    }
}
