
namespace Anycmd.DataContracts
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IMessageDto : IDto
    {
        /// <summary>
        /// ApiVersion
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 是否是哑的
        /// </summary>
        bool IsDumb { get; }

        /// <summary>
        /// 消息类型
        /// </summary>
        string MessageType { get; }

        /// <summary>
        /// 动作类型
        /// </summary>
        string Verb { get; }

        /// <summary>
        /// 本体码
        /// </summary>
        string Ontology { get; }

        /// <summary>
        /// 客户端生成的时间戳。根据消息类型的不同其为Event时间戳或Command时间戳或Action时间戳。
        /// <remarks>
        /// 注意：Message是来自客户端的消息，因此TimeStamp指的是客户端传入的时间戳。
        /// 对于Event该时间是事件在客户端发生的时间而对于Action和Command该时间戳的意义由客户端自由定义
        /// </remarks>
        /// </summary>
        Int64 TimeStamp { get; }

        /// <summary>
        /// 消息标识
        /// </summary>
        string MessageId { get; }

        /// <summary>
        /// 是一对表达当前消息如何与其它消息关联的值。
        /// </summary>
        string RelatesTo { get; }

        /// <summary>
        /// 
        /// </summary>
        string To { get; }

        /// <summary>
        /// 
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// 
        /// </summary>
        FromData From { get; }

        /// <summary>
        /// 
        /// </summary>
        BodyData Body { get; }

        /// <summary>
        /// 证书
        /// </summary>
        CredentialData Credential { get; }
    }
}
