
namespace Anycmd.Engine.Host.Ac.Rbac
{
    using Engine.Ac.Dsd;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示i动态职责分离角色数据访问实体。
    /// </summary>
    public class DsdRole : DsdRoleBase, IAggregateRoot
    {
        public static DsdRole Create(IDsdRoleCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new DsdRole
            {
                Id = input.Id.Value,
                RoleId = input.RoleId,
                DsdSetId = input.DsdSetId
            };
        }
    }
}
