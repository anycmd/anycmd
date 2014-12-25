
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine.Host.Ac.InOuts;
    using Model;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewUpdateInput : IInputModel, IUiViewUpdateIo
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Tooltip { get; set; }
    }
}
