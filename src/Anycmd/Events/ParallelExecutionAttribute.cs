
namespace Anycmd.Events
{
    using System;

    /// <summary>
    /// Represents that the event handlers decorated by this attribute
    /// will handle the events in a parallel manner.
    /// </summary>
    /// <remarks>This attribute is only applicable to the event handlers and will only
    /// be used by the event buses, event aggregators or event dispatchers. Applying this attribute to
    /// other types of classes will take no effect.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ParallelExecutionAttribute : Attribute
    {

    }
}
