
namespace Anycmd.Ac.ViewModels.Infra.DicViewModels
{
    using Engine.Ac.InOuts;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DicUpdateInput : IDicUpdateIo
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
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
    }
}
