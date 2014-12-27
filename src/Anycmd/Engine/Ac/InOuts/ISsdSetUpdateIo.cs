
namespace Anycmd.Engine.Ac.InOuts
{
    using Model;

    /// <summary>
    /// 表示该接口的实现类是更新静态职责分离角色集时的输入或输出参数类型。
    /// </summary>
    public interface ISsdSetUpdateIo : IEntityUpdateInput
    {
        string Name { get; }

        /// <summary>
        /// 阀值
        /// </summary>
        int SsdCard { get; }

        /// <summary>
        /// 是否启用
        /// </summary>
        int IsEnabled { get; }

        string Description { get; }
    }
}
