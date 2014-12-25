
namespace Anycmd.Ac.ViewModels.Infra.MenuViewModels
{
    using Engine.Host.Ac.InOuts;
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class MenuCreateInput : EntityCreateInput, IInputModel, IMenuCreateIo
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid AppSystemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
    }
}
