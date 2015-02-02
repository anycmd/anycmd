
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeElementCareUpdatedEvent : DomainEvent
    {
        public NodeElementCareUpdatedEvent(IAcSession acSession, NodeElementCareBase source)
            : base(acSession, source)
        {
            this.IsInfoIdItem = source.IsInfoIdItem;
        }

        public bool IsInfoIdItem { get; private set; }
    }
}
