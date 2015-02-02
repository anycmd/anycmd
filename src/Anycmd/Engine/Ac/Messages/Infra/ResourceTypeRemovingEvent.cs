
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class ResourceTypeRemovingEvent: DomainEvent
    {
        public ResourceTypeRemovingEvent(IAcSession acSession, ResourceTypeBase source)
            : base(acSession, source)
        {
        }
    }
}