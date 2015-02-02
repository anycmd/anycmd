
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class CatalogActionAddedEvent : DomainEvent
    {
        public CatalogActionAddedEvent(IAcSession acSession, CatalogAction source) : base(acSession, source) { }
    }
}
