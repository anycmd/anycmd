
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine;
    using Engine.Ac.InOuts;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonCreateInput : EntityCreateInput, IUiViewButtonCreateIo
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid UiViewId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ButtonId { get; set; }
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
