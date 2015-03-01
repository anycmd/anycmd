
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

        internal bool IsPrivate { get; set; }
    }
}