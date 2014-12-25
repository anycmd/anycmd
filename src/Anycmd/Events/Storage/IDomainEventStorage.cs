
namespace Anycmd.Events.Storage
{
    using Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是加载和保存领域事件的领域事件仓库。
    /// </summary>
    public interface IDomainEventStorage : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// 将给定的领域事件保存到领域事件仓库。
        /// </summary>
        /// <param name="domainEvent">将被保存的领域事件。</param>
        void SaveEvent(IDomainEvent domainEvent);

        /// <summary>
        /// 加载给定的聚合根的所有领域事件。
        /// </summary>
        /// <param name="aggregateRootType">聚合根的.NET类型。</param>
        /// <param name="id">聚合根的标识。</param>
        /// <returns>领域事件对象列表。</returns>
        IEnumerable<IDomainEvent> LoadEvents(Type aggregateRootType, Guid id);

        /// <summary>
        /// 加载给定的聚合根的发生在给定的版本号之后的领域事件。
        /// </summary>
        /// <param name="aggregateRootType">聚合根的.NET类型。</param>
        /// <param name="id">聚合根的标识。</param>
        /// <param name="version">领域事件版本号。</param>
        /// <returns>发生在给定的版本号之后的领域事件对象列表。</returns>
        IEnumerable<IDomainEvent> LoadEvents(Type aggregateRootType, Guid id, long version);
    }
}
