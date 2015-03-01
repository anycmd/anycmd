
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    public sealed class MenuRemovingEvent : DomainEvent
    {
        public MenuRemovingEvent(IAcSession acSession, MenuBase source)
            : base(acSession, source)
        {
        }
    }
}