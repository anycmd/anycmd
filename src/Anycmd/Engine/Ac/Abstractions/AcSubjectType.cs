
namespace Anycmd.Engine.Ac.Abstractions
{
    /// <summary>
    /// 定义用户类别的合法Ac主体取值。
    /// <see cref="IPrivilegeBigram"/>
    /// </summary>
    public enum AcSubjectType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// 账户主体类型。
        /// </summary>
        Account = 0x0001,
        /// <summary>
        /// 角色主体类型。
        /// </summary>
        Role = 0x0002,
        /// <summary>
        /// 组织结构主体类型。
        /// </summary>
        Organization = 0x0004,
        /// <summary>
        /// 暂不支持，该取值的存在是为了概念完整性。组成授权路由链表。如同面向对象机制中类的继承。
        /// </summary>
        Privilege = 0x1fff
    }
}
