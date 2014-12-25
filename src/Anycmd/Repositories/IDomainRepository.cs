
namespace Anycmd.Repositories
{
    using Model;
    using System;

    /// <summary>
    /// 表示该接口的实现类是领域仓储。
    /// </summary>
    public interface IDomainRepository : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Gets the instance of the aggregate root with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>The instance of the aggregate root with the specified identifier.</returns>
        TAggregateRoot Get<TAggregateRoot>(Guid id)
            where TAggregateRoot : class, ISourcedAggregateRoot;

        /// <summary>
        /// Saves the aggregate represented by the specified aggregate root to the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root that is going to be saved.</param>
        void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, ISourcedAggregateRoot;
    }
}
