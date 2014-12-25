
namespace Anycmd.Repositories
{
    using Bus;

    /// <summary>
    /// 表示保存聚合时会向事件总线发布事件的领域仓储的基类。
    /// </summary>
    public abstract class EventPublisherDomainRepository : DomainRepository
    {
        #region Private Fields
        private readonly IEventBus _eventBus;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>EventPublisherDomainRepository</c> 类型的对象。
        /// </summary>
        /// <param name="eventBus">The <see cref="Anycmd.Bus.IEventBus"/> instance
        /// to which the domain events are published.</param>
        protected EventPublisherDomainRepository(IEventBus eventBus)
        {
            this._eventBus = eventBus;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _eventBus.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the <see cref="Anycmd.Bus.IEventBus"/> instance to which the domain events are published.
        /// </summary>
        public IEventBus EventBus
        {
            get { return this._eventBus; }
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get { return this._eventBus.DistributedTransactionSupported; }
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            _eventBus.Rollback();
        }
        #endregion
    }
}
