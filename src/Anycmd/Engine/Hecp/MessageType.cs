
namespace Anycmd.Engine.Hecp
{
    using System.ComponentModel;

    /// <summary>
    /// 命令请求类型枚举。必须是Action或Command或Event。Action接收立即执行，Command接收后周期执行，如何处理Event由服务端自定。
    /// </summary>
    public enum MessageType : byte
    {
        /// <summary>
        /// 未定义的请求类型
        /// </summary>
        [Description("未定义的请求类型")]
        Undefined = 0,
        /// <summary>
        /// 行动。Action接收立即执行
        /// </summary>
        [Description("行动")]
        Action = 1,
        /// <summary>
        /// 命令。Command接收后周期执行
        /// </summary>
        [Description("命令")]
        Command = 2,
        /// <summary>
        /// 事件。如何处理Event由服务端自定
        /// </summary>
        [Description("事件")]
        Event = 3
    }
}
