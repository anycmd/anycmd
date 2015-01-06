
namespace Anycmd.Engine.Ac.InOuts
{

    /// <summary>
    /// 表示该接口的实现类是更新权限二元组时的输入或输出参数类型。
    /// </summary>
    public interface IPrivilegeBigramUpdateIo : IEntityUpdateInput
    {
        string PrivilegeConstraint { get; }
    }
}
