
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是界面视图类型。
    /// <remarks>
    /// 界面视图是空间视图，它跟场所、地点等概念是一类的。主体的分神在空间中活动，界面视图就是空间的投影。
    /// </remarks>
    /// </summary>
    public interface IUiView
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 帮助、提示信息。
        /// </summary>
        string Tooltip { get; }
        /// <summary>
        /// 
        /// </summary>
        string Icon { get; }
    }
}
