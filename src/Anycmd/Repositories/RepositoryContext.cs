
namespace Anycmd.Repositories
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Represents the repository context.
    /// </summary>
    public abstract class RepositoryContext : DisposableObject, IRepositoryContext
    {
        #region Private Fields
        private readonly Guid _id = Guid.NewGuid();
        private ThreadLocal<List<object>> _localNewCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private ThreadLocal<List<object>> _localModifiedCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private ThreadLocal<List<object>> _localDeletedCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private ThreadLocal<bool> _localCommitted = new ThreadLocal<bool>(() => true);
        #endregion

        private ThreadLocal<List<object>> LocalNewCollection
        {
            get {
                return _localNewCollection ??
                       (_localNewCollection = new ThreadLocal<List<object>>(() => new List<object>()));
            }
        }

        private ThreadLocal<List<object>> LocalModifiedCollection
        {
            get {
                return _localModifiedCollection ??
                       (_localModifiedCollection = new ThreadLocal<List<object>>(() => new List<object>()));
            }
        }

        private ThreadLocal<List<object>> LocalDeletedCollection
        {
            get {
                return _localDeletedCollection ??
                       (_localDeletedCollection = new ThreadLocal<List<object>>(() => new List<object>()));
            }
        }

        private ThreadLocal<bool> LocalCommitted
        {
            get { return _localCommitted ?? (_localCommitted = new ThreadLocal<bool>(() => true)); }
        }

        #region Protected Properties
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be added to the repository.
        /// </summary>
        protected IEnumerable<object> NewCollection
        {
            get { return LocalNewCollection.Value; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be modified in the repository.
        /// </summary>
        protected IEnumerable<object> ModifiedCollection
        {
            get { return LocalModifiedCollection.Value; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over the collection that contains all the objects need to be deleted from the repository.
        /// </summary>
        protected IEnumerable<object> DeletedCollection
        {
            get { return LocalDeletedCollection.Value; }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Clears all the registration in the repository context.
        /// </summary>
        /// <remarks>Note that this can only be called after the repository context has successfully committed.</remarks>
        protected void ClearRegistrations()
        {
            this.LocalNewCollection.Value.Clear();
            this.LocalModifiedCollection.Value.Clear();
            this.LocalDeletedCollection.Value.Clear();
        }
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_localCommitted != null)
            {
                this._localCommitted.Dispose();
                _localCommitted = null;
            }
            if (_localDeletedCollection != null)
            {
                this._localDeletedCollection.Dispose();
                _localDeletedCollection = null;
            }
            if (_localModifiedCollection != null)
            {
                this._localModifiedCollection.Dispose();
                _localModifiedCollection = null;
            }
            if (_localNewCollection == null) return;
            this._localNewCollection.Dispose();
            _localNewCollection = null;
        }
        #endregion

        #region IRepositoryContext Members
        /// <summary>
        /// Gets the Id of the repository context.
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }
        /// <summary>
        /// Registers a new object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterNew(object obj)
        {
            //if (localModifiedCollection.Value.Contains(obj))
            //   throw new InvalidOperationException("The object cannot be registered as a new object since it was marked as modified.");
            //if (localNewCollection.Value.Contains(obj))
            //    throw new InvalidOperationException("The object has already been registered as a new object.");
            LocalNewCollection.Value.Add(obj);
            Committed = false;
        }
        /// <summary>
        /// Registers a modified object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterModified(object obj)
        {
            if (LocalDeletedCollection.Value.Contains(obj))
                throw new InvalidOperationException("The object cannot be registered as a modified object since it was marked as deleted.");
            if (!LocalModifiedCollection.Value.Contains(obj) && !LocalNewCollection.Value.Contains(obj))
                LocalModifiedCollection.Value.Add(obj);
            Committed = false;
        }
        /// <summary>
        /// Registers a deleted object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        public virtual void RegisterDeleted(object obj)
        {
            if (LocalNewCollection.Value.Contains(obj))
            {
                if (LocalNewCollection.Value.Remove(obj))
                    return;
            }
            var removedFromModified = LocalModifiedCollection.Value.Remove(obj);
            var addedToDeleted = false;
            if (!LocalDeletedCollection.Value.Contains(obj))
            {
                LocalDeletedCollection.Value.Add(obj);
                addedToDeleted = true;
            }
            LocalCommitted.Value = !(removedFromModified || addedToDeleted);
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public virtual bool DistributedTransactionSupported
        {
            get { return false; }
        }
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work was successfully committed.
        /// </summary>
        public virtual bool Committed
        {
            get { return LocalCommitted.Value; }
            protected set { LocalCommitted.Value = value; }
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public abstract void Commit();
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public abstract void Rollback();

        public abstract IQueryable<TEntity> Query<TEntity>() where TEntity : class;
        #endregion
    }
}
