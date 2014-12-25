
namespace Anycmd.Edi.ServiceModel.Operations
{
    using DataContracts;
    using ServiceStack;
    using System;
    using System.Runtime.Serialization;
    using Types;

    /// <summary>
    /// 数据传输模型：请求获取单个本体的请求模型
    /// </summary>
    [Route("/Ontology/Get/{OntologyCode}")]
    [Route("/Ontology/Get")]
    [Api("单个本体。")]
    [DataContract]
    public sealed class GetOntology : IReturn<GetOntologyResponse>, IDto
    {
        /// <summary>
        /// 本体码
        /// </summary>
        [DataMember]
        [ApiMember(Description = "本体码", IsRequired = true)]
        public string OntologyCode { get; set; }
    }

    /// <summary>
    /// 数据传输模型：请求获取单个本体的响应模型
    /// </summary>
    [DataContract]
    public sealed class GetOntologyResponse
    {
        private static readonly string HostName = System.Net.Dns.GetHostName();
        private string _serverId;
        private long _serverTicks = DateTime.UtcNow.Ticks;

        public GetOntologyResponse()
        {
            this.Description = "单个本体";
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
        /// 
        /// </summary>
        [DataMember]
        public OntologyData Ontology { get; set; }
    }
}
