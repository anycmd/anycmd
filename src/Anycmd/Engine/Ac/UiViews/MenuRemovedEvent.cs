
namespace Anycmd.Engine.Ac.UiViews
{
    using UiViews;
    using Events;

    public class MenuRemovedEvent : DomainEvent
    {
        public MenuRemovedEvent(IAcSession acSession, MenuBase source)
            : base(acSession, source)
        {
        }
    }
}