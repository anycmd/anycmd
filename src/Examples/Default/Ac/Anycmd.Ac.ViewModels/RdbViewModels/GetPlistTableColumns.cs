
namespace Anycmd.Ac.ViewModels.RdbViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    /// <summary>
    /// 分页获取数据库表列
    /// </summary>
    public sealed class GetPlistTableColumns : GetPlistResult
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
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string TableName { get; set; }
        [Required]
        public string TableId { get; set; }
    }
}
