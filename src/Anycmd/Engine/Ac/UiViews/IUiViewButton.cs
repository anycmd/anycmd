
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是界面视图菜单类型。
    /// <remarks>
    /// 按钮界面元素对应系统内的行为记录。用户通过点击按钮来告诉系统他/她希望系统发生什么运动。
    /// 这里的按钮只是个用以标识“添加”、“修改”、“删除”、“上传”等动词的记录，这里的按钮
    /// 尚没有绑定到系统内的Function记录。而IUiViewButton完成了Button到Function的绑定。
    /// </remarks>
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
