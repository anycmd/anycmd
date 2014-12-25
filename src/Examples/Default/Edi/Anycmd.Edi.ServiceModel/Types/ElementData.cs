
namespace Anycmd.Edi.ServiceModel.Types
{
    using DataContracts;
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据传输模型：本体元素基本信息。包括名称、编码、数据项属性。
    /// </summary>
    [DataContract]
    public sealed class ElementData : IDto
    {
        /// <summary>
        /// 
        /// </summary>
        public ElementData()
        {
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
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 30)]
        public string OType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 40)]
        public int? MaxLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 50)]
        public bool Nullable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 80)]
        public string ValueDic { get; set; }

        [DataMember(Order = 90)]
        public int IsEnabled { get; set; }
    }
}
