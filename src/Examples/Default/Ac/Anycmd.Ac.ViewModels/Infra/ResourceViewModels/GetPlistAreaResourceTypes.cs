
namespace Anycmd.Ac.ViewModels.Infra.ResourceViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlistAreaResourceTypes : GetPlistResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid AppSystemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsAssigned { get; set; }
    }
}
