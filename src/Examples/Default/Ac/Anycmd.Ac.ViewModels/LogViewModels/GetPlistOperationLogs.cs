
namespace Anycmd.Ac.ViewModels.LogViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistOperationLogs : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LeftCreateOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RightCreateOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? TargetId { get; set; }
    }
}
