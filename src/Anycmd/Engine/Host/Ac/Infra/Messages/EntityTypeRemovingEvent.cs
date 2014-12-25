
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class EntityTypeRemovingEvent: DomainEvent
    {
        public EntityTypeRemovingEvent(EntityTypeBase source)
            : base(source)
        {
        }
    }
}
