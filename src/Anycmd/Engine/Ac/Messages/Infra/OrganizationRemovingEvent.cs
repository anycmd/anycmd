
namespace Anycmd.Engine.Ac.Messages.Infra
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
