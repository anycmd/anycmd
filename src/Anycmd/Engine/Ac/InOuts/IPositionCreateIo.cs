
namespace Anycmd.Engine.Ac.InOuts
{

    /// <summary>
    /// 表示该接口的实现类是创建岗位时的输入或输出参数类型。
    /// <remarks>
    /// 岗位是绑定了组织结构的工作组。
    /// </remarks>
    /// </summary>
    public interface IPositionCreateIo : IEntityCreateInput
    {
        string OrganizationCode { get; }
        string CategoryCode { get; }
        string Description { get; }
        int IsEnabled { get; }
        string Name { get; }
        string ShortName { get; }
        int SortCode { get; }
    }
}