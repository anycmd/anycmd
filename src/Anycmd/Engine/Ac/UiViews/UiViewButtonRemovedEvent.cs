
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class UiViewButtonRemovedEvent : DomainEvent
    {
        public UiViewButtonRemovedEvent(IAcSession acSession, UiViewButtonBase source)
            : base(acSession, source)
        {
        }

        internal UiViewButtonRemovedEvent(IAcSession acSession, UiViewButtonBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
