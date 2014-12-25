
namespace Anycmd.Edi.ViewModels.StateCodeViewModels
{
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class StateCodeInfo : IDbViewModel, IInfoModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReasonPhrase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
    }
}
