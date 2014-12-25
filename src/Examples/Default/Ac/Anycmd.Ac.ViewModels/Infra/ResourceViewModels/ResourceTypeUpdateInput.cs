
namespace Anycmd.Ac.ViewModels.Infra.ResourceViewModels
{
    using Engine.Host.Ac.InOuts;
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceTypeUpdateInput : IInputModel, IResourceTypeUpdateIo
    {
        public Guid Id { get; set; }
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
