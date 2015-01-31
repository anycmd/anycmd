
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogAddedEvent : EntityAddedEvent<ICatalogCreateIo>
    {
        public CatalogAddedEvent(IUserSession userSession, CatalogBase source, ICatalogCreateIo input)
            : base(userSession, source, input)
        {
        }
    }
}
