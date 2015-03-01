
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    public sealed class ButtonRemovingEvent : DomainEvent
    {
        public ButtonRemovingEvent(IAcSession acSession, ButtonBase source)
            : base(acSession, source)
        {
        }
    }
}