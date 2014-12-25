
using System;

namespace Anycmd.Exceptions
{
    /// <summary>
    /// Represents that the implemented classes are exception handlers.
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Handles a specific exception.
        /// </summary>
        /// <param name="ex">The exception to be handled.</param>
        /// <returns>True if the exceptioin was successfully handled, otherwise, false.</returns>
        bool HandleException(Exception ex);
    }
}
