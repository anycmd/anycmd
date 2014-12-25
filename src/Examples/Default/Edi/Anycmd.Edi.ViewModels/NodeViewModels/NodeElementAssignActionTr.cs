
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public partial class NodeElementAssignActionTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid NodeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ElementId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ActionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid OntologyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ElementCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ElementName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAllowed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsAudit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActionIsAllow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ElementActionIsAudit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ElementActionIsAllow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Verb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        public int SortCode { get; set; }
    }
}
