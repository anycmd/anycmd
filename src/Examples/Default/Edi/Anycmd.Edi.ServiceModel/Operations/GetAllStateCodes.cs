
namespace Anycmd.Edi.ServiceModel.Operations
{
    using DataContracts;
    using ServiceStack;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Types;

    /// <summary>
    /// 数据传输模型：请求获取全部数据交换协议状态码的请求模型
    /// </summary>
    [DataContract]
    [Route("/StateCode/GetAll")]
    [Api("全部状态码。")]
    public class GetAllStateCodes : IReturn<GetAllStateCodesResponse>, IDto
    {
        private string _version = "v1";

        /// <summary>
        /// 
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
    /// 数据传输模型：请求获取全部数据交换协议状态码的响应模型
    /// </summary>
    [DataContract]
    public class GetAllStateCodesResponse
    {
        private static readonly string HostName = System.Net.Dns.GetHostName();
        private string _serverId;
        private long _serverTicks = DateTime.UtcNow.Ticks;

        public GetAllStateCodesResponse()
        {
            StateCodes = new List<StateCodeData>();
            Description = "数据交换协议状态码表";
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
        public List<StateCodeData> StateCodes { get; set; }
    }
}
