
namespace Anycmd.Engine.Ac.Dsd
{
    using Events;

    public sealed class DsdSetRemovedEvent : DomainEvent
    {
        public DsdSetRemovedEvent(IAcSession acSession, DsdSetBase source)
            : base(acSession, source)
        {
        }
        internal bool IsPrivate { get; set; }
    }
}