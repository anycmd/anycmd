
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using System.ComponentModel;

    /// <summary>
    /// 事件源类型
    /// </summary>
    public enum EventSourceType
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("未定义的事件源类型")]
        Invalid = 0,
        /// <summary>
        /// 
        /// </summary>
        [Description("命令事件源")]
        Command = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("实体事件源")]
        Entity = 2
    }
}
