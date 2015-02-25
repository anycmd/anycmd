
namespace Anycmd.Ac.ViewModels.Infra.MenuViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistMenuChildren : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
