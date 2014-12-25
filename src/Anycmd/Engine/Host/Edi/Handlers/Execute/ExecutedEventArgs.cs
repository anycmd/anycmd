
namespace Anycmd.Engine.Host.Edi.Handlers.Execute
{
    using System;

    /// <summary>
    /// 命令执行后事件参数
    /// </summary>
    public sealed class ExecutedEventArgs : ExecutingEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "command"></param>
        public ExecutedEventArgs(
            MessageContext command)
            : base(command)
        {
        }

        /// <summary>
        /// 完成执行的时间
        /// </summary>
        public DateTime ExecutedOn
        {
            get;
            internal set;
        }

        /// <summary>
        /// 完成执行所耗时长
        /// </summary>
        public TimeSpan TimeSpan
        {
            get
            {
                return ExecutedOn - ExecutingOn;
            }
        }
    }
}
