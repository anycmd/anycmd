
namespace Anycmd.Logging
{
    using Model;
    using System;

    /// <summary>
    /// 操作日志
    /// </summary>
    public abstract class OperationLogBase : EntityObject, IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid TargetId { get; set; }
    }
}
