
namespace Anycmd.Engine.Hecp
{
    using DataContracts;
    using System;

    /// <summary>
    /// 心跳响应模型
    /// </summary>
    public sealed class BeatResponse : IBeatResponse
    {
        public BeatResponse() { }

        public void SetData(IBeatResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            this.IsAlive = response.IsAlive;
            this.Status = response.Status;
            this.ReasonPhrase = response.ReasonPhrase;
            this.Description = response.Description;
        }

        /// <summary>
        /// 是否服务中
        /// </summary>
        public bool IsAlive { get; private set; }

        /// <summary>
        /// 命令状态码
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// 原因短语
        /// </summary>
        public string ReasonPhrase { get; private set; }

        /// <summary>
        /// 命令状态码描述
        /// </summary>
        public string Description { get; private set; }
    }
}
