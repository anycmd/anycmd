
namespace Anycmd.Engine.Ac.UiViews
{
    using UiViews;
    using Events;
    using InOuts;

    public class MenuUpdatedEvent : DomainEvent
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

        public IMenuUpdateIo Input { get; private set; }
    }
}