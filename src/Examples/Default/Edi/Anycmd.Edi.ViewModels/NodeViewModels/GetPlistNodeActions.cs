
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistNodeActions : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid OntologyId { get; set; }
    }
}
