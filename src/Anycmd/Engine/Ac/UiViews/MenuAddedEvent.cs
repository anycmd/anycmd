
namespace Anycmd.Engine.Ac.UiViews
{
    using Messages;

    public sealed class MenuAddedEvent : EntityAddedEvent<IMenuCreateIo>
    {
        public MenuAddedEvent(IAcSession acSession, MenuBase source, IMenuCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal MenuAddedEvent(IAcSession acSession, MenuBase source, IMenuCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}