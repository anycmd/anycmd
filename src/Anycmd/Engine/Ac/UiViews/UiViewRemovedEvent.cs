
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewRemovedEvent : DomainEvent
    {
        public UiViewRemovedEvent(IAcSession acSession, UiViewBase source)
            : base(acSession, source)
        {
        }
    }
}
