
namespace Anycmd.Engine.Ac.Catalogs
{
    using Messages;

    public sealed class CatalogAddedEvent : EntityAddedEvent<ICatalogCreateIo>
    {
        public CatalogAddedEvent(IAcSession acSession, CatalogBase source, ICatalogCreateIo input)
            : base(acSession, source, input)
        {
        }

        internal CatalogAddedEvent(IAcSession acSession, CatalogBase source, ICatalogCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
