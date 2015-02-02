
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class CatalogActionUpdatedEvent : DomainEvent
    {
        public CatalogActionUpdatedEvent(IAcSession userSession, CatalogAction source)
            : base(userSession, source)
        {
        }
    }
}
