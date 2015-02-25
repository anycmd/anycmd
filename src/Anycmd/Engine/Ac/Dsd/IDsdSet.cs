
namespace Anycmd.Engine.Ac.Dsd
{
    using System;

    /// <summary>
    /// 表示动态责任分离角色集
    /// </summary>
    public interface IDsdSet
    {
        Guid Id { get; }
        string Name { get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        int IsEnabled { get; }

        /// <summary>
        /// 阀值
        /// </summary>
        int DsdCard { get; }

        string Description { get; }

        DateTime? CreateOn { get; }
    }
}
