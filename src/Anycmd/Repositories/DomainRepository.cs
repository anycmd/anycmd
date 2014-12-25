
namespace Anycmd.Repositories
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 表示领域仓储的基类。
    /// </summary>
    public abstract class DomainRepository : DisposableObject, IDomainRepository
    {
        #region Private Fields
        private volatile bool _committed;
        private readonly HashSet<ISourcedAggregateRoot> _saveHash = new HashSet<ISourcedAggregateRoot>();
        private readonly Action<ISourcedAggregateRoot> _delegatedUpdateAndClearAggregateRoot = ar => ar.GetType().GetMethod(SourcedAggregateRoot.UpdateVersionAndClearUncommittedEventsMethodName,
            BindingFlags.NonPublic | BindingFlags.Instance).Invoke(ar, null);
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>DomainRepository</c> 类型的对象。
        /// </summary>
        protected DomainRepository()
        {
            this._committed = false;
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// 查看被保存的聚合跟对象列表。
        /// </summary>
        protected HashSet<ISourcedAggregateRoot> SaveHash
        {
            get { return this._saveHash; }
        }

        /// <summary>
        /// 查看更新聚合跟的版本号并清空它的未提交事件列表的委托方法。
        /// </summary>
        protected Action<ISourcedAggregateRoot> DelegatedUpdateAndClearAggregateRoot
        {
            get { return this._delegatedUpdateAndClearAggregateRoot; }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 提交领域仓储中的变化。
        /// </summary>
        protected abstract void DoCommit();

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing) { }

        /// <summary>
        /// 创建并返回一个给定类型的聚合根对象。注意：该类型的聚合根必须有无参构造函数。
        /// </summary>
        /// <typeparam name="TAggregateRoot">聚合根对象类型。</typeparam>
        /// <returns>给定类型的聚合根对象。</returns>
        /// <exception cref="RepositoryException">当给定类型的聚合根没有无参构造函数时引发。</exception>
        protected TAggregateRoot CreateAggregateRootInstance<TAggregateRoot>()
            where TAggregateRoot : class, ISourcedAggregateRoot
        {
            Type aggregateRootType = typeof(TAggregateRoot);
            ConstructorInfo constructor = aggregateRootType
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p =>
                {
                    var parameters = p.GetParameters();
                    return parameters.Length == 0;
                }).FirstOrDefault();
            if (constructor != null)
                return constructor.Invoke(null) as TAggregateRoot;
            throw new RepositoryException("At least one parameterless constructor should be defined on the aggregate root type '{0}'.", typeof(TAggregateRoot));
        }
        #endregion

        #region IDomainRepository Members
        /// <summary>
        /// 获取给定标识的聚合根对象。
        /// </summary>
        /// <param name="id">聚合根标识。</param>
        /// <returns>给定类型的聚合根对象。</returns>
        public abstract TAggregateRoot Get<TAggregateRoot>(Guid id)
            where TAggregateRoot : class, ISourcedAggregateRoot;

        /// <summary>
        /// 保存给定的聚合根对象进领域仓储。
        /// </summary>
        /// <param name="aggregateRoot">将被保存的聚合根对象。</param>
        public virtual void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, ISourcedAggregateRoot
        {
            if (!_saveHash.Contains(aggregateRoot))
                _saveHash.Add(aggregateRoot);
            _committed = false;
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public abstract bool DistributedTransactionSupported { get; }
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work was successfully committed.
        /// </summary>
        public bool Committed
        {
            get { return this._committed; }
            protected set { this._committed = value; }
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void Commit()
        {
            this.DoCommit();
            this._saveHash.ToList().ForEach(this._delegatedUpdateAndClearAggregateRoot);
            this._saveHash.Clear();
            this._committed = true;
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public abstract void Rollback();
        #endregion
    }
}
