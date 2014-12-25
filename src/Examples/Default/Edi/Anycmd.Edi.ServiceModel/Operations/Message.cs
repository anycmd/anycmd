
namespace Anycmd.Edi.ServiceModel.Operations
{
    using DataContracts;
    using ServiceStack;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 操作单条实体
    /// </summary>
    [DataContract]
    [Route("/Any")]
    [Api("单条语句。")]
    public sealed class Message : IMessageDto, IReturn<Message>, IDto
    {
        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        public Message()
        {
            this.Body = new BodyData();
            this.Credential = new CredentialData();
        }
        #endregion

        /// <summary>
        /// 版本标识。取值：v1
        /// </summary>
        [DataMember(Order = 10)]
        [ApiMember(Description = "版本标识。取值：v1", IsRequired = true)]
        public string Version { get; set; }

        /// <summary>
        /// 请求类型。必须是Action或Command或Event。Action接收立即执行，Command接收后周期执行，
        /// 如何处理Event由服务端自定。面向EventSourceType、EventSubjectCode、EventStateCode编程。
        /// </summary>
        [DataMember(Order = 11)]
        [ApiMember(Description =
@"消息类型。必须是Action或Command或Event。Action接收立即执行，Command何时执行由服务端自定但必须执行，
如何处理Event由服务端自定，面向EventSourceType、EventSubjectCode、EventStateCode编程。",
            IsRequired = true)]
        public string MessageType { get; set; }

        /// <summary>
        /// 本地请求标识。当Type为Event且EventSourceType为Command时是远端节点向我分发命令时使用的MessageID。
        /// </summary>
        [DataMember(Order = 12)]
        [ApiMember(Description =
@"请求标识。当Type为Event且EventSourceType为Command时是远端节点向我分发命令时使用的MessageID。",
            IsRequired = true)]
        public string MessageId { get; set; }

        /// <summary>
        /// 关联消息。比如，回复消息关联被回复消息
        /// </summary>
        [DataMember(Order = 13)]
        [ApiMember(Description = "关联消息。比如，回复消息关联被回复消息", IsRequired = false)]
        public string RelatesTo { get; set; }

        /// <summary>
        /// 消息目标接收节点的地址
        /// </summary>
        [DataMember(Order = 14)]
        [ApiMember(Description = "消息目标接收节点的地址", IsRequired = true)]
        public string To { get; set; }

        /// <summary>
        /// 表示消息的同一次会话的标识，在同一次会话中可以有多条消息进行一系列操作。
        /// </summary>
        [DataMember(Order = 15)]
        [ApiMember(Description = "表示消息的同一次会话的标识，在同一次会话中可以有多条消息进行一系列操作。", IsRequired = false)]
        public string SessionId { get; set; }

        /// <summary>
        /// 是否是哑的。默认为False。True表示是哑的，False表示不是哑的。哑命令在服务端不执行，
        /// 所以哑命令不会影响服务端实体的状态，但哑命令会如正常命令一样经过相应的非哑命令的
        /// 管道路径，从而客户节点程序员可以通过发送哑命令的方式测试响应的非哑命令。
        /// </summary>
        [DataMember(Order = 16)]
        [ApiMember(DataType = "Boolean", IsRequired = false, Description =
@"是否是哑的。默认为False。True表示是哑的，False表示不是哑的。哑命令不会影响服务端实体的状态，
但哑命令与对应的非哑命令一样经过相同的流程路径。客户节点程序员可以通过发送哑命令的方式测试对应的非哑命令。")]
        public bool IsDumb { get; set; }

        /// <summary>
        /// 本体码。教师对应JS，学生对应XS，测试对应JSTest。
        /// </summary>
        [DataMember(Order = 20)]
        [ApiMember(Description = "本体码。教师对应JS，学生对应XS，测试对应JSTest。", IsRequired = true)]
        public string Ontology { get; set; }

        /// <summary>
        /// 动作码。教师、学生两个本体的动作码巧合相同。目前均是Create、Update、Delete、Get、Head五个取值。
        /// </summary>
        [DataMember(Order = 30)]
        [ApiMember(
            Description =
@"动作码。教师、学生两个本体的动作码巧合相同。目前均是Create、Update、Delete、Get、Head五个取值。",
            IsRequired = true)]
        public string Verb { get; set; }

        /// <summary>
        /// 客户端生成的时间戳。根据消息类型的不同其为Event时间戳或Command时间戳或Action时间戳。
        /// <remarks>
        /// 对于Event该时间是事件在客户端发生的时间而对于Action和Command该时间戳的意义由客户端自由定义
        /// </remarks>
        /// </summary>
        [DataMember(Order = 31)]
        [ApiMember(
            Description =
@"命令时间戳。对于Event该时间是事件在客户端发生的时间（请校准时钟）而对于Action和Command该时间戳的意义由客户端自由定义。",
            IsRequired = true)]
        public Int64 TimeStamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 200)]
        [ApiMember(Description = @"输入。",
            DataType = "BodyData",
            IsRequired = false)]
        public BodyData Body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 210)]
        [ApiMember(Description = @"消息来源。",
            DataType = "FromData",
            IsRequired = false)]
        public FromData From { get; set; }

        /// <summary>
        /// 证书。证书对象在结构上有CredentialType、ClientType、ClientID、UserType、UserName、Password、Ticks七个属性，它们用于验证节点身份。
        /// </summary>
        [DataMember(Order = 300)]
        [ApiMember(Description =
@"证书。证书对象在结构上有CredentialType、ClientType、ClientID、UserType、UserName、Password、Ticks七个属性，它们用于验证节点身份。",
            DataType = "CredentialData",
            IsRequired = true)]
        public CredentialData Credential { get; set; }
    }
}
