
namespace Anycmd.Engine.Host.Ac.InOuts
{
    using Model;

    /// <summary>
    /// 表示该接口的实现类是更新系统字典时的输入或输出参数类型。
    /// </summary>
    public interface IDicUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Description { get; }
        int IsEnabled { get; }
        int SortCode { get; }
        string Name { get; }
    }
}
