
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class MenuRemovingEvent: DomainEvent
    {
        public MenuRemovingEvent(IAcSession userSession, MenuBase source)
            : base(userSession, source)
        {
        }
    }
}