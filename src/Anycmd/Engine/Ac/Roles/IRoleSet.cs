
namespace Anycmd.Engine.Ac.Roles
{
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
        /// 尝试读取给定的标识标定的角色。
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        bool TryGetRole(Guid roleId, out RoleState role);

        /// <summary>
        /// 读取给定的标识标定的角色，如果标识的角色不存在则抛出异常。
        /// </summary>
        /// <param name="roleId">角色标识</param>
        /// <returns></returns>
        RoleState GetRole(Guid roleId);

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
