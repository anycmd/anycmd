
namespace Anycmd.Logging
{
    using Events;
    using System;

    /// <summary>
    /// 操作完成事件
    /// </summary>
    public class OperatedEvent : DomainEvent
    {
        #region Ctor
        public OperatedEvent() { }
        public OperatedEvent(OperationLogBase source) : base(source) { }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public DateTime OperatedOn { get; set; }
    }
}