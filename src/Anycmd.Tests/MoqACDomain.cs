
namespace Anycmd.Tests
{
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Engine.Host.Impl;
    using Repositories;
    using System;
    using System.Linq;

    public class MoqAcDomain : DefaultAcDomain
    {
        protected override void OnSignOuted(Guid sessionId)
        {
            var repository = GetRequiredService<IRepository<UserSession>>();
            var entity = repository.GetByKey(sessionId);
            if (entity != null)
            {
                entity.IsAuthenticated = false;
                repository.Update(entity);
            }
        }

        protected override Account GetAccountById(Guid accountId)
        {
            var repository = GetRequiredService<IRepository<Account>>();
            return repository.GetByKey(accountId);
        }

        protected override Account GetAccountByLoginName(string loginName)
        {
            var repository = GetRequiredService<IRepository<Account>>();
            return repository.AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, loginName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
