
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;
    using InOuts;

    public class SsdSetUpdatedEvent: DomainEvent
    {
        public SsdSetUpdatedEvent(SsdSetBase source, ISsdSetUpdateIo output)
            : base(source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        public ISsdSetUpdateIo Output { get; private set; }
    }
}