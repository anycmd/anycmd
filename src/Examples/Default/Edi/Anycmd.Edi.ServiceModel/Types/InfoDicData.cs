
namespace Anycmd.Edi.ServiceModel.Types
{
    using DataContracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据传输模型：信息字典
    /// </summary>
    [DataContract]
    public sealed class InfoDicData : IDto
    {
        public InfoDicData()
        {
            this.InfoDicItems = new List<InfoDicItemData>();
        }

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
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 40)]
        public List<InfoDicItemData> InfoDicItems { get; set; }
    }
}
