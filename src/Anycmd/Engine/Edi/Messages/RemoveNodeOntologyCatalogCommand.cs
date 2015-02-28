
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using Commands;
    using System;

    public class RemoveNodeOntologyCatalogCommand : Command, IAnycmdCommand
    {
        public RemoveNodeOntologyCatalogCommand(IAcSession acSession, Guid nodeId, Guid ontologyId, Guid catalogId)
        {
            this.AcSession = acSession;
            this.NodeId = nodeId;
            this.OntologyId = ontologyId;
            this.CatalogId = catalogId;
        }

        public IAcSession AcSession { get; private set; }
        public Guid NodeId { get; private set; }
        public Guid OntologyId { get; private set; }
        public Guid CatalogId { get; private set; }
    }
}
