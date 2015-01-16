
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    public class PositionUpdatedEvent : DomainEvent
    {
        public PositionUpdatedEvent(GroupBase source, IPositionUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IPositionUpdateIo Output { get; private set; }
    }
}