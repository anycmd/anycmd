
namespace Anycmd.Engine.Ac.Dsd
{
    using System;

    /// <summary>
    /// 表示动态职责分离角色。
    /// </summary>
    public interface IDsdRole
    {
        /// <summary>
        /// 查看动态职责分离角色标识。
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 查看动态职责分离角色集标识。
        /// </summary>
        Guid DsdSetId { get; }

        /// <summary>
        /// 查看角色标识。
        /// </summary>
        Guid RoleId { get; }
    }
}
