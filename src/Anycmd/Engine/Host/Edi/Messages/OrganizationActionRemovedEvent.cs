
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class OrganizationActionRemovedEvent : DomainEvent
    {
        public OrganizationActionRemovedEvent(OrganizationAction source) : base(source) { }
    }
}
