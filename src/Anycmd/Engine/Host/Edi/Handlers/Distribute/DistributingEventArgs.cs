
namespace Anycmd.Engine.Host.Edi.Handlers.Distribute
{
    using System;

    /// <summary>
    /// 命令分发前事件参数
    /// </summary>
    public class DistributingEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DistributingEventArgs(DistributeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.Context = context;
            this.DistributingOn = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        public DistributeContext Context { get; private set; }

        public DateTime DistributingOn { get; private set; }
    }
}
