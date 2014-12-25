
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistOntologyOrganizationActions : GetPlistResult
    {
        public Guid OntologyId { get; set; }

        public Guid OrganizationId { get; set; }
    }
}