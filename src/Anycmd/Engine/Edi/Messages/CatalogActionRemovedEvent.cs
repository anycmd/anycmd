
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class CatalogActionRemovedEvent : DomainEvent
    {
        public CatalogActionRemovedEvent(IAcSession userSession, CatalogAction source) : base(userSession, source) { }
    }
}
