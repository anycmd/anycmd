
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;
    using InOuts;

    public class DsdSetUpdatedEvent : DomainEvent
    {
        public DsdSetUpdatedEvent(DsdSetBase source, IDsdSetUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public IDsdSetUpdateIo Output { get; private set; }
    }
}