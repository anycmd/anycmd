
namespace Anycmd.Engine.Ac.Ssd
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是静态职责分离角色集。
    /// </summary>
    public interface ISsdSetSet : IEnumerable<SsdSetState>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssdSetId"></param>
        /// <param name="ssdSet"></param>
        /// <returns></returns>
        bool TryGetSsdSet(Guid ssdSetId, out SsdSetState ssdSet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssdSet"></param>
        /// <returns></returns>
        IReadOnlyCollection<SsdRoleState> GetSsdRoles(SsdSetState ssdSet);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<SsdRoleState> GetSsdRoles();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CheckRoles(HashSet<RoleState> roles, out string msg);
    }
}
