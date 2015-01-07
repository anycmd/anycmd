
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class OrganizationActionRemovedEvent : DomainEvent
    {
        public OrganizationActionRemovedEvent(OrganizationAction source) : base(source) { }
    }
}
