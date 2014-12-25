
namespace Anycmd.DataContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class FromData : IDto
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReplyTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string FaultTo { get; set; }
    }
}
