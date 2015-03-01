
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

        internal bool IsPrivate { get; set; }
    }
}
