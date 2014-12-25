
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class NodeOrganizationActionAddedEvent : DomainEvent
    {
        public NodeOrganizationActionAddedEvent(NodeOrganizationAction source) : base(source) { }
    }
}
