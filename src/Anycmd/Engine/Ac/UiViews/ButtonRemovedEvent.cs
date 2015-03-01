
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ButtonRemovedEvent : DomainEvent
    {
        public ButtonRemovedEvent(IAcSession acSession, ButtonBase source)
            : base(acSession, source)
        {
        }

        internal ButtonRemovedEvent(IAcSession acSession, ButtonBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}