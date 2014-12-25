
namespace Anycmd.Engine.Host.Edi.Handlers
{

    /// <summary>
    /// 命令类型
    /// </summary>
    public enum MessageTypeKind : byte
    {
        /// <summary>
        /// 未定义的命令
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// AnyCommand
        /// </summary>
        AnyCommand = 1,
        /// <summary>
        /// 已成功接收的命令
        /// </summary>
        Received = 2,
        /// <summary>
        /// 接收失败的命令
        /// </summary>
        Unaccepted = 3,
        /// <summary>
        /// 已成功执行的命令
        /// </summary>
        Executed = 4,
        /// <summary>
        /// 执行失败的命令
        /// </summary>
        ExecuteFailing = 5,
        /// <summary>
        /// 待分发命令
        /// </summary>
        Distribute = 6,
        /// <summary>
        /// 已成功分发的命令
        /// </summary>
        Distributed = 7,
        /// <summary>
        /// 分发失败的命令
        /// </summary>
        DistributeFailing = 8,
        /// <summary>
        /// 本地事件
        /// </summary>
        LocalEvent = 9,
        /// <summary>
        /// 客户端事件
        /// </summary>
        ClientEvent = 10
    }
}
