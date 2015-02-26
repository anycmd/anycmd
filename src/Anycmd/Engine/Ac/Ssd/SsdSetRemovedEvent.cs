
namespace Anycmd.Engine.Ac.Ssd
{
    using Events;

    public class SsdSetRemovedEvent : DomainEvent
    {
        public SsdSetRemovedEvent(IAcSession acSession, SsdSetBase source)
            : base(acSession, source)
        {
        }
    }
}