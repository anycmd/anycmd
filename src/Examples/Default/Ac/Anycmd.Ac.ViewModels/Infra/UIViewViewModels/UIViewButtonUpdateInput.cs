
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine.Ac.InOuts;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonUpdateInput : IUiViewButtonUpdateIo
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
