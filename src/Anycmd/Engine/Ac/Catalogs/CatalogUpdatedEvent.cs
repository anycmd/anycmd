
namespace Anycmd.Engine.Ac.Catalogs
{
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogUpdatedEvent : DomainEvent
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

        public ICatalogUpdateIo Input { get; private set; }
    }
}
