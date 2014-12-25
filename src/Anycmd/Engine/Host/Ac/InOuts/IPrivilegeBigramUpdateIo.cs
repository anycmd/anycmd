
namespace Anycmd.Engine.Host.Ac.InOuts
{
    using Model;

    /// <summary>
    /// 表示该接口的实现类是更新权限二元组时的输入或输出参数类型。
    /// </summary>
    public interface IPrivilegeBigramUpdateIo : IEntityUpdateInput
    {
        string PrivilegeConstraint { get; }
    }
}
