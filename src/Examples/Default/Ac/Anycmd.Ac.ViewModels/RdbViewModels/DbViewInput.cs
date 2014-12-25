
namespace Anycmd.Ac.ViewModels.RdbViewModels
{
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 数据库视图输入模型
    /// </summary>
    public sealed class DbViewInput : IInputModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid DatabaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
