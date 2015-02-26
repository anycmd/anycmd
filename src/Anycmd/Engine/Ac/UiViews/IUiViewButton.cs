
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是界面视图菜单类型。
    /// </summary>
    public interface IUiViewButton
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid UiViewId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid? FunctionId { get; }
        /// <summary>
        /// 
        /// </summary>
        Guid ButtonId { get; }

        /// <summary>
        /// 菜单在界面的有效状态
        /// <remarks>是否可点击的意思</remarks>
        /// </summary>
        int IsEnabled { get; }
    }
}
