
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class ButtonRemovingEvent: DomainEvent
    {
        public ButtonRemovingEvent(IAcSession userSession, ButtonBase source)
            : base(userSession, source)
        {
        }
    }
}