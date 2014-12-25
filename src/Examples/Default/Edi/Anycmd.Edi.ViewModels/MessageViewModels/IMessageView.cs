
namespace Anycmd.Edi.ViewModels.MessageViewModels
{
    using System;

    /// <summary>
    /// 定义命令展示模型的接口
    /// </summary>
    public interface IMessageView
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsSelf { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsCenter { get; }

        /// <summary>
        /// 
        /// </summary>
        string MessageId { get; }

        /// <summary>
        /// 
        /// </summary>
        string MessageType { get; }

        /// <summary>
        /// 
        /// </summary>
        string ClientType { get; }

        /// <summary>
        /// 节点标识
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// 动作类型
        /// </summary>
        string Verb { get; }

        /// <summary>
        /// 信息格式
        /// </summary>
        string InfoFormat { get; }

        /// <summary>
        /// 信息值字符串
        /// </summary>
        string InfoValue { get; }

        /// <summary>
        /// 本体码
        /// </summary>
        string Ontology { get; }

        /// <summary>
        /// 信息标识字符串
        /// </summary>
        string InfoId { get; }

        /// <summary>
        /// 本地实体标识
        /// </summary>
        string LocalEntityId { get; }

        /// <summary>
        /// 命令发起人
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTime ReceivedOn { get; }

        /// <summary>
        /// 命令创建时间
        /// </summary>
        DateTime CreateOn { get; }

        int StateCode { get; }

        string ReasonPhrase { get; }

        string Description { get; }
    }
}
