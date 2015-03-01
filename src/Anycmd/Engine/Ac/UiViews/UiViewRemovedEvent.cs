
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

        internal UiViewRemovedEvent(IAcSession acSession, UiViewBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
