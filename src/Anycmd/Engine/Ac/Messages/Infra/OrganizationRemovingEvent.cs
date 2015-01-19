
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class OrganizationRemovingEvent: DomainEvent
    {
        public OrganizationRemovingEvent(IUserSession userSession, OrganizationBase source)
            : base(userSession, source)
        {
        }
    }
}
