
namespace Anycmd.Engine.Ac.Catalogs
{
    using Messages;

    public class CatalogAddedEvent : EntityAddedEvent<ICatalogCreateIo>
    {
        public CatalogAddedEvent(IAcSession acSession, CatalogBase source, ICatalogCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
