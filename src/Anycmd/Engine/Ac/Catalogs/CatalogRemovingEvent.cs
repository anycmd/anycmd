
namespace Anycmd.Engine.Ac.Catalogs
{
    using Events;

    public class CatalogRemovingEvent: DomainEvent
    {
        public CatalogRemovingEvent(IAcSession acSession, CatalogBase source)
            : base(acSession, source)
        {
        }
    }
}
