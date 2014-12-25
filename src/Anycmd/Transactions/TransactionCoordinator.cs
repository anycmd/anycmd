
namespace Anycmd.Transactions
{
    using Model;
    using System.Collections.Generic;

    /// <summary>
    /// 事务协调器抽象实现，用于子类的继承
    /// </summary>
    public abstract class TransactionCoordinator : DisposableObject, ITransactionCoordinator
    {
        private readonly List<IUnitOfWork> _managedUnitOfWorks = new List<IUnitOfWork>();

        protected TransactionCoordinator(params IUnitOfWork[] unitOfWorks)
        {
            if (unitOfWorks == null || unitOfWorks.Length <= 0) return;
            foreach (var uow in unitOfWorks)
                _managedUnitOfWorks.Add(uow);
        }

        protected override void Dispose(bool disposing)
        {
        }

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return true; } // 没有意义
        }

        public bool Committed
        {
            get { return true; } // 没有意义
        }

        public virtual void Commit()
        {
            if (_managedUnitOfWorks.Count <= 0) return;
            foreach (var uow in _managedUnitOfWorks)
                uow.Commit();
        }

        public virtual void Rollback() // 基本上没有意义
        {

        }

        #endregion
    }
}
