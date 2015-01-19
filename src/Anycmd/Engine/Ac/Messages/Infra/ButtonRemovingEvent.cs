
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class ButtonRemovingEvent: DomainEvent
    {
        public ButtonRemovingEvent(IUserSession userSession, ButtonBase source)
            : base(userSession, source)
        {
        }
    }
}