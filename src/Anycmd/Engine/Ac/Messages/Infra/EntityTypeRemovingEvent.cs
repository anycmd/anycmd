
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class EntityTypeRemovingEvent: DomainEvent
    {
        public EntityTypeRemovingEvent(EntityTypeBase source)
            : base(source)
        {
        }
    }
}
