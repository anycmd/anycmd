
namespace Anycmd.Engine.Host.Ac.MessageHandlers
{
    using Commands;
    using Engine.Ac;
    using Exceptions;
    using Identity;
    using Identity.Messages;
    using Repositories;
    using System;
    using System.Linq;

    public class AddAccountCommandHandler : CommandHandler<AddAccountCommand>
    {
        private readonly IAcDomain _host;

        public AddAccountCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(AddAccountCommand command)
        {
            var accountRepository = _host.GetRequiredService<IRepository<Account>>();
            if (string.IsNullOrEmpty(command.Input.OrganizationCode))
            {
                throw new AnycmdException("用户必须属于一个组织结构");
            }
            OrganizationState organization;
            if (!_host.OrganizationSet.TryGetOrganization(command.Input.OrganizationCode, out organization))
            {
                throw new AnycmdException("意外的组织结构码" + command.Input.OrganizationCode);
            }
            if (accountRepository.AsQueryable().Any(a => a.Code == command.Input.Code && a.Id != command.Input.Id))
            {
                throw new ValidationException("用户编码重复");
            }
            if (accountRepository.AsQueryable().Any(a => a.LoginName == command.Input.LoginName))
            {
                throw new ValidationException("重复的登录名");
            }
            var entity = Account.Create(command.Input);
            if (string.IsNullOrEmpty(command.Input.Password))
            {
                throw new ValidationException("新密码不能为空");
            }
            var passwordEncryptionService = _host.GetRequiredService<IPasswordEncryptionService>();
            entity.Password = passwordEncryptionService.Encrypt(command.Input.Password);
            entity.LastPasswordChangeOn = DateTime.Now;

            accountRepository.Add(entity);
            accountRepository.Context.Commit();
            _host.EventBus.Publish(new AccountAddedEvent(entity));
            _host.EventBus.Commit();
        }
    }
}
