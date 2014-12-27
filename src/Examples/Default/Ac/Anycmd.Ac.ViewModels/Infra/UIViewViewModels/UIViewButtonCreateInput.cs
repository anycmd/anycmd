using System;

namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine.Ac.InOuts;
    using Model;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonCreateInput : EntityCreateInput, IInputModel, IUiViewButtonCreateIo
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
