
namespace Anycmd.Edi.ServiceModel.Operations
{
    using DataContracts;
    using ServiceStack;
    using System;
    using System.Runtime.Serialization;
    using Types;

    /// <summary>
    /// 数据传输模型：请求获取单个字典的请求模型
    /// </summary>
    [DataContract]
    [Route("/InfoDic/Get/{DicCode}")]
    [Route("/InfoDic/Get")]
    [Api("单个字典。")]
    public sealed class GetInfoDic : IReturn<GetInfoDicResponse>, IDto
    {
        /// <summary>
        /// 字典码
        /// </summary>
        [DataMember]
        [ApiMember(Description = "字典码", IsRequired = true)]
        public string DicCode { get; set; }
    }

    /// <summary>
    /// 数据传输模型：请求获取单个字典的响应模型
    /// </summary>
    [DataContract]
    public sealed class GetInfoDicResponse
    {
        private static readonly string HostName = System.Net.Dns.GetHostName();
        private string _serverId;
        private long _serverTicks = DateTime.UtcNow.Ticks;

        public GetInfoDicResponse()
        {
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
        public InfoDicData InfoDic { get; set; }
    }
}
