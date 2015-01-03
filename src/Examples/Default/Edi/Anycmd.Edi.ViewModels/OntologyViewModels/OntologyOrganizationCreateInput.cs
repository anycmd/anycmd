
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using System;

    public class OntologyOrganizationCreateInput : EntityCreateInput, IOntologyOrganizationCreateIo
    {
        public Guid OntologyId { get; set; }

        public Guid OrganizationId { get; set; }
    }
}
