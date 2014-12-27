
namespace Anycmd.Engine.Ac.Messages.Infra
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