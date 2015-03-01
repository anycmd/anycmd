
namespace Anycmd.Engine.Ac.Dsd
{
    using Events;

    public sealed class DsdSetUpdatedEvent : DomainEvent
    {
        public DsdSetUpdatedEvent(IAcSession acSession, DsdSetBase source, IDsdSetUpdateIo output)
            : base(acSession, source)
        {
            if (output == null)
            {
                throw new System.ArgumentNullException("output");
            }
            this.Output = output;
        }

        internal DsdSetUpdatedEvent(IAcSession acSession, DsdSetBase source, IDsdSetUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public IDsdSetUpdateIo Output { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}