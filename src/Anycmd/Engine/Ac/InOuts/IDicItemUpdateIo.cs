
namespace Anycmd.Engine.Ac.InOuts
{
    using Model;

    /// <summary>
    /// 表示该接口的实现类是更新系统字典项时的输入或输出参数类型。
    /// </summary>
    public interface IDicItemUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Description { get; }
        int IsEnabled { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
