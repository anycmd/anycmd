
namespace Anycmd.Engine.Host
{
    using System;

    /// <summary>
    /// 线程睡觉前事件参数
    /// </summary>
    public sealed class SleepingEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sleepTimeSpan"></param>
        public SleepingEventArgs(int sleepTimeSpan)
        {
            this.SleepTimeSpan = sleepTimeSpan;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Canceled { get; set; }

        /// <summary>
        /// 毫秒
        /// </summary>
        public int SleepTimeSpan { get; private set; }
    }
}
