
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using System;

    public class RemoveNodeOntologyCatalogCommand : Command, IAnycmdCommand
    {
        public RemoveNodeOntologyCatalogCommand(IUserSession userSession, Guid nodeId, Guid ontologyId, Guid catalogId)
        {
            this.UserSession = userSession;
            this.NodeId = nodeId;
            this.OntologyId = ontologyId;
            this.CatalogId = catalogId;
        }

        public IUserSession UserSession { get; private set; }
        public Guid NodeId { get; private set; }
        public Guid OntologyId { get; private set; }
        public Guid CatalogId { get; private set; }
    }
}
