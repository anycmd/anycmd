
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistOntologyCatalogActions : GetPlistResult
    {
        public Guid OntologyId { get; set; }

        public Guid CatalogId { get; set; }
    }
}