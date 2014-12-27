
namespace Anycmd.Engine.Host.Ac
{
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示权限二元组数据访问实体。
    /// </summary>
    public class PrivilegeBigram : PrivilegeBigramBase, IAggregateRoot
    {
        public PrivilegeBigram() { }

        public static PrivilegeBigram Create(IPrivilegeBigramCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new PrivilegeBigram
            {
                Id = input.Id.Value,
                SubjectType = input.SubjectType,
                SubjectInstanceId = input.SubjectInstanceId,
                ObjectType = input.ObjectType,
                ObjectInstanceId = input.ObjectInstanceId,
                PrivilegeConstraint = input.PrivilegeConstraint,
                PrivilegeOrientation = input.PrivilegeOrientation
            };
        }

        public void Update(IPrivilegeBigramUpdateIo input)
        {
            this.PrivilegeConstraint = input.PrivilegeConstraint;
        }
    }
}
