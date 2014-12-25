
namespace Anycmd.Engine.Host.Ac.MessageHandlers
{
    using Commands;
    using Engine.Ac;
    using Exceptions;
    using Identity;
    using Identity.Messages;
    using Repositories;

    public class RemoveAccountCommandHandler : CommandHandler<RemoveAccountCommand>
    {
        private readonly IAcDomain _host;

        public RemoveAccountCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(RemoveAccountCommand command)
        {
            var accountRepository = _host.GetRequiredService<IRepository<Account>>();
            AccountState developer;
            if (_host.SysUsers.TryGetDevAccount(command.EntityId, out developer))
            {
                throw new ValidationException("该账户是开发人员，删除该账户之前需先删除该开发人员");
            }
            var entity = accountRepository.GetByKey(command.EntityId);
            if (entity == null)
            {
                return;
            }
            accountRepository.Remove(entity);
            accountRepository.Context.Commit();
            _host.EventBus.Publish(new AccountRemovedEvent(entity));
            _host.EventBus.Commit();
        }
    }
}
