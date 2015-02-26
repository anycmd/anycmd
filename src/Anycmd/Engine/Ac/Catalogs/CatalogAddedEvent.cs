
namespace Anycmd.Engine.Ac.Catalogs
{
    /// <summary>
    /// 
    /// </summary>
    public class CatalogAddedEvent : EntityAddedEvent<ICatalogCreateIo>
    {
        public CatalogAddedEvent(IAcSession acSession, CatalogBase source, ICatalogCreateIo input)
            : base(acSession, source, input)
        {
        }
    }
}
