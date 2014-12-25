using System;

namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine.Host.Ac.InOuts;
    using Model;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonUpdateInput : IInputModel, IUiViewButtonUpdateIo
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? FunctionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
    }
}
