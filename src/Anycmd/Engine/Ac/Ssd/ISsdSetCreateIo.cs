
namespace Anycmd.Engine.Ac.Ssd
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是创建静态职责分离角色集时的输入或输出参数类型。
    /// </summary>
    public interface ISsdSetCreateIo : IEntityCreateInput
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
