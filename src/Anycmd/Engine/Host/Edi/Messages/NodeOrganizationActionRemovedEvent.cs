
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class NodeOrganizationActionRemovedEvent : DomainEvent
    {
        public NodeOrganizationActionRemovedEvent(NodeOrganizationAction source) : base(source) { }
    }
}
