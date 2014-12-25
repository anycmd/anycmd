
namespace Anycmd.Ac.ViewModels.Identity.AccountViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistMyVisitingLogs : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LeftVisitOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RightVisitOn { get; set; }
    }
}
