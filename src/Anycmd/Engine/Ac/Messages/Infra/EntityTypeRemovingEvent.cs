
namespace Anycmd.Engine.Ac.Messages.Infra
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
