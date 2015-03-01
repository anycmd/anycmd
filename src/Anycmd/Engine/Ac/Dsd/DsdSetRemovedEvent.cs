
namespace Anycmd.Engine.Ac.Dsd
{
    using Events;

    public sealed class DsdSetRemovedEvent : DomainEvent
    {
        public DsdSetRemovedEvent(IAcSession acSession, DsdSetBase source)
            : base(acSession, source)
        {
        }

        internal DsdSetRemovedEvent(IAcSession acSession, DsdSetBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}