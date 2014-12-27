
namespace Anycmd.Engine.Host.Ac.MessageHandlers
{
    using Commands;
    using Engine.Ac;
    using Exceptions;
    using Identity;
    using Identity.Messages;
    using Repositories;
    using System.Linq;

    public class UpdateAccountCommandHandler : CommandHandler<UpdateAccountCommand>
    {
        private readonly IAcDomain _host;

        public UpdateAccountCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(UpdateAccountCommand command)
        {
            var accountRepository = _host.GetRequiredService<IRepository<Account>>();
            if (accountRepository.AsQueryable().Any(a => a.Code == command.Output.Code && a.Id != command.Output.Id))
            {
                throw new ValidationException("用户编码重复");
            }
            var entity = accountRepository.GetByKey(command.Output.Id);
            if (entity == null)
            {
                throw new NotExistException();
            }
            if (command.Output.OrganizationCode != entity.OrganizationCode)
            {
                if (string.IsNullOrEmpty(command.Output.OrganizationCode))
                {
                    throw new AnycmdException("用户必须属于一个组织结构");
                }
                OrganizationState organization;
                if (!_host.OrganizationSet.TryGetOrganization(command.Output.OrganizationCode, out organization))
                {
                    throw new AnycmdException("意外的组织结构码" + command.Output.OrganizationCode);
                }
            }
            entity.Update(command.Output);
            accountRepository.Update(entity);
            accountRepository.Context.Commit();
            AccountState devAccount;
            if (_host.SysUsers.TryGetDevAccount(entity.Id, out devAccount))
            {
                _host.EventBus.Publish(new DeveloperUpdatedEvent(entity));
            }
            _host.EventBus.Publish(new AccountUpdatedEvent(entity));
            _host.EventBus.Commit();
        }
    }
}
