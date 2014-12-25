
namespace Anycmd.Engine.Host.Ac
{
    using System.ComponentModel;

    public enum AllowType : byte
    {
        /// <summary>
        /// 非法的动作控制
        /// </summary>
        [Description("非法的动作控制")]
        Invalid = 0,
        /// <summary>
        /// 显式允许动作
        /// </summary>
        [Description("显式允许动作")]
        ExplicitAllow = 1,
        /// <summary>
        /// 隐式允许动作
        /// </summary>
        [Description("隐式允许动作")]
        ImplicitAllow = 2,
        /// <summary>
        /// 显式不允许动作
        /// </summary>
        [Description("显式不允许动作")]
        NotAllow = 3
    }
}
