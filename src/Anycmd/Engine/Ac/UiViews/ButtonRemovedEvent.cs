
namespace Anycmd.Engine.Ac.UiViews
{
    using UiViews;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class ButtonRemovedEvent : DomainEvent
    {
        public ButtonRemovedEvent(IAcSession acSession, ButtonBase source)
            : base(acSession, source)
        {
        }
    }
}