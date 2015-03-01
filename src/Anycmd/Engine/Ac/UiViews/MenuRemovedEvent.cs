
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    public sealed class MenuRemovedEvent : DomainEvent
    {
        public MenuRemovedEvent(IAcSession acSession, MenuBase source)
            : base(acSession, source)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}