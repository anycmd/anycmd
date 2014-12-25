
namespace Anycmd.Engine.Ac.Abstractions.Infra
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是界面视图类型。
    /// </summary>
    public interface IUiView
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 帮助、提示信息
        /// </summary>
        string Tooltip { get; }
        /// <summary>
        /// 
        /// </summary>
        string Icon { get; }
    }
}
