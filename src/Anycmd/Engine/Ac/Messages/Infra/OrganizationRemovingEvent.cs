
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class OrganizationRemovingEvent: DomainEvent
    {
        public OrganizationRemovingEvent(OrganizationBase source)
            : base(source)
        {
        }
    }
}
