
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class EntityTypeRemovingEvent: DomainEvent
    {
        public EntityTypeRemovingEvent(IUserSession userSession, EntityTypeBase source)
            : base(userSession, source)
        {
        }
    }
}
