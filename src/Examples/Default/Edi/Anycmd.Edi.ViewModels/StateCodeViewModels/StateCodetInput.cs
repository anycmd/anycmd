
namespace Anycmd.Edi.ViewModels.StateCodeViewModels
{
    using Engine;
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
