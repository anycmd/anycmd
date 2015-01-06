
namespace Anycmd.Engine.Ac.InOuts
{

    /// <summary>
    /// 表示该接口的实现类是更新岗位时的输入或输出参数类型。
    /// </summary>
    public interface IPositionUpdateIo : IEntityUpdateInput
    {
        string CategoryCode { get; }
        string Description { get; }
        int IsEnabled { get; }
        string Name { get; }
        string ShortName { get; }
        int SortCode { get; }
    }
}
