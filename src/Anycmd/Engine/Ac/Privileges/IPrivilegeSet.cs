
namespace Anycmd.Engine.Ac.Privileges
{
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是权限集。
    /// <remarks>
    /// 它是RolePrivilege、CatalogPrivilege的集合而不是AccountPrivilege的集合。AccountPrivilege的集合是会话级的，在AcSession中而不在这里。
    /// </remarks>
    /// </summary>
    public interface IPrivilegeSet : IEnumerable<PrivilegeState>
    {
    }
}
