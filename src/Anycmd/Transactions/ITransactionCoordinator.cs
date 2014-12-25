
namespace Anycmd.Transactions
{
    using Model;
    using System;

    /// <summary>
    /// 定义事务协调器
    /// </summary>
    public interface ITransactionCoordinator : IUnitOfWork, IDisposable
    {
    }
}
