
namespace Anycmd.Engine.Ac.Ssd
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是静态职责分离角色。
    /// </summary>
    public interface ISsdRole
    {
        /// <summary>
        /// 查看静态职责分离角色标识。
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 查看静态职责分离角色集标识。
        /// </summary>
        Guid SsdSetId { get; }
        /// <summary>
        /// 查看角色标识。
        /// </summary>
        Guid RoleId { get; }
    }
}
