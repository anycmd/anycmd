
namespace Anycmd.Engine.Ac.Catalogs
{
    using Events;

    public sealed class CatalogRemovingEvent : DomainEvent
    {
        public CatalogRemovingEvent(IAcSession acSession, CatalogBase source)
            : base(acSession, source)
        {
        }
    }
}
