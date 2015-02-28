
namespace Anycmd.Engine.Ac.Catalogs
{
    using Messages;

    public sealed class CatalogAddedEvent : EntityAddedEvent<ICatalogCreateIo>
    {
        public CatalogAddedEvent(IAcSession acSession, CatalogBase source, ICatalogCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
