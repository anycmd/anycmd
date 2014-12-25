
namespace Anycmd.Edi.ViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistEntity : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string OntologyCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? ArchiveId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrganizationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? Includedescendants { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? Translate { get; set; }
    }
}
