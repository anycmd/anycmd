
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class ResourceTypeRemovingEvent: DomainEvent
    {
        public ResourceTypeRemovingEvent(IUserSession userSession, ResourceTypeBase source)
            : base(userSession, source)
        {
        }
    }
}