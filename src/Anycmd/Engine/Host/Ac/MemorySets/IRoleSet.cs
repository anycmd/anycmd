
namespace Anycmd.Engine.Host.Ac.MemorySets
{
    using Engine.Ac;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是角色集。
    /// </summary>
    public interface IRoleSet : IEnumerable<RoleState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool TryGetRole(Guid roleId, out RoleState role);
        /// <summary>
        /// 返回给定角色的子孙角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>

        IReadOnlyCollection<RoleState> GetDescendantRoles(RoleState role);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        IReadOnlyCollection<RoleState> GetAscendantRoles(RoleState role);
    }
}
