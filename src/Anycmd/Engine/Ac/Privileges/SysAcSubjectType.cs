
namespace Anycmd.Engine.Ac.Privileges
{
    /// <summary>
    /// 定义系统类别的合法Ac主体取值。
    /// <see cref="IPrivilege"/>
    /// </summary>
    public enum SysAcSubjectType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// 组客体类型。
        /// </summary>
        Group = 0x0008,
        /// <summary>
        /// 功能客体类型。
        /// </summary>
        Function = 0x0010,
        /// <summary>
        /// 菜单客体类型。
        /// </summary>
        Menu = 0x0020,
        /// <summary>
        /// 应用系统客体类型。
        /// </summary>
        AppSystem = 0x0040,
        /// <summary>
        /// 暂不支持，该取值的存在是为了概念完整性。组成授权路由链表。如同面向对象机制中类的“继承”。
        /// </summary>
        Privilege = 0x1fff
    }
}
