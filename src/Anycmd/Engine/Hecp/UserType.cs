
namespace Anycmd.Engine.Hecp
{
    using System.ComponentModel;

    /// <summary>
    /// 用户类型枚举
    /// </summary>
    public enum UserType : byte
    {
        /// <summary>
        /// 未定义的用户类型
        /// </summary>
        [Description("未定义的用户类型")]
        None = 0,
        /// <summary>
        /// 实体自己
        /// </summary>
        [Description("实体自己")]
        EntitySelf = 1,
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Manager = 2
    }
}
