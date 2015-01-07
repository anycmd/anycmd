
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeOrganizationActionAddedEvent : DomainEvent
    {
        public NodeOrganizationActionAddedEvent(NodeOrganizationAction source) : base(source) { }
    }
}
