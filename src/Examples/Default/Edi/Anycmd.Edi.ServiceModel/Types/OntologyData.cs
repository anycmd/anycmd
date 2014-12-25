
namespace Anycmd.Edi.ServiceModel.Types
{
    using DataContracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据传输模型：本体的基本信息。包括：名称、编码、本体元素。
    /// </summary>
    [DataContract]
    public sealed class OntologyData : IDto
    {
        public OntologyData()
        {
            this.Actions = new List<ActionData>();
            this.Elements = new List<ElementData>();
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
        /// 是否是系统本体
        /// </summary>
        [DataMember(Order = 30)]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 50)]
        public List<ActionData> Actions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 200)]
        public List<ElementData> Elements { get; set; }
    }
}