
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using System;

    public class OntologyCatalogCreateInput : EntityCreateInput, IOntologyCatalogCreateIo
    {
        public OntologyCatalogCreateInput()
        {
            HecpOntology = "OntologyCatalog";
            HecpVerb = "Create";
        }

        public Guid OntologyId { get; set; }

        public Guid CatalogId { get; set; }

        public override IAnycmdCommand ToCommand(IAcSession userSession)
        {
            return new AddOntologyCatalogCommand(userSession, this);
        }
    }
}
