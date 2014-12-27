
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Events;
    using InOuts;

    public class MenuUpdatedEvent : DomainEvent
    {
        public MenuUpdatedEvent(MenuBase source, IMenuUpdateIo input)
            : base(source)
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