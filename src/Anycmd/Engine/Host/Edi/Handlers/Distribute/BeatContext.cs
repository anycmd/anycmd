
namespace Anycmd.Engine.Host.Edi.Handlers.Distribute
{
    using Engine.Edi;
    using Hecp;
    using System;

    /// <summary>
    /// 服务心跳上下文。
    /// </summary>
    public sealed class BeatContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="request"></param>
        public BeatContext(NodeDescriptor node, BeatRequest request)
        {
            this.Node = node;
            this.Request = request;
            this.Response = new BeatResponse();
        }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NodeDescriptor Node { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public BeatRequest Request { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public BeatResponse Response { get; private set; }
    }
}
