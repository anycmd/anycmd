
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeActionUpdatedEvent : DomainEvent
    {
        public NodeActionUpdatedEvent(NodeAction source)
            : base(source)
        {
            this.IsAllowed = source.IsAllowed;
            this.IsAudit = source.IsAudit;
        }

        public string IsAllowed { get; private set; }
        public string IsAudit { get; private set; }
    }
}
