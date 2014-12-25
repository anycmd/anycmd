
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistNodeElementActions : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ElementId { get; set; }
    }
}
