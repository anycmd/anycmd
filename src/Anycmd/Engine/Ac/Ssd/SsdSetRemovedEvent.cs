
namespace Anycmd.Engine.Ac.Ssd
{
    using Events;

    public sealed class SsdSetRemovedEvent : DomainEvent
    {
        public SsdSetRemovedEvent(IAcSession acSession, SsdSetBase source)
            : base(acSession, source)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}