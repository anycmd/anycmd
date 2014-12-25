
namespace Anycmd.Edi.ServiceModel.Types
{
    using DataContracts;
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据传输模型：本体动作。
    /// </summary>
    [DataContract]
    public class ActionData : IDto
    {
        public ActionData() { }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 10)]
        public string Verb { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 20)]
        public string Name { get; set; }
    }
}
