
namespace Anycmd.Exceptions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public ExceptionEventArgs() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public ExceptionEventArgs(Exception e)
        {
            this.Exception = e;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ExceptionHandled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }
    }
}
