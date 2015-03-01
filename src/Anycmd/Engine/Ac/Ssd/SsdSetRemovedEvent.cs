
namespace Anycmd.Engine.Ac.Ssd
{
    using Events;

    public sealed class SsdSetRemovedEvent : DomainEvent
    {
        public SsdSetRemovedEvent(IAcSession acSession, SsdSetBase source)
            : base(acSession, source)
        {
        }

        internal SsdSetRemovedEvent(IAcSession acSession, SsdSetBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}