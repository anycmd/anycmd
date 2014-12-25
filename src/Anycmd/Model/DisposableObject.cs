
namespace Anycmd.Model
{
    using System;

    /// <summary>
    /// 表示它的子类是可释放的类。
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        #region Finalization Constructs
        /// <summary>
        /// 释放当前对象。
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(disposing: false);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 释放当前对象。
        /// </summary>
        /// <param name="disposing">指示当前对象是否应被显式释放。true表示显式释放。</param>
        protected abstract void Dispose(bool disposing);
        /// <summary>
        /// Provides the facility that disposes the object in an explicit manner,
        /// preventing the Finalizer from being called after the object has been
        /// disposed explicitly.
        /// </summary>
        protected void ExplicitDispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.ExplicitDispose();
        }
        #endregion
    }
}
