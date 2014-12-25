
namespace Anycmd.Ac.ViewModels.RdbViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModel;

    /// <summary>
    /// 分页获取数据库视图
    /// </summary>
    public sealed class GetPlistViews : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid? DatabaseId { get; set; }
    }
}
