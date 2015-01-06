
namespace Anycmd.Engine.Ac.InOuts
{

    /// <summary>
    /// 表示该接口的实现类是更新系统资源类型时的输入或输出参数类型。
    /// </summary>
    public interface IResourceTypeUpdateIo : IEntityUpdateInput
    {
        string Code { get; }
        string Description { get; }
        string Icon { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
