
namespace Anycmd.Ac.ViewModels.Infra.UIViewViewModels
{
    using Engine;
    using Engine.Ac.InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewCreateInput : EntityCreateInput, IUiViewCreateIo
    {
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
