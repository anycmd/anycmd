
namespace Anycmd.Engine.Ac.InOuts
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是创建系统字典时的输入或输出参数模型。
    /// </summary>
    public interface IDicCreateIo : IEntityCreateInput
    {
        string Code { get; }
        string Description { get; }
        int IsEnabled { get; }
        int SortCode { get; }
        string Name { get; }
    }
}
