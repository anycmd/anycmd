
namespace Anycmd.Engine.Host.Ac
{
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
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
