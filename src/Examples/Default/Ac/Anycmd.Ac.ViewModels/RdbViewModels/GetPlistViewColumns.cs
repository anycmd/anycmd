
namespace Anycmd.Ac.ViewModels.RdbViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    /// <summary>
    /// 分页获取数据库视图列
    /// </summary>
    public sealed class GetPlistViewColumns : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid? DatabaseId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SchemaName { get; set; }
        [Required]
        public string ViewName { get; set; }
        [Required]
        public string ViewId { get; set; }
    }
}
