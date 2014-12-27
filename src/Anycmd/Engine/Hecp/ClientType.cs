
namespace Anycmd.Engine.Hecp
{
    using System.ComponentModel;

    /// <summary>
    /// 客户端类型枚举
    /// </summary>
    public enum ClientType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("未定义的客户端类型")]
        Undefined = 0,
        /// <summary>
        /// 节点。节点的能力比较完整，等于或接近中心节点。
        /// </summary>
        [Description("节点")]
        Node = 1,
        /// <summary>
        /// app。应用是能力不完整的节点。比如那个用于导入数据库的ExcelClient Winform程序就算App而不算Node。
        /// </summary>
        [Description("应用")]
        App = 2,
        /// <summary>
        /// 监察程序、巡查程序。暂不支持。设计用于巡查个节点的健康状态。
        /// </summary>
        [Description("监察程序")]
        Monitor = 3
    }
}
