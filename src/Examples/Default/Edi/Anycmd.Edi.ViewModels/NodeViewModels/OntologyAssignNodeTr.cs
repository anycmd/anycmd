
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public partial class OntologyAssignNodeTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid OntologyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAssigned { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
