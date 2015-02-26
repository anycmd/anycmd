
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是按钮
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
