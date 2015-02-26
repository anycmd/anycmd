
namespace Anycmd.Engine.Ac.Ssd
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是静态责任分离角色集
    /// </summary>
    public interface ISsdSet
    {
        Guid Id { get; }
        string Name { get; }
        int SsdCard { get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        int IsEnabled { get; }

        string Description { get; }

        DateTime? CreateOn { get; }
    }
}
