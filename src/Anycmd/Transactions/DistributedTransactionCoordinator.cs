
namespace Anycmd.Transactions
{
    using Model;
    using System.Transactions;


    /// <summary>
    /// 分布式事务协调器。其依赖于微软的<see cref="TransactionScope"/>类实现分布式事务协调
    /// </summary>
    internal sealed class DistributedTransactionCoordinator : TransactionCoordinator
    {
        private readonly TransactionScope _scope = new TransactionScope();

        public DistributedTransactionCoordinator(params IUnitOfWork[] unitOfWorks)
            : base(unitOfWorks)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _scope.Dispose();
        }


        public override void Commit()
        {
            base.Commit();
            _scope.Complete();
        }
    }
}
