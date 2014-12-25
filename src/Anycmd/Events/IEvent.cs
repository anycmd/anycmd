
namespace Anycmd.Events
{
    using Bus;
    using Model;
    using System;

    /// <summary>
    /// 表示该接口的实现类是事件。
    /// </summary>
    public interface IEvent : IMessage, IEntity
    {
        /// <summary>
        /// 读取该事件发生的事件。
        /// </summary>
        /// <remarks>系统间的时间格式可能是不同的，系统间推荐使用UTC时间。</remarks>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the assembly qualified type name of the event. which includes the name
        /// of the assembly from which the System.Type was loaded.
        /// </summary>
        string AssemblyQualifiedEventType { get; }
    }
}
