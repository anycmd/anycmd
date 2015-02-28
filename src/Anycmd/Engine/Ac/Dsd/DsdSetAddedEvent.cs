
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;

    public sealed class DsdSetAddedEvent : EntityAddedEvent<IDsdSetCreateIo>
    {
        public DsdSetAddedEvent(IAcSession acSession, DsdSetBase source, IDsdSetCreateIo output)
            : base(acSession, source, output)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
