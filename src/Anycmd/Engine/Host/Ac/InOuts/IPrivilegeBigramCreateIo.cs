
namespace Anycmd.Engine.Host.Ac.InOuts
{
    using Model;
    using System;

    /// <summary>
    /// 表示该接口的实现类是创建权限二元组时的输入或输出参数类型。
    /// </summary>
    public interface IPrivilegeBigramCreateIo : IEntityCreateInput
    {
        string SubjectType { get; }
        Guid SubjectInstanceId { get; }
        string ObjectType { get; }
        Guid ObjectInstanceId { get; }
        string PrivilegeConstraint { get; }
        int PrivilegeOrientation { get; }
    }
}
