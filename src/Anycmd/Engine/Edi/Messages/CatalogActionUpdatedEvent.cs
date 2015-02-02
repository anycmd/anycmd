
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class CatalogActionUpdatedEvent : DomainEvent
    {
        public CatalogActionUpdatedEvent(IAcSession acSession, CatalogAction source)
            : base(acSession, source)
        {
        }
    }
}
