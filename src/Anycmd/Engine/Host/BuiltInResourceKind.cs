
namespace Anycmd.Engine.Host
{
    using System.ComponentModel;

    /// <summary>
    /// 资源。一条命令的整个生命周期中涉及很多资源的参与，比如如果某个命令需要审核的话那么“审计系统”就会参与进来，
    /// 审计系统是什么？审计系统就可以抽象为资源。有时需要记录命令的整个生命周期中所有参与进来的资源，
    /// 记录下“什么资源，什么时间参与进来的，处理的结果是什么等信息”记录下的这些信息到底有什么用？用来打点命令的处理路径，比如可以用来调试。
    /// </summary>
    public enum BuiltInResourceKind : short
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义的资源类型")]
        Invalid = 0,
        /// <summary>
        /// Hecp处理程序
        /// </summary>
        [Description("Hecp处理程序")]
        HecpHandler = 1,
        /// <summary>
        /// 证书验证器
        /// </summary>
        [Description("证书认证者")]
        Authenticator = 2,
        /// <summary>
        /// 本体元素规约
        /// </summary>
        [Description("信息验证器")]
        InfoCheck = 12,
        /// <summary>
        /// 命令转移器
        /// </summary>
        [Description("命令转移器")]
        CommandTransfer = 13,
        /// <summary>
        /// 待分发命令建造器
        /// </summary>
        [Description("命令建造器")]
        CommandBuilder = 16,
        /// <summary>
        /// 待分发命令工厂
        /// </summary>
        [Description("命令工厂")]
        CommandFactory = 17,
        /// <summary>
        /// 命令执行策略
        /// </summary>
        [Description("命令执行策略")]
        ExecuteStrategy = 20,
        /// <summary>
        /// 命令提供程序
        /// </summary>
        [Description("命令提供程序")]
        CommandDbProvider = 22,
        /// <summary>
        /// 实体提供程序
        /// </summary>
        [Description("实体提供程序")]
        EntityDbProvider = 26,
        /// <summary>
        /// 本体库
        /// </summary>
        [Description("本体数据库")]
        OntologyDb = 27,
        /// <summary>
        /// 插件
        /// </summary>
        [Description("插件")]
        Plugin = 28
    }
}
