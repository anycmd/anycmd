
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public sealed class SsdSetAddedEvent : EntityAddedEvent<ISsdSetCreateIo>
    {
        public SsdSetAddedEvent(IAcSession acSession, SsdSetBase source, ISsdSetCreateIo output)
            : base(acSession, source, output)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
