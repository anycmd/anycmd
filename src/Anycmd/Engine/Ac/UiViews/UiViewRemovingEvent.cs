
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    public sealed class UiViewRemovingEvent : DomainEvent
    {
        public UiViewRemovingEvent(IAcSession acSession, UiViewBase source)
            : base(acSession, source)
        {
        }
    }
}
