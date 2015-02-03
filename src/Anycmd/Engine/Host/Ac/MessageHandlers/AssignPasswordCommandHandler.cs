
namespace Anycmd.Engine.Host.Ac.MessageHandlers
{
    using Commands;
    using Engine.Ac;
    using Engine.Ac.Messages.Identity;
    using Exceptions;
    using Host;
    using Identity;
    using Repositories;
    using System;
    using System.Linq;

    public class AssignPasswordCommandHandler : CommandHandler<AssignPasswordCommand>
    {
        private readonly IAcDomain _acDomain;

        public AssignPasswordCommandHandler(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public override void Handle(AssignPasswordCommand command)
        {
            var accountRepository = _acDomain.RetrieveRequiredService<IRepository<Account>>();
            if (string.IsNullOrEmpty(command.Input.LoginName))
            {
                throw new ValidationException("登录名不能为空");
            }
            if (string.IsNullOrEmpty(command.Input.Password))
            {
                throw new ValidationException("密码不能为空");
            }
            if (accountRepository.AsQueryable().Any(a => a.LoginName == command.Input.LoginName && a.Id != command.Input.Id))
            {
                throw new ValidationException("重复的登录名");
            }
            var entity = accountRepository.GetByKey(command.Input.Id);
            if (entity == null)
            {
                throw new NotExistException("账户不存在");
            }
            bool loginNameChanged = !string.Equals(command.Input.LoginName, entity.LoginName);
            AccountState developer;
            if (_acDomain.SysUserSet.TryGetDevAccount(command.Input.Id, out developer))
            {
                if (!command.AcSession.IsDeveloper())
                {
                    throw new ValidationException("对不起，您不能修改开发人员的密码。");    
                }
                else if (command.AcSession.Account.Id != command.Input.Id)
                {
                    throw new ValidationException("对不起，您不能修改别的开发者的密码。");
                }
            }
            if (!command.AcSession.IsDeveloper() && "admin".Equals(entity.LoginName, StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationException("对不起，您无权修改admin账户的密码");
            }
            #region 更改登录名
            if (string.IsNullOrEmpty(command.Input.LoginName))
            {
                throw new ValidationException("登录名不能为空");
            }
            if (loginNameChanged)
            {
                entity.LoginName = command.Input.LoginName;
            }
            #endregion
            if (string.IsNullOrEmpty(command.Input.Password))
            {
                throw new ValidationException("新密码不能为空");
            }
            var passwordEncryptionService = _acDomain.RetrieveRequiredService<IPasswordEncryptionService>();
            var newPassword = passwordEncryptionService.Encrypt(command.Input.Password);
            entity.Password = newPassword;
            entity.LastPasswordChangeOn = DateTime.Now;
            accountRepository.Update(entity);
            accountRepository.Context.Commit();
            if (loginNameChanged)
            {
                _acDomain.EventBus.Publish(new LoginNameChangedEvent(command.AcSession, entity));
                if (_acDomain.SysUserSet.TryGetDevAccount(entity.Id, out developer))
                {
                    _acDomain.MessageDispatcher.DispatchMessage(new DeveloperUpdatedEvent(command.AcSession, entity));
                }
            }
            _acDomain.EventBus.Publish(new PasswordUpdatedEvent(command.AcSession, entity));
            _acDomain.EventBus.Commit();
        }
    }
}
