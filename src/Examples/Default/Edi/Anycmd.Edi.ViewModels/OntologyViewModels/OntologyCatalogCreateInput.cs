
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.InOuts;
    using Engine.Messages;
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

        public override IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new AddOntologyCatalogCommand(acSession, this);
        }
    }
}
