
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Engine.Ac.Abstractions.Infra;
    using Events;

    public class OrganizationRemovingEvent: DomainEvent
    {
        public OrganizationRemovingEvent(OrganizationBase source)
            : base(source)
        {
        }
    }
}
