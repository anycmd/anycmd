
namespace Anycmd.Engine.Ac.Roles
{
    using Engine.InOuts;

    /// <summary>
    /// 表示该接口的实现类是创建角色时的输入或输出参数类型。
    /// </summary>
    public interface IRoleCreateIo : IEntityCreateInput
    {
        string CategoryCode { get; }
        string Description { get; }
        string Icon { get; }
        int IsEnabled { get; }
        string Name { get; }
        int SortCode { get; }
    }
}
