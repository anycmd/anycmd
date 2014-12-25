
namespace Anycmd.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Is thrown when the ServiceManager cannot find a required service.
    /// <remarks>
    /// ServiceManager通常是IoC框架。
    /// </remarks>
    /// </summary>
    [Serializable()]
    public class ServiceNotFoundException : CoreException
    {
        public ServiceNotFoundException()
            : base()
        {
        }

        public ServiceNotFoundException(Type serviceType)
            : base("Required service not found: " + serviceType.FullName)
        {
        }

        public ServiceNotFoundException(string message)
            : base(message)
        {
        }

        public ServiceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ServiceNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
