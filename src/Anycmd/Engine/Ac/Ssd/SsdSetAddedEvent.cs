
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public sealed class SsdSetAddedEvent : EntityAddedEvent<ISsdSetCreateIo>
    {
        public SsdSetAddedEvent(IAcSession acSession, SsdSetBase source, ISsdSetCreateIo output)
            : base(acSession, source, output)
        {
        }

        internal SsdSetAddedEvent(IAcSession acSession, SsdSetBase source, ISsdSetCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
