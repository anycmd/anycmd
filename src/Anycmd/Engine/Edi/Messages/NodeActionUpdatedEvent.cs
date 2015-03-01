
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class NodeActionUpdatedEvent : DomainEvent
    {
        public NodeActionUpdatedEvent(IAcSession acSession, NodeAction source)
            : base(acSession, source)
        {
            this.IsAllowed = source.IsAllowed;
            this.IsAudit = source.IsAudit;
        }

        public string IsAllowed { get; private set; }
        public string IsAudit { get; private set; }
    }
}
