
namespace Anycmd.Logging
{
    using System.ComponentModel;

    /// <summary>
    /// 访问状态枚举
    /// </summary>
    public enum VisitState
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("未定义")]
        Default = 0,
        /// <summary>
        /// 
        /// </summary>
        [Description("登录成功")]
        Logged = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("登录失败")]
        LogOnFail = 2,
        /// <summary>
        /// 
        /// </summary>
        [Description("成功退出")]
        LogOut = 3,
        /// <summary>
        /// 
        /// </summary>
        [Description("超时退出")]
        Timeout = 4
    }
}
