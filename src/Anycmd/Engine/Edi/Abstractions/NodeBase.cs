
namespace Anycmd.Engine.Edi.Abstractions
{
    using Model;
    using System;

    public abstract class NodeBase : EntityBase, INode
    {
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        public int IncrementId { get; set; }
        public string EntityAction { get; set; }
        public string EntityElementAction { get; set; }
        public string EntityAclPolicy { get; set; }
        public string EntityJavascript { get; set; }
        public Guid CreateNodeId { get; set; }
        public Guid ModifiedNodeId { get; set; }
        public Guid? CreateCommandId { get;set;}
        
        public Guid? ModifiedCommandId { get; set; }
        
        public string Actions { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 命令执行开关
        /// </summary>
        public bool IsExecuteEnabled { get; set; }
        /// <summary>
        /// 命令生产开关
        /// </summary>
        public bool IsProduceEnabled { get; set; }
        /// <summary>
        /// 命令接收开关
        /// </summary>
        public bool IsReceiveEnabled { get; set; }
        /// <summary>
        /// 命令转移开关
        /// </summary>
        public bool IsDistributeEnabled { get; set; }
        /// <summary>
        /// 发送策略
        /// </summary>
        public Guid TransferId { get; set; }
        /// <summary>
        /// WebApi接口
        /// </summary>
        public string AnycmdApiAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AnycmdWsAddress { get; set; }
        public int? BeatPeriod { get; set; }
        /// <summary>
        /// 自我在该端的标识
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// 安全码
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 业务说明
        /// </summary>
        public string Abstract { get; set; }
        /// <summary>
        /// 接入单位名称
        /// </summary>
        public string Catalog { get; set; }
        /// <summary>
        /// 专员
        /// </summary>
        public string Steward { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string Qq { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public int DeletionStateCode { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
