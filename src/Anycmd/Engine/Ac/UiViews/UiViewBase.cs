
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using Model;

    /// <summary>
    /// 界面视图基类<see cref="IUiView"/>
    /// </summary>
    public abstract class UiViewBase : EntityBase, IUiView
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
