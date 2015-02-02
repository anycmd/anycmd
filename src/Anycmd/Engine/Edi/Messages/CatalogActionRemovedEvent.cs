
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class CatalogActionRemovedEvent : DomainEvent
    {
        public CatalogActionRemovedEvent(IAcSession acSession, CatalogAction source) : base(acSession, source) { }
    }
}
