
namespace Anycmd.Engine.Ac.Ssd
{
    using Events;

    public sealed class SsdSetUpdatedEvent : DomainEvent
    {
        public SsdSetUpdatedEvent(IAcSession acSession, SsdSetBase source, ISsdSetUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal SsdSetUpdatedEvent(IAcSession acSession, SsdSetBase source, ISsdSetUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public ISsdSetUpdateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}