
namespace Anycmd.Engine.Ac.UiViews
{
    using Events;

    public sealed class MenuUpdatedEvent : DomainEvent
    {
        public MenuUpdatedEvent(IAcSession acSession, MenuBase source, IMenuUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        internal MenuUpdatedEvent(IAcSession acSession, MenuBase source, IMenuUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IMenuUpdateIo Input { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}