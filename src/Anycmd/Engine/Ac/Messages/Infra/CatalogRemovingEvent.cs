
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class CatalogRemovingEvent: DomainEvent
    {
        public CatalogRemovingEvent(IUserSession userSession, CatalogBase source)
            : base(userSession, source)
        {
        }
    }
}
