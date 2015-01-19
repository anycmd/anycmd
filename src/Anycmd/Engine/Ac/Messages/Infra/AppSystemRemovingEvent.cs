
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class AppSystemRemovingEvent: DomainEvent
    {
        public AppSystemRemovingEvent(IUserSession userSession, AppSystemBase source)
            : base(userSession, source)
        {
        }
    }
}
