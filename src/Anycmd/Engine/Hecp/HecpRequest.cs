
namespace Anycmd.Engine.Hecp
{
    using DataContracts;
    using Exceptions;
    using Info;
    using System;
    using System.Linq;

    /// <summary>
    /// 包含单条命令的Edi请求
    /// </summary>
    public sealed class HecpRequest
    {
        private readonly Verb _verb;
        private readonly string _eventSourceType;
        private readonly string _eventSubject;
        private readonly int _eventStatus;
        private readonly string _eventReasonPhrase;
        private readonly IMessageDto _message;
        private readonly DataItem[] _infoId;
        private readonly DataItem[] _infoValue;
        private readonly string[] _queryList;
        private readonly IAcDomain _acDomain;

        private HecpRequest(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        private HecpRequest(IAcDomain acDomain, IMessageDto cmdDto)
            : this(acDomain)
        {
            if (cmdDto == null)
            {
                throw new ArgumentNullException("cmdDto");
            }
            if (cmdDto.Body == null)
            {
                throw new AnycmdException();
            }
            this._message = cmdDto;
            if (cmdDto.Body.InfoId == null)
            {
                _infoId = new DataItem[0];
            }
            if (cmdDto.Body.InfoValue == null)
            {
                _infoValue = new DataItem[0];
            }
            if (cmdDto.Body.InfoId != null)
            {
                _infoId = cmdDto.Body.InfoId.Where(a => a != null).Select(a => new DataItem(a.Key, a.Value)).ToArray();
            }
            if (cmdDto.Body.InfoValue != null)
            {
                _infoValue = cmdDto.Body.InfoValue.Where(a => a != null).Select(a => new DataItem(a.Key, a.Value)).ToArray();
            }
            this._queryList = cmdDto.Body.QueryList;
            this.Credential = new CredentialObject(cmdDto.Credential);
            this._verb = new Verb(cmdDto.Verb);
            if (cmdDto.Body.Event == null) return;
            _eventSourceType = cmdDto.Body.Event.SourceType;
            _eventSubject = cmdDto.Body.Event.Subject;
            _eventStatus = cmdDto.Body.Event.Status;
            _eventReasonPhrase = cmdDto.Body.Event.ReasonPhrase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="cmdDto"></param>
        /// <returns></returns>
        public static HecpRequest Create(IAcDomain acDomain, IMessageDto cmdDto)
        {
            return cmdDto == null ? null : new HecpRequest(acDomain, cmdDto);
        }

        public IAcDomain Host
        {
            get { return _acDomain; }
        }

        /// <summary>
        /// 证书
        /// </summary>
        public CredentialObject Credential { get; private set; }

        /// <summary>
        /// ApiVersion
        /// </summary>
        public string Version { get { return this._message.Version; } }

        /// <summary>
        /// 是否是哑的
        /// </summary>
        public bool IsDumb { get { return this._message.IsDumb; } }

        /// <summary>
        /// 
        /// </summary>
        public string MessageType { get { return this._message.MessageType; } }

        /// <summary>
        /// 动作类型
        /// </summary>
        public Verb Verb { get { return this._verb; } }

        /// <summary>
        /// 本体码
        /// </summary>
        public string Ontology { get { return this._message.Ontology; } }

        /// <summary>
        /// 客户端生成的时间戳。根据消息类型的不同其为Event时间戳或Command时间戳或Action时间戳。
        /// <remarks>
        /// 注意：Message是来自客户端的消息，因此TimeStamp指的是客户端传入的时间戳。
        /// 对于Event该时间是事件在客户端发生的时间而对于Action和Command该时间戳的意义由客户端自由定义
        /// </remarks>
        /// </summary>
        public Int64 TimeStamp { get { return this._message.TimeStamp; } }

        /// <summary>
        /// 本地请求标识
        /// </summary>
        public string MessageId { get { return this._message.MessageId; } }

        /// <summary>
        /// 
        /// </summary>
        public FromData From { get { return this._message.From; } }

        /// <summary>
        /// 
        /// </summary>
        public string RelatesTo { get { return this._message.RelatesTo; } }

        /// <summary>
        /// 
        /// </summary>
        public string SessionId { get { return this._message.SessionId; } }

        /// <summary>
        /// 
        /// </summary>
        public string To { get { return this._message.To; } }

        /// <summary>
        /// 事件主题码
        /// </summary>
        public string EventSubject { get { return _eventSubject; } }

        /// <summary>
        /// 事件源类型
        /// </summary>
        public string EventSourceType { get { return _eventSourceType; } }

        /// <summary>
        /// 事件状态码
        /// </summary>
        public int EventStatus { get { return _eventStatus; } }

        /// <summary>
        /// 事件原因短语
        /// </summary>
        public string EventReasonPhrase { get { return _eventReasonPhrase; } }

        /// <summary>
        /// 信息标识
        /// </summary>
        public DataItem[] InfoId { get { return _infoId; } }

        /// <summary>
        /// 信息值
        /// </summary>
        public DataItem[] InfoValue { get { return _infoValue; } }

        public string[] QueryList { get { return _queryList; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public string ToOrignalString(ICredentialData credential)
        {
            return this._message.ToOrignalString(credential);
        }
    }
}
