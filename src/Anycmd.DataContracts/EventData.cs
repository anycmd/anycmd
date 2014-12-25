
namespace Anycmd.DataContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据传输模型：事件头。
    /// </summary>
    [DataContract]
    public class EventData : IDto
    {
        /// <summary>
        /// 事件源类型。必须是Command或Entity。Command事件源类型的事件用以告诉远端节点它发送过来的命令在我端的处理状态。
        /// Entity事件源类型的事件用以告诉远端节点我端的实体发生了某个事件，我端的实体在远端有对应的实体。
        /// </summary>
        [DataMember(Order = 50)]
        public string SourceType { get; set; }

        /// <summary>
        /// 事件主题码。主题码为点号分割的层级结构。
        /// 编码为“StateCodeChanged”的主题基本包括命令的所有事件，而“StateCodeChanged.Audit”编码的是审核事件。
        /// </summary>
        [DataMember(Order = 51)]
        public string Subject { get; set; }

        /// <summary>
        /// 状态码。状态码参见《数据交换协议状态码表》。
        /// </summary>
        [DataMember(Order = 52)]
        public int Status { get; set; }

        /// <summary>
        /// 原因短语。状态码参见《数据交换协议状态码表》。
        /// </summary>
        [DataMember(Order = 53)]
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// 状态描述。
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
