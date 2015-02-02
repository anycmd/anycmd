
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class CatalogRemovingEvent: DomainEvent
    {
        public CatalogRemovingEvent(IAcSession acSession, CatalogBase source)
            : base(acSession, source)
        {
        }
    }
}
