
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class EntityTypeRemovingEvent: DomainEvent
    {
        public EntityTypeRemovingEvent(IAcSession userSession, EntityTypeBase source)
            : base(userSession, source)
        {
        }
    }
}
