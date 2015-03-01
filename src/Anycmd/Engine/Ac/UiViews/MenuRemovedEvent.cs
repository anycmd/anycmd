
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    public sealed class MenuRemovedEvent : DomainEvent
    {
        public MenuRemovedEvent(IAcSession acSession, MenuBase source)
            : base(acSession, source)
        {
        }

        internal MenuRemovedEvent(IAcSession acSession, MenuBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}