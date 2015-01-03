
namespace Anycmd.Edi.ViewModels.ProcessViewModels
{
    using Engine;
    using Engine.Edi.InOuts;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessCreateInput : EntityCreateInput, IProcessCreateIo
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid OntologyId { get; set; }

        [Required]
        public int NetPort { get; set; }

        public string OrganizationCode { get; set; }
    }
}
