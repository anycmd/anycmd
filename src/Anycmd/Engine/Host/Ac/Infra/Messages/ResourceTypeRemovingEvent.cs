
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class ResourceTypeRemovingEvent: DomainEvent
    {
        public ResourceTypeRemovingEvent(ResourceTypeBase source)
            : base(source)
        {
        }
    }
}