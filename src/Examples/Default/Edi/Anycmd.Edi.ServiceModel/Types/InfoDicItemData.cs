
namespace Anycmd.Edi.ServiceModel.Types
{
    using DataContracts;
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据传输模型：信息字典项
    /// </summary>
    [DataContract]
    public sealed class InfoDicItemData : IDto
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 10)]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 20)]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 30)]
        public string Level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 40)]
        public string Description { get; set; }
    }
}
