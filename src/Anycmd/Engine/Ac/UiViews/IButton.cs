
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是按钮。
    /// <remarks>
    /// 按钮界面元素对应系统内的行为记录。用户通过点击按钮来告诉系统他/她希望系统发生什么运动。
    /// 这里的按钮只是个用以标识“添加”、“修改”、“删除”、“上传”等动词的记录，这里的按钮
    /// 尚没有绑定到系统内的Function记录。而IUiViewButton完成了Button到Function的绑定。
    /// </remarks>
    /// </summary>
    public interface IButton
    {
        /// <summary>
        /// 按钮标识
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 编码
        /// </summary>
        string Code { get; }

        /// <summary>
        /// 按钮分类取值来自于Catalog树上的Button节点下的分类字典。
        /// </summary>
        string CategoryCode { get; }

        /// <summary>
        /// 小图标
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// 排序
        /// </summary>
        int SortCode { get; }

        int IsEnabled { get; }
    }
}
