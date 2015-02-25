
namespace Anycmd.Engine.Ac.Groups
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是更新组时的输入或输出参数类型。
    /// </summary>
    public interface IGroupUpdateIo : IEntityUpdateInput
    {
        string CategoryCode { get; }
        string Description { get; }
        int IsEnabled { get; }
        string Name { get; }
        string ShortName { get; }
        int SortCode { get; }
    }
}
