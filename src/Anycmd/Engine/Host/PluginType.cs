
namespace Anycmd.Engine.Host
{
    using System.ComponentModel;

    /// <summary>
    /// 表示插件类型
    /// </summary>
    public enum PluginType : byte
    {
        /// <summary>
        /// 未定义的插件类型
        /// </summary>
        [Description("未定义的插件类型")]
        Invalid = 0,
        /// <summary>
        /// 消息提供程序
        /// </summary>
        [Description("消息提供程序")]
        MessageProvider = 1,
        /// <summary>
        /// 实体提供程序
        /// </summary>
        [Description("实体提供程序")]
        EntityProvider = 2,
        /// <summary>
        /// 信息字符串转化器
        /// </summary>
        [Description("信息字符串转化器")]
        InfoStringConverter = 3,
        /// <summary>
        /// 信息验证器
        /// </summary>
        [Description("信息验证器")]
        InfoConstraint = 4,
        /// <summary>
        /// 消息转移器
        /// </summary>
        [Description("消息转移器")]
        MessageTransfer = 5,
        /// <summary>
        /// 通用插件
        /// </summary>
        [Description("插件")]
        Plugin = 6
    }
}
