
namespace Anycmd.Repositories
{
    using Model;
    using System;
    using System.Linq;

    /// <summary>
    /// 标识领域聚合跟实体对象仓储基类。
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根.NET对象类型。</typeparam>
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        #region Private Fields
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>Repository&lt;TAggregateRoot&gt;</c> 类型的对象。
        /// </summary>
        protected Repository()
        {
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// 添加一个聚合根实体对象到仓储。
        /// </summary>
        /// <param name="aggregateRoot">被添加到聚合根实体仓储的聚合根实体对象。</param>
        protected abstract void DoAdd(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 根据给定的标识从仓储中获取聚合根实体。
        /// </summary>
        /// <param name="key">聚合根实体的标识</param>
        /// <returns>聚合根实体对象。</returns>
        protected abstract TAggregateRoot DoGetByKey(ValueType key);

        /// <summary>
        /// 更新当前聚合根实体仓储中的给定的聚合根对象。
        /// </summary>
        /// <param name="aggregateRoot">将被更新的聚合根实体对象。</param>
        protected abstract void DoUpdate(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 从仓储中移除给定的聚合根实体。
        /// </summary>
        /// <param name="aggregateRoot">将被移除的聚合根实体。</param>
        protected abstract void DoRemove(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 查询当前聚合根实体数据源。
        /// </summary>
        /// <returns>对聚合根实体集进行查询计算的接口</returns>
        protected abstract IQueryable<TAggregateRoot> DoAsQueryable();
        #endregion

        #region IRepository<TAggregateRoot> Members
        /// <summary>
        /// 获取该领域聚合跟实体仓储依附的仓储上下文对象<see cref="Anycmd.Repositories.IRepositoryContext"/>
        /// </summary>
        public abstract IRepositoryContext Context { get; }

        /// <summary>
        /// 查询当前聚合根实体数据源。
        /// </summary>
        /// <returns>对聚合根实体集进行查询计算的接口</returns>
        public IQueryable<TAggregateRoot> AsQueryable()
        {
            return this.DoAsQueryable();
        }

        /// <summary>
        /// 根据给定的标识从仓储中获取聚合根实体。
        /// </summary>
        /// <param name="key">聚合根实体的标识</param>
        /// <returns>聚合根实体对象。</returns>
        public TAggregateRoot GetByKey(ValueType key)
        {
            return this.DoGetByKey(key);
        }

        /// <summary>
        /// 添加一个聚合根实体对象到仓储。
        /// </summary>
        /// <param name="aggregateRoot">被添加到聚合根实体仓储的聚合根实体对象。</param>
        public void Add(TAggregateRoot aggregateRoot)
        {
            this.DoAdd(aggregateRoot);
        }

        /// <summary>
        /// 更新当前聚合根实体仓储中的给定的聚合根对象。
        /// </summary>
        /// <param name="aggregateRoot">将被更新的聚合根实体对象。</param>
        public void Update(TAggregateRoot aggregateRoot)
        {
            this.DoUpdate(aggregateRoot);
        }

        /// <summary>
        /// 从仓储中移除给定的聚合根实体。
        /// </summary>
        /// <param name="aggregateRoot">将被移除的聚合根实体。</param>
        public void Remove(TAggregateRoot aggregateRoot)
        {
            this.DoRemove(aggregateRoot);
        }
        #endregion
    }
}
