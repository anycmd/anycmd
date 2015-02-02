
namespace Anycmd.Engine.Host
{
    using Ac;
    using Ac.Infra;

    /// <summary>
    /// 应用配置。
    /// </summary>
    public interface IAppConfig
    {
        string CurrentAcSessionCacheKey { get; }
        bool EnableClientCache { get; }
        bool EnableOperationLog { get; }
        string SelfAppSystemCode { get; }
        string SqlServerTableColumnsSelect { get; }
        string SqlServerTablesSelect { get; }
        string SqlServerViewColumnsSelect { get; }
        string SqlServerViewsSelect { get; }
        int TicksTimeout { get; }

        /// <summary>
        /// 序列标识生成器类型标识字符串
        /// </summary>
        string SequenceIdGenerator { get; }

        /// <summary>
        /// 标识生成器类型标识字符串
        /// </summary>
        string IdGenerator { get; }

        /// <summary>
        /// 命令的InfoID和InfoValue字符串的格式。该格式是命令持久化所使用的默认格式。
        /// </summary>
        string InfoFormat { get; }

        /// <summary>
        /// 实体库归档路径
        /// </summary>
        string EntityArchivePath { get; }

        /// <summary>
        /// 实体库备份路径
        /// </summary>
        string EntityBackupPath { get; }

        /// <summary>
        /// 服务是否可用
        /// </summary>
        bool ServiceIsAlive { get; }

        /// <summary>
        /// 是否开启Trace
        /// </summary>
        bool TraceIsEnabled { get; }

        /// <summary>
        /// 检测节点是否在线的请求周期单位（分钟）
        /// </summary>
        int BeatPeriod { get; }

        /// <summary>
        /// 中心节点标识
        /// </summary>
        string CenterNodeId { get; }

        /// <summary>
        /// 本节点自我标识
        /// </summary>
        string ThisNodeId { get; }

        /// <summary>
        /// 审核配置深度
        /// </summary>
        ConfigLevel AuditLevel { get; }

        /// <summary>
        /// 隐式审核意为
        /// </summary>
        AuditType ImplicitAudit { get; }

        /// <summary>
        /// 访问控制配置深度
        /// </summary>
        ConfigLevel AclLevel { get; }

        /// <summary>
        /// 隐式访问控制意为
        /// </summary>
        AllowType ImplicitAllow { get; }

        /// <summary>
        /// 实体登录控制配置深度
        /// </summary>
        ConfigLevel EntityLogonLevel { get; }

        /// <summary>
        /// 隐式实体登录意为
        /// </summary>
        EntityLogon ImplicitEntityLogon { get; }
    }
}
