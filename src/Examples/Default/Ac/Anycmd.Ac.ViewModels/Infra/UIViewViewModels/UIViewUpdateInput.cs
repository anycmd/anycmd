
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine.Ac.InOuts;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewUpdateInput : IUiViewUpdateIo
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
