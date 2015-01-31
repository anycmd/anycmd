
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class RemoveOntologyCatalogCommand : Command, IAnycmdCommand
    {
        public RemoveOntologyCatalogCommand(IUserSession userSession, Guid ontologyId, Guid catalogId)
        {
            this.UserSession = userSession;
            this.OntologyId = ontologyId;
            this.CatalogId = catalogId;
        }

        public IUserSession UserSession { get; private set; }

        public Guid OntologyId { get; private set; }

        public Guid CatalogId { get; private set; }
    }
}
