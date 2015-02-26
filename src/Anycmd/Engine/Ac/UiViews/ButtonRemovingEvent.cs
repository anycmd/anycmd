
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    public class ButtonRemovingEvent: DomainEvent
    {
        public ButtonRemovingEvent(IAcSession acSession, ButtonBase source)
            : base(acSession, source)
        {
        }
    }
}