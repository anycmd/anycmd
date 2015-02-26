
namespace Anycmd.Engine.Ac.UiViews
{
    using UiViews;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonRemovedEvent : DomainEvent
    {
        public UiViewButtonRemovedEvent(IAcSession acSession, UiViewButtonBase source)
            : base(acSession, source)
        {
        }
    }
}
