
namespace Anycmd.Engine.Host.Ac.MessageHandlers
{
    using Commands;
    using Engine.Ac;
    using Engine.Ac.Messages.Identity;
    using Exceptions;
    using Identity;
    using Repositories;
    using System;
    using System.Linq;

    public class AddAccountCommandHandler : CommandHandler<AddAccountCommand>
    {
        private readonly IAcDomain _acDomain;

        public AddAccountCommandHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public override void Handle(AddAccountCommand command)
        {
            var accountRepository = _acDomain.RetrieveRequiredService<IRepository<Account>>();
            if (string.IsNullOrEmpty(command.Input.LoginName))
            {
                throw new AnycmdException("LoginName不能为空");
            }
            if (string.IsNullOrEmpty(command.Input.CatalogCode))
            {
                throw new AnycmdException("用户必须属于一个目录");
            }
            CatalogState catalog;
            if (!_acDomain.CatalogSet.TryGetCatalog(command.Input.CatalogCode, out catalog))
            {
                throw new AnycmdException("意外的目录码" + command.Input.CatalogCode);
            }
            if (!string.IsNullOrEmpty(command.Input.Code) && accountRepository.AsQueryable().Any(a => a.Code == command.Input.Code && a.Id != command.Input.Id))
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
            var passwordEncryptionService = _acDomain.RetrieveRequiredService<IPasswordEncryptionService>();
            entity.Password = passwordEncryptionService.Encrypt(command.Input.Password);
            entity.LastPasswordChangeOn = DateTime.Now;

            accountRepository.Add(entity);
            accountRepository.Context.Commit();
            _acDomain.EventBus.Publish(new AccountAddedEvent(command.AcSession, entity));
            _acDomain.EventBus.Commit();
        }
    }
}
