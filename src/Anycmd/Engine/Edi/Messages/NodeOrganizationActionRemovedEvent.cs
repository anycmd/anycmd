
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeOrganizationActionRemovedEvent : DomainEvent
    {
        public NodeOrganizationActionRemovedEvent(NodeOrganizationAction source) : base(source) { }
    }
}
