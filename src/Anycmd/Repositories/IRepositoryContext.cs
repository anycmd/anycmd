
namespace Anycmd.Repositories
{
    using Model;
    using System;
    using System.Linq;

    /// <summary>
    /// 表示该接口的实现类是仓储事务上下文。
    /// </summary>
    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Gets the unique-identifier of the repository context.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Registers a new object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        void RegisterNew(object obj);
        /// <summary>
        /// Registers a modified object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        void RegisterModified(object obj);
        /// <summary>
        /// Registers a deleted object to the repository context.
        /// </summary>
        /// <param name="obj">The object to be registered.</param>
        void RegisterDeleted(object obj);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    }
}
