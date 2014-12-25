
namespace Anycmd.Ac.ViewModels.RdbViewModels
{
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 关系数据库输入模型
    /// </summary>
    public sealed class DatabaseInput : IInputModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
