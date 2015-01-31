
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using DataContracts;
    using Exceptions;
    using Hecp;
    using Info;
    using System;
    using Util;

    /// <summary>
    /// 服务端命令基类。<see cref="IMessage"/>
    /// </summary>
    public abstract class MessageBase : IMessage
    {
        #region Ctor
        /// <summary>
        /// 构建给定类型的命令。
        /// </summary>
        /// <param name="type">命令类型</param>
        /// <param name="dataTuple">数据项集合对</param>
        /// <param name="id">
        /// 命令标识。
        /// <remarks>
        /// 命令标识在命令的整个生命周期内是不改变的。命令标识在服务端命令创生时生成之后无论命令处在什么状态它的标识都是不变的。
        /// 借助数据库层来说明这个问题：命令从ReceivedMessage表移到ExecutedMessage表的时候主键是不变的。
        /// </remarks>
        /// </param>
        protected MessageBase(MessageTypeKind type, Guid id, DataItemsTuple dataTuple)
        {
            if (type == MessageTypeKind.Invalid)
            {
                throw new AnycmdException("非法的命令类型");
            }
            if (id == Guid.Empty)
            {
                throw new AnycmdException("命令Id不能为空Guid");
            }
            if (dataTuple == null)
            {
                throw new ArgumentNullException("dataTuple");
            }
            Version = ApiVersion.V1.ToName();
            this.CommandType = type;
            this.Id = id;
            this.DataTuple = dataTuple;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 协议版本号
        /// </summary>
        public string Version { get; protected internal set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public MessageTypeKind CommandType { get; private set; }

        /// <summary>
        /// 命令标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 是否是哑的
        /// </summary>
        public bool IsDumb { get; protected internal set; }

        /// <summary>
        /// 请求类型
        /// </summary>
        public MessageType MessageType { get; protected internal set; }

        /// <summary>
        /// 远端命令标识
        /// </summary>
        public string MessageId { get; protected internal set; }

        public string RelatesTo { get; protected internal set; }

        public string To { get; protected internal set; }

        public string SessionId { get; protected internal set; }

        public FromData From { get; protected internal set; }

        /// <summary>
        /// 本体实体标识
        /// </summary>
        public string LocalEntityId { get; protected internal set; }

        /// <summary>
        /// 本地目录码
        /// </summary>
        public string CatalogCode { get; protected internal set; }

        /// <summary>
        /// 客户端类型
        /// </summary>
        public ClientType ClientType { get; protected internal set; }

        /// <summary>
        /// 本体码
        /// </summary>
        public string Ontology { get; protected internal set; }

        /// <summary>
        /// 动作类型
        /// </summary>
        public Verb Verb { get; protected internal set; }

        /// <summary>
        /// 节点标识
        /// </summary>
        public string ClientId { get; protected internal set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ReceivedOn { get; protected internal set; }

        /// <summary>
        /// 命令时间戳
        /// </summary>
        public DateTime TimeStamp { get; protected internal set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime CreateOn { get; protected internal set; }

        /// <summary>
        /// 命令状态码
        /// </summary>
        public int Status { get; protected internal set; }

        /// <summary>
        /// 原因短语
        /// </summary>
        public string ReasonPhrase { get; protected internal set; }

        /// <summary>
        /// 命令状态描述
        /// </summary>
        public string Description { get; protected internal set; }

        /// <summary>
        /// 事件主题
        /// </summary>
        public string EventSubjectCode { get; protected internal set; }

        /// <summary>
        /// 其值为枚举字符串
        /// </summary>
        public string EventSourceType { get; protected internal set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public string UserName { get; protected internal set; }

        /// <summary>
        /// 数据项集合对
        /// </summary>
        public DataItemsTuple DataTuple { get; private set; }
        #endregion

        /// <summary>
        /// 转化为已成功接收的命令
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToReceived(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.Received, result);
        }

        /// <summary>
        /// 转化为接收失败的命令
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToUnaccepted(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.Unaccepted, result);
        }

        /// <summary>
        /// 转化为已成功执行的命令
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToExecuted(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.Executed, result);
        }

        /// <summary>
        /// 转化为执行失败的命令
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToExecuteFailing(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.ExecuteFailing, result);
        }

        /// <summary>
        /// 转化为待分发的命令
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToDistribute(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.Distribute, result);
        }

        /// <summary>
        /// 转化为已成功分发的命令
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToDistributed(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.Distributed, result);
        }

        /// <summary>
        /// 转化为分发失败的命令
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToDistributeFailing(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.DistributeFailing, result);
        }

        /// <summary>
        /// 转化为本地事件
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToLocalEvent(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.LocalEvent, result);
        }

        /// <summary>
        /// 转化为客户端事件
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal MessageEntity ToClientEvent(QueryResult result)
        {
            return this.ToEntity(MessageTypeKind.ClientEvent, result);
        }

        private MessageEntity ToEntity(MessageTypeKind commandType, QueryResult result)
        {
            if (commandType == MessageTypeKind.Invalid || commandType == MessageTypeKind.AnyCommand)
            {
                throw new AnycmdException("不能转化为Invalid或AnyCommand类型命令");
            }
            return new MessageEntity(commandType, this.Id, this.DataTuple)
            {
                Status = result.Status,
                ReasonPhrase = result.ReasonPhrase,
                Description = result.Description,
                CreateOn = DateTime.Now,
                Version = this.Version,
                IsDumb = this.IsDumb,
                Verb = this.Verb,
                ClientId = this.ClientId,
                ClientType = this.ClientType,
                EventSourceType = this.EventSourceType,
                EventSubjectCode = this.EventSubjectCode,
                UserName = this.UserName,
                Ontology = this.Ontology,
                ReceivedOn = this.ReceivedOn,
                MessageId = this.MessageId,
                MessageType = this.MessageType,
                LocalEntityId = this.LocalEntityId,
                CatalogCode = this.CatalogCode,
                TimeStamp = this.TimeStamp,
                From = this.From,
                RelatesTo = this.RelatesTo,
                SessionId = this.SessionId,
                To = this.To
            };
        }
    }
}
