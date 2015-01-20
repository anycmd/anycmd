
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using System;

    public class OntologyOrganizationCreateInput : EntityCreateInput, IOntologyOrganizationCreateIo
    {
        public OntologyOrganizationCreateInput()
        {
            HecpOntology = "OntologyOrganization";
            HecpVerb = "Create";
        }

        public Guid OntologyId { get; set; }

        public Guid OrganizationId { get; set; }

        public override IAnycmdCommand ToCommand(IUserSession userSession)
        {
            return new AddOntologyOrganizationCommand(userSession, this);
        }
    }
}
