
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using System;

    public class NodeOntologyCatalogCreateInput : INodeOntologyCatalogCreateIo
    {
        public NodeOntologyCatalogCreateInput()
        {
            this.HecpOntology = "NodeOntologyCatalog";
            this.HecpVerb = "Create";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        public Guid NodeId { get; set; }

        public Guid OntologyId { get; set; }

        public Guid CatalogId { get; set; }

        public Guid? Id { get; set; }

        public IAnycmdCommand ToCommand(IUserSession userSession)
        {
            return new AddNodeOntologyCatalogCommand(userSession, this);
        }
    }
}
