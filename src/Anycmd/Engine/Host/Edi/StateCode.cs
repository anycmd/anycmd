
namespace Anycmd.Engine.Host.Edi
{
    /// <summary>
    /// 状态码文档。该表的Code和ReasonPhrase字段由上下文自动维护，Description由人工维护。
    /// </summary>
    public sealed class StateCode
    {
        public StateCode(int code, string reasonPhrase, string description)
        {
            this.Code = code;
            this.ReasonPhrase = reasonPhrase;
            this.Description = description;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 编码
        /// </summary>
        public int Code { get; private set; }
        /// <summary>
        /// 原因短语
        /// </summary>
        public string ReasonPhrase { get; private set; }
    }
}
