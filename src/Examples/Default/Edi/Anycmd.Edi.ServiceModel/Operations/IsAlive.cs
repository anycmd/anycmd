
namespace Anycmd.Edi.ServiceModel.Operations
{
    using DataContracts;
    using ServiceStack;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据传输模型：请求服务端告诉我服务端服务的可用状态
    /// </summary>
    [DataContract]
    [Route("/IsAlive")]
    [Route("/AnyIsAlive")]
    public sealed class IsAlive : IReturn<IsAliveResponse>, IDto
    {
        private string _version = "v1";

        /// <summary>
        /// 版本标识。取值：v1
        /// </summary>
        [DataMember(Order = 1)]
        [ApiMember(Description = "版本标识。取值：v1", IsRequired = true)]
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
    }

    /// <summary>
    /// 数据传输模型：服务端服务状态的响应模型。
    /// </summary>
    [DataContract]
    public sealed class IsAliveResponse : IBeatResponse
    {
        private static readonly string HostName = System.Net.Dns.GetHostName();
        private string _serverId;
        private long _serverTicks = DateTime.UtcNow.Ticks;

        /// <summary>
        /// 
        /// </summary>
        public IsAliveResponse()
        {
            this.IsAlive = true;
        }

        /// <summary>
        /// 命令状态码。
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// 原因短语。
        /// </summary>
        [DataMember]
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// 命令状态码描述。
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 服务器响应时间戳
        /// </summary>
        [DataMember]
        public long ServerTicks
        {
            get { return _serverTicks; }
            set { _serverTicks = value; }
        }

        /// <summary>
        /// 服务器标识
        /// </summary>
        [DataMember]
        public string ServerId
        {
            get
            {
                return _serverId ?? HostName;
            }
            set
            {
                _serverId = value;
            }
        }
        /// <summary>
        /// 是否服务中
        /// </summary>
        [DataMember]
        public bool IsAlive { get; set; }
    }
}
