
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class ButtonRemovingEvent: DomainEvent
    {
        public ButtonRemovingEvent(IAcSession acSession, ButtonBase source)
            : base(acSession, source)
        {
        }
    }
}