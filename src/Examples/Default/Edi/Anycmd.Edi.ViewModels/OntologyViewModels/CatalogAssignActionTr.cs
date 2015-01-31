
namespace Anycmd.Edi.ViewModels.OntologyViewModels
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class CatalogAssignActionTr
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
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
        public Guid CatalogId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid OntologyCatalogId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IsAudit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IsAllowed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActionIsAllowed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActionIsAudit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Verb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
