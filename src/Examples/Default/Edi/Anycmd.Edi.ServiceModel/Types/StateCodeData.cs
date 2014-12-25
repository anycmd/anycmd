
namespace Anycmd.Edi.ServiceModel.Types
{
    using DataContracts;
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据传输模型：数据交换协议状态码
    /// </summary>
    [DataContract]
    public class StateCodeData : IDto
    {
        /// <summary>
        /// 
        /// </summary>
        public StateCodeData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 10)]
        public int StateCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 20)]
        public string ReasonPhrase { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 30)]
        public string Description { get; set; }
    }
}