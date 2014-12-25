
namespace Anycmd.Engine.Host.Edi
{
    using DataContracts;
    using Handlers;
    using Info;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Util;

    /// <summary>
    /// 命令处理结果
    /// </summary>
    public sealed class QueryResult : IMessageDto
    {
        private readonly IMessage _request;
        private readonly CredentialData _credential = new CredentialData();
        private readonly EventData _evnt = new EventData
        {
            SourceType = EventSourceType.Command.ToName(),
            Subject = "Response",
            Status = 500,
            ReasonPhrase = "InternalServerError",
            Description = "服务逻辑异常"
        };

        /// <summary>
        /// 构造命令处理结果对象。
        /// </summary>
        internal QueryResult(IMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            this._request = request;
            this.Version = request.Version;
            this.IsDumb = request.IsDumb;
            this.MessageType = "Event";
            this.Ontology = request.Ontology;
            this.TimeStamp = DateTime.UtcNow.Ticks;
            this.MessageId = request.MessageId;
            this._credential = new CredentialData();
            this.ResultDataItems = new List<DataItem>();
        }

        /// <summary>
        /// 当请求命令的Verb为get时有值。
        /// </summary>
        internal List<DataItem> ResultDataItems { get; set; }

        /// <summary>
        /// 命令在服务端的管道栈
        /// </summary>
        public string StackTrace { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                var value = (int)this._evnt.Status;
                return value >= 200 && value < 300;
            }
        }

        /// <summary>
        /// 查看或设置命令响应流程的关闭状态。设置为false以跳出命令响应过滤器的应用。
        /// </summary>
        public bool IsClosed { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDumb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string MessageType { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Verb { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ontology { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public long TimeStamp { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string MessageId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            get
            {
                return this._evnt.Status;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ReasonPhrase
        {
            get
            {
                return this._evnt.ReasonPhrase;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return this._evnt.Description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RelatesTo
        {
            get { return this._request.RelatesTo; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string To
        {
            get { return this._request.To; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SessionId
        {
            get { return this._request.SessionId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public FromData From
        {
            get { return this._request.From; }
        }

        /// <summary>
        /// 
        /// </summary>
        CredentialData IMessageDto.Credential
        {
            get
            {
                return _credential;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        BodyData IMessageDto.Body
        {
            get
            {
                return new BodyData(
                    _request.DataTuple.IdItems.Items.Select(a => new KeyValue(a.Key, a.Value)).ToArray(),
                    this.ResultDataItems.Select(a => new KeyValue(a.Key, a.Value)).ToArray())
                    {
                        Event = this._evnt
                    };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="description"></param>
        internal void UpdateStatus(Status status, string description)
        {
            this._evnt.Status = (int)status;
            this._evnt.ReasonPhrase = status.ToName();
            this._evnt.Description = description;
        }
    }
}
