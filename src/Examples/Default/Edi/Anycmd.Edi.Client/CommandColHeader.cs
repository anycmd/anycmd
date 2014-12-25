
namespace Anycmd.Edi.Client
{

    /// <summary>
    /// 列头模型
    /// </summary>
    public class CommandColHeader
    {
        /// <summary>
        /// $MessageId 请求标识 命令消息标识
        /// </summary>
        public const string MessageId = "$MessageId";
        /// <summary>
        /// $MessageType 请求类型
        /// </summary>
        public const string MessageType = "$MessageType";
        /// <summary>
        /// $EventSourceType 事件源类型
        /// </summary>
        public const string EventSourceType = "$EventSourceType";
        /// <summary>
        /// $EventSubjectCode 事件主题码
        /// </summary>
        public const string EventSubjectCode = "$EventSubjectCode";
        /// <summary>
        /// $EventStateCode 事件状态码
        /// </summary>
        public const string EventStateCode = "$EventStateCode";
        /// <summary>
        /// $EventReasonPhrase 时间原因短语
        /// </summary>
        public const string EventReasonPhrase = "$EventReasonPhrase";
        /// <summary>
        /// $Verb 本体动作码
        /// </summary>
        public const string Verb = "$Verb";
        /// <summary>
        /// $IsDumb 是否是哑命令
        /// </summary>
        public const string IsDumb = "$IsDumb";
        /// <summary>
        /// $TimeStamp 本体时间戳
        /// </summary>
        public const string TimeStamp = "$TimeStamp";
        /// <summary>
        /// $Ontology 本体码
        /// </summary>
        public const string Ontology = "$Ontology";
        /// <summary>
        /// $Version 协议版本号
        /// </summary>
        public const string Version = "$Version";
        /// <summary>
        /// $ServerTicks 服务器时间戳
        /// </summary>
        public const string ServerTicks = "$ServerTicks";
        /// <summary>
        /// $LocalEntityId 建议信息标识
        /// </summary>
        public const string LocalEntityId = "$LocalEntityId";
        /// <summary>
        /// $StateCode 响应状态码
        /// </summary>
        public const string StateCode = "$StateCode";
        /// <summary>
        /// $ReasonPhrase 响应原因短语
        /// </summary>
        public const string ReasonPhrase = "$ReasonPhrase";
        /// <summary>
        /// $Description 响应描述
        /// </summary>
        public const string Description = "$Description";
        /// <summary>
        /// $InfoIDKeys 信息标识键
        /// </summary>
        public const string InfoIdKeys = "$InfoIDKeys";
        /// <summary>
        /// $InfoValueKeys 信息值键
        /// </summary>
        public const string InfoValueKeys = "$InfoValueKeys";

        /// <summary>
        /// 列编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 列默认值
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// 是否隐藏该列。
        /// </summary>
        public bool IsHidden { get; set; }
    }
}
