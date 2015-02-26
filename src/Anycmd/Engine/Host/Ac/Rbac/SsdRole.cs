
namespace Anycmd.Engine.Host.Ac.Rbac
{
    using Engine.Ac.Ssd;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示静态职责分离角色数据访问实体。
    /// </summary>
    public class SsdRole : SsdRoleBase, IAggregateRoot
    {
        public static SsdRole Create(ISsdRoleCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new SsdRole
            {
                Id = input.Id.Value,
                SsdSetId = input.SsdSetId,
                RoleId = input.RoleId
            };
        }
    }
}
