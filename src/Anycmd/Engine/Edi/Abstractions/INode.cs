
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;

    /// <summary>
    /// 定义数据交换节点实体。
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// 业务节点标识。这是“我节点”给远端节点分配的标识。
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 编码
        /// </summary>
        string Code { get; }
        /// <summary>
        /// 
        /// </summary>
        string Actions { get; }
        /// <summary>
        /// 业务说明
        /// </summary>
        string Abstract { get; }
        /// <summary>
        /// 接入单位名称
        /// </summary>
        string Catalog { get; }
        /// <summary>
        /// 专员
        /// </summary>
        string Steward { get; }
        /// <summary>
        /// 固定电话
        /// </summary>
        string Telephone { get; }
        /// <summary>
        /// 电子信箱
        /// </summary>
        string Email { get; }
        /// <summary>
        /// 手机号码
        /// </summary>
        string Mobile { get; }
        /// <summary>
        /// QQ
        /// </summary>
        string Qq { get; }
        /// <summary>
        /// 图标
        /// </summary>
        string Icon { get; }
        /// <summary>
        /// 有效标记
        /// </summary>
        int IsEnabled { get; }
        /// <summary>
        /// 命令执行开关
        /// </summary>
        bool IsExecuteEnabled { get; }
        /// <summary>
        /// 命令生产开关
        /// </summary>
        bool IsProduceEnabled { get; }
        /// <summary>
        /// 命令接收开关
        /// </summary>
        bool IsReceiveEnabled { get; }
        /// <summary>
        /// 命令转移开关
        /// </summary>
        bool IsDistributeEnabled { get; }
        /// <summary>
        /// 发送策略
        /// </summary>
        Guid TransferId { get; }
        /// <summary>
        /// WebApi服务基地址
        /// </summary>
        string AnycmdApiAddress { get; }
        /// <summary>
        /// 
        /// </summary>
        string AnycmdWsAddress { get; }
        /// <summary>
        /// 
        /// </summary>
        int? BeatPeriod { get; }
        /// <summary>
        /// 公钥
        /// </summary>
        string PublicKey { get; }
        /// <summary>
        /// 安全码
        /// </summary>
        string SecretKey { get; }
        /// <summary>
        /// 排序
        /// </summary>
        int SortCode { get; }
        /// <summary>
        /// 创建事件
        /// </summary>
        DateTime? CreateOn { get; }
    }
}
