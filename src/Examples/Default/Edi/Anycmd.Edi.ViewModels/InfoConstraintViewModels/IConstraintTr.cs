
namespace Anycmd.Edi.ViewModels.InfoConstraintViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public interface IConstraintTr : IDbViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string FullName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string AuthorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? CreateOn { get; set; }
    }
}
