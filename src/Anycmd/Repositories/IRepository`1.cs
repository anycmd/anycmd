
namespace Anycmd.Repositories
{
    using Model;
    using System;
    using System.Linq;

    /// <summary>
    /// 表示该接口的实现类是领域聚合根实体仓储。
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根.NET对象类型。</typeparam>
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 获取该领域聚合跟实体仓储依附的仓储上下文对象。
        /// </summary>
        IRepositoryContext Context { get; }

        /// <summary>
        /// 查询当前聚合根实体数据源。
        /// </summary>
        /// <returns>对聚合根实体集进行查询计算的接口</returns>
        IQueryable<TAggregateRoot> AsQueryable();

        /// <summary>
        /// 根据给定的标识从仓储中获取聚合根实体。
        /// </summary>
        /// <param name="key">聚合根实体的标识</param>
        /// <returns>聚合根实体对象。</returns>
        TAggregateRoot GetByKey(ValueType key);

        /// <summary>
        /// 添加一个聚合根实体对象到仓储。
        /// </summary>
        /// <param name="aggregateRoot">被添加到聚合根实体仓储的聚合根实体对象。</param>
        void Add(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 更新当前聚合根实体仓储中的给定的聚合根对象。
        /// </summary>
        /// <param name="aggregateRoot">将被更新的聚合根实体对象。</param>
        void Update(TAggregateRoot aggregateRoot);

        /// <summary>
        /// 从仓储中移除给定的聚合根实体。
        /// </summary>
        /// <param name="aggregateRoot">将被移除的聚合根实体。</param>
        void Remove(TAggregateRoot aggregateRoot);
    }
}
