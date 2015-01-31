
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogUpdatedEvent : DomainEvent
    {
        public CatalogUpdatedEvent(IUserSession userSession, CatalogBase source, ICatalogUpdateIo input)
            : base(userSession, source)
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
