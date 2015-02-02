
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class CatalogActionAddedEvent : DomainEvent
    {
        public CatalogActionAddedEvent(IAcSession userSession, CatalogAction source) : base(userSession, source) { }
    }
}
