
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    public class MenuUpdatedEvent : DomainEvent
    {
        public MenuUpdatedEvent(IUserSession userSession, MenuBase source, IMenuUpdateIo input)
            : base(userSession, source)
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