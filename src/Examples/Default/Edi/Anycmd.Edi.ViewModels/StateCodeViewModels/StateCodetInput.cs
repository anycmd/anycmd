
namespace Anycmd.Edi.ViewModels.StateCodeViewModels
{
    using Model;
    using System;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class StateCodeInput : ManagedPropertyValues, IInfoModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
