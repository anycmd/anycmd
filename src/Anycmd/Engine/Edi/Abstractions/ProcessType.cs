
namespace Anycmd.Engine.Edi.Abstractions
{
    using System.ComponentModel;

    /// <summary>
    /// 进程类型
    /// </summary>
    public enum ProcessType : byte
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义的进程类型")]
        Invalid = 0,
        /// <summary>
        /// 平台管理Mis进程
        /// </summary>
        [Description("平台管理")]
        Mis = 1,
        /// <summary>
        /// 命令服务器进程
        /// </summary>
        [Description("Web服务")]
        Service = 2,
        /// <summary>
        /// 命令执行器进程
        /// </summary>
        [Description("命令执行器")]
        CommandExecutor = 3,
        /// <summary>
        /// 命令分发器进程
        /// </summary>
        [Description("命令分发器")]
        CommandDispatcher = 4
    }
}
