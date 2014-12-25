
namespace Anycmd.Edi.ViewModels.InfoConstraintViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public interface IConstraintInfo : IInfoModel
    {
        /// <summary>
        /// 
        /// </summary>
        string Title { get; }
        /// <summary>
        /// 
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        string FullName { get; }
        /// <summary>
        /// 
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 
        /// </summary>
        string AuthorCode { get; }
        /// <summary>
        /// 
        /// </summary>
        int IsEnabled { get; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? CreateOn { get; }
    }
}
