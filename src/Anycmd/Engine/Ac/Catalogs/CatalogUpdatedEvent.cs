
namespace Anycmd.Engine.Ac.Catalogs
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class CatalogUpdatedEvent : DomainEvent
    {
        public CatalogUpdatedEvent(IAcSession acSession, CatalogBase source, ICatalogUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        internal CatalogUpdatedEvent(IAcSession acSession, CatalogBase source, ICatalogUpdateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        public ICatalogUpdateIo Input { get; private set; }
        internal bool IsPrivate { get; private set; }
    }
}
