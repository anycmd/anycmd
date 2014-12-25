
namespace Anycmd.Edi.ViewModels.InfoDicViewModels
{
    using Engine.Host.Edi.InOuts;
    using Model;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class InfoDicItemCreateInput : EntityCreateInput, IInputModel, IInfoDicItemCreateIo
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid InfoDicId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }
    }
}
