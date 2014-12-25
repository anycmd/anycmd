
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class NodeElementCareUpdatedEvent : DomainEvent
    {
        public NodeElementCareUpdatedEvent(NodeElementCareBase source)
            : base(source)
        {
            this.IsInfoIdItem = source.IsInfoIdItem;
        }

        public bool IsInfoIdItem { get; private set; }
    }
}
