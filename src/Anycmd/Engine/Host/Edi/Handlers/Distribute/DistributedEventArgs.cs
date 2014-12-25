
namespace Anycmd.Engine.Host.Edi.Handlers.Distribute
{
    using System;

    /// <summary>
    /// 命令已分发事件参数
    /// </summary>
    public sealed class DistributedEventArgs : DistributingEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DistributedEventArgs(DistributeContext context)
            : base(context)
        {
        }

        /// <summary>
        /// 转移完成时间
        /// </summary>
        public DateTime DistributedOn { get; internal set; }

        /// <summary>
        /// 转移耗时长
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                return DistributedOn - DistributingOn;
            }
        }
    }
}
