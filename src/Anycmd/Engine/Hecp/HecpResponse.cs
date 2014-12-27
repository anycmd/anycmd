
namespace Anycmd.Engine.Hecp
{
    using DataContracts;
    using Host.Edi;
    using System;

    /// <summary>
    /// Hecp响应模型
    /// </summary>
    public sealed class HecpResponse : IMessageDto
    {
        private static readonly string HostName = System.Net.Dns.GetHostName();
        private string _serverId;
        private Int64 _serverTicks = DateTime.UtcNow.Ticks;
        private CredentialData _credential;
        private BodyData _body;

        /// <summary>
        /// 
        /// </summary>
        private HecpResponse()
        {
            this.MessageType = "Event";
            this._body = new BodyData();
            this._credential = new CredentialData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        private HecpResponse(string requestId)
            : this()
        {
            this.MessageId = requestId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public static HecpResponse Create(string requestId)
        {
            return new HecpResponse(requestId);
        }

        /// <summary>
        /// 
        /// </summary>
        public string MessageId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string RelatesTo { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string To { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public FromData From { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return this._body.Event.Status >= 200 && this._body.Event.Status < 300;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int64 ServerTicks
        {
            get { return _serverTicks; }
            private set { _serverTicks = value; }
        }

        /// <summary>
        /// 服务器标识
        /// </summary>
        public string ServerId
        {
            get
            {
                return _serverId ?? HostName;
            }
            private set
            {
                _serverId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StackTrace { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsClosed { get; private set; }

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
        public string MessageType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Verb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ontology { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public long TimeStamp { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public BodyData Body
        {
            get
            {
                return _body;
            }
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
        /// <param name="status"></param>
        /// <param name="description"></param>
        internal void UpdateStatus(Status status, string description)
        {
            this.Body.Event.Status = (int)status;
            this.Body.Event.ReasonPhrase = status.ToName();
            this.Body.Event.Description = description;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        internal void Fill(QueryResult result)
        {
            this.Verb = result.Verb;
            this._credential = ((IMessageDto)result).Credential;
            this._body = ((IMessageDto)result).Body;
            this.IsClosed = result.IsClosed;
            this.IsDumb = result.IsDumb;
            this.Ontology = result.Ontology;
            this.MessageId = result.MessageId;
            this.StackTrace = result.StackTrace;
            this.TimeStamp = result.TimeStamp;
            this.MessageType = result.MessageType;
            this.Version = result.Version;
            this.From = result.From;
            this.SessionId = result.SessionId;
            this.To = result.To;
            this.RelatesTo = result.RelatesTo;
        }
    }
}
