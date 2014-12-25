
namespace Anycmd.Engine.Host.Edi.Handlers.Execute
{
    using System;

    /// <summary>
    /// 命令执行前事件参数
    /// </summary>
    public class ExecutingEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        protected ExecutingEventArgs(
            MessageContext command)
        {
            this.Id = Guid.NewGuid();
            this.ExecutingOn = DateTime.Now;
            this.Command = command;
        }

        /// <summary>
        /// 事件标识
        /// </summary>
        public Guid Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime ExecutingOn
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MessageContext Command { get; private set; }
    }
}
