
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class CatalogActionUpdatedEvent : DomainEvent
    {
        public CatalogActionUpdatedEvent(IAcSession acSession, CatalogAction source)
            : base(acSession, source)
        {
        }
    }
}
