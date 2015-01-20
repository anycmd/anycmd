
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

    public class ChangePasswordCommandHandler : CommandHandler<ChangePasswordCommand>
    {
        private readonly IAcDomain _host;

        public ChangePasswordCommandHandler(IAcDomain host)
        {
            this._host = host;
        }

        public override void Handle(ChangePasswordCommand command)
        {
            var accountRepository = _host.RetrieveRequiredService<IRepository<Account>>();
            if (command.Input == null)
            {
                throw new InvalidOperationException("command.Input == null");
            }
            if (string.IsNullOrEmpty(command.Input.LoginName))
            {
                throw new ValidationException("登录名不能为空");
            }

            var entity = accountRepository.AsQueryable().FirstOrDefault(a => a.LoginName == command.Input.LoginName);
            if (entity == null)
            {
                throw new NotExistException("用户名" + command.Input.LoginName + "不存在");
            }
            bool loginNameChanged = !string.Equals(command.Input.LoginName, entity.LoginName);
            AccountState developer;
            if (_host.SysUserSet.TryGetDevAccount(command.Input.LoginName, out developer) && !command.UserSession.IsDeveloper())
            {
                throw new ValidationException("对不起，您不能修改开发人员的密码。");
            }
            if (!command.UserSession.IsDeveloper() && "admin".Equals(entity.LoginName, StringComparison.OrdinalIgnoreCase))
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
            #region 更改密码
            if (string.IsNullOrEmpty(command.Input.OldPassword))
            {
                throw new ValidationException("旧密码不能为空");
            }
            if (string.IsNullOrEmpty(command.Input.NewPassword))
            {
                throw new ValidationException("新密码不能为空");
            }
            var passwordEncryptionService = _host.RetrieveRequiredService<IPasswordEncryptionService>();
            var oldPwd = passwordEncryptionService.Encrypt(command.Input.OldPassword);
            if (!string.Equals(entity.Password, oldPwd))
            {
                throw new ValidationException("旧密码不正确");
            }
            var newPassword = passwordEncryptionService.Encrypt(command.Input.NewPassword);
            if (oldPwd != newPassword)
            {
                entity.Password = newPassword;
                entity.LastPasswordChangeOn = DateTime.Now;
                _host.EventBus.Publish(new PasswordUpdatedEvent(command.UserSession, entity));
            }
            #endregion
            if (loginNameChanged)
            {
                _host.EventBus.Publish(new LoginNameChangedEvent(command.UserSession, entity));
                if (_host.SysUserSet.TryGetDevAccount(entity.Id, out developer))
                {
                    _host.MessageDispatcher.DispatchMessage(new DeveloperUpdatedEvent(command.UserSession, entity));
                }
            }
            accountRepository.Update(entity);
            accountRepository.Context.Commit();
            _host.EventBus.Commit();
        }
    }
}
