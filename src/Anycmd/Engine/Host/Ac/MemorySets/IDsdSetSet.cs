
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Engine.Ac;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是动态职责分离角色集。
    /// </summary>
    public interface IDsdSetSet : IEnumerable<DsdSetState>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsdSetId"></param>
        /// <param name="dsdSet"></param>
        /// <returns></returns>
        bool TryGetDsdSet(Guid dsdSetId, out DsdSetState dsdSet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsdSet"></param>
        /// <returns></returns>
        IReadOnlyCollection<DsdRoleState> GetDsdRoles(DsdSetState dsdSet);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<DsdRoleState> GetDsdRoles();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool CheckRoles(IEnumerable<RoleState> roles, out string msg);
    }
}
