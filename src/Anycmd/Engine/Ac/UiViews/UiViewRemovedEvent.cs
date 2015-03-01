
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class UiViewRemovedEvent : DomainEvent
    {
        public UiViewRemovedEvent(IAcSession acSession, UiViewBase source)
            : base(acSession, source)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
