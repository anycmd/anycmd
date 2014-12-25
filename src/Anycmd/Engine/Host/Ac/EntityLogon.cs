
namespace Anycmd.Engine.Host.Ac
{
    using System.ComponentModel;

    public enum EntityLogon : byte
    {
        /// <summary>
        /// 非法的登录类型
        /// </summary>
        [Description("非法的登录类型")]
        Invalid = 0,
        /// <summary>
        /// 显式审核
        /// </summary>
        [Description("显式登录")]
        ExplicitLogon = 1,
        /// <summary>
        /// 隐式审核
        /// </summary>
        [Description("隐式登录")]
        ImplicitLogon = 2,
        /// <summary>
        /// 显式不审核
        /// </summary>
        [Description("显式不登录")]
        NotLogon = 3
    }
}
