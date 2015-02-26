
namespace Anycmd.Engine.Host.Impl
{
    using Ac.Identity;
    using Ac.Rbac;
    using Dapper;
    using Engine.Ac;
    using Engine.Ac.Ssd;
    using Engine.Ac.Accounts;
    using Engine.Rdb;
    using Exceptions;
    using Host;
    using Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using Util;


    public class DefaultAcSessionMethod : IAcSessionMethod
    {
        private readonly Guid _id = new Guid("B44819B4-6D34-4ABB-802E-F77252FD9A47");

        public DefaultAcSessionMethod()
        {
            SignIn = DoSignIn;
            SignOut = DoSignOut;
            SignOuted = OnSignOuted;
            GetAccountById = GetAccount;
            GetAccountByLoginName = GetAccount;
            GetAcSessionEntity = GetAcSessionEntityById;
            GetAcSession = GetAcSessionByLoginName;
            AddAcSession = DoAddAcSession;
            UpdateAcSession = DoUpdateAcSession;
            DeleteAcSession = DoDeleteAcSession;
        }

        public DefaultAcSessionMethod(
            Action<IAcDomain, Dictionary<string, object>> signIn,
            Action<IAcDomain, IAcSession> signOut,
            Action<IAcDomain, Guid> signOuted,
            Func<IAcDomain, Guid, IAccount> getAccountById,
            Func<IAcDomain, string, IAccount> getAccountByLoginName,
            Func<IAcDomain, Guid, IAcSessionEntity> getAcSessionEntity,
            Func<IAcDomain, string, IAcSession> getAcSession,
            Action<IAcDomain, IAcSessionEntity> addAcSession,
            Action<IAcDomain, IAcSessionEntity> updateAcSession,
            Action<IAcDomain, Guid> deleteAcSession)
        {
            this.SignIn = signIn;
            this.SignOut = signOut;
            this.SignOuted = signOuted;
            this.GetAccountById = getAccountById;
            this.GetAccountByLoginName = getAccountByLoginName;
            this.GetAcSessionEntity = getAcSessionEntity;
            this.GetAcSession = getAcSession;
            this.AddAcSession = addAcSession;
            this.UpdateAcSession = updateAcSession;
            this.DeleteAcSession = deleteAcSession;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public Action<IAcDomain, Dictionary<string, object>> SignIn { get; private set; }

        public Action<IAcDomain, IAcSession> SignOut { get; private set; }

        public Action<IAcDomain, Guid> SignOuted { get; private set; }

        public Func<IAcDomain, Guid, IAccount> GetAccountById { get; private set; }

        public Func<IAcDomain, string, IAccount> GetAccountByLoginName { get; private set; }

        public Func<IAcDomain, Guid, IAcSessionEntity> GetAcSessionEntity { get; private set; }

        public Func<IAcDomain, string, IAcSession> GetAcSession { get; private set; }

        public Action<IAcDomain, IAcSessionEntity> AddAcSession { get; private set; }

        public Action<IAcDomain, IAcSessionEntity> UpdateAcSession { get; private set; }

        public Action<IAcDomain, Guid> DeleteAcSession { get; private set; }

        #region 私有方法
        private IAcSession GetAcSessionByLoginName(IAcDomain acDomain, string loginName)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return AcSessionState.Empty;
            }
            var storage = acDomain.GetRequiredService<IAcSessionStorage>();
            var acSession = storage.GetData(acDomain.Config.CurrentAcSessionCacheKey) as IAcSession;
            if (acSession != null) return acSession;
            var account = AcSessionState.AcMethod.GetAccountByLoginName(acDomain, loginName);
            if (account == null)
            {
                return AcSessionState.Empty;
            }
            var sessionEntity = AcSessionState.AcMethod.GetAcSessionEntity(acDomain, account.Id);
            if (sessionEntity != null)
            {
                if (!sessionEntity.IsAuthenticated)
                {
                    return AcSessionState.Empty;
                }
                acSession = new AcSessionState(acDomain, sessionEntity);
            }
            else
            {
                // 使用账户标识作为会话标识会导致一个账户只有一个会话
                // TODO:支持账户和会话的一对多，为会话级的动态责任分离做准备
                var accountState = AccountState.Create(account);
                var identity = new AnycmdIdentity(account.LoginName);
                var acSessionEntity = new AcSession
                {
                    Id = account.Id,
                    AccountId = account.Id,
                    AuthenticationType = identity.AuthenticationType,
                    Description = null,
                    IsAuthenticated = identity.IsAuthenticated,
                    IsEnabled = 1,
                    LoginName = account.LoginName
                };
                AcSessionState.AcMethod.AddAcSession(acDomain, acSessionEntity);
                acSession = new AcSessionState(acDomain, account.Id, accountState);
            }
            storage.SetData(acDomain.Config.CurrentAcSessionCacheKey, acSession);
            return acSession;
        }

        private RdbDescriptor GetAccountDb(IAcDomain acDomain)
        {
            EntityTypeState entityType;
            if (!acDomain.EntityTypeSet.TryGetEntityType(new Coder("Ac", "Account"), out entityType))
            {
                throw new AnycmdException("意外的实体类型码Ac.Account");
            }
            RdbDescriptor db;
            if (!acDomain.Rdbs.TryDb(entityType.DatabaseId, out db))
            {
                throw new AnycmdException("意外的账户数据库标识" + entityType.DatabaseId);
            }
            return db;
        }

        private void OnSignOuted(IAcDomain acDomain, Guid sessionId)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return;
            }
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute("update AcSession set IsAuthenticated=@IsAuthenticated where Id=@Id", new { IsAuthenticated = false, Id = sessionId });
            }
        }

        private void DoSignIn(IAcDomain acDomain, Dictionary<string, object> args)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return;
            }
            var loginName = args.ContainsKey("loginName") ? (args["loginName"] ?? string.Empty).ToString() : string.Empty;
            var password = args.ContainsKey("password") ? (args["password"] ?? string.Empty).ToString() : string.Empty;
            var rememberMe = args.ContainsKey("rememberMe") ? (args["rememberMe"] ?? string.Empty).ToString() : string.Empty;
            var passwordEncryptionService = acDomain.GetRequiredService<IPasswordEncryptionService>();
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                throw new ValidationException("用户名和密码不能为空");
            }
            var addVisitingLogCommand = new AddVisitingLogCommand(AcSessionState.Empty)
            {
                IpAddress = IpHelper.GetClientIp(),
                LoginName = loginName,
                VisitedOn = null,
                VisitOn = DateTime.Now,
                Description = "登录成功",
                ReasonPhrase = VisitState.LogOnFail.ToName(),
                StateCode = (int)VisitState.LogOnFail
            };
            password = passwordEncryptionService.Encrypt(password);
            var account = AcSessionState.AcMethod.GetAccountByLoginName(acDomain, loginName);
            if (account == null)
            {
                addVisitingLogCommand.Description = "用户名错误";
                acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            else
            {
                addVisitingLogCommand.AccountId = account.Id;
            }
            if (password != account.Password)
            {
                addVisitingLogCommand.Description = "密码错误";
                acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (account.IsEnabled == 0)
            {
                addVisitingLogCommand.Description = "对不起，该账户已被禁用";
                acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            string auditState = account.AuditState == null ? account.AuditState : account.AuditState.ToLower();
            CatalogState dicItem;
            if (!acDomain.CatalogSet.TryGetCatalog(auditState, out dicItem))
            {
                throw new AnycmdException("意外的字典编码" + auditState);
            }
            if (auditState == null
                || auditState == "notaudit")
            {
                addVisitingLogCommand.Description = "对不起，该账户尚未审核";
                acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (auditState == "auditnotpass")
            {
                addVisitingLogCommand.Description = "对不起，该账户未通过审核";
                acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (account.AllowStartTime.HasValue && SystemTime.Now() < account.AllowStartTime.Value)
            {
                addVisitingLogCommand.Description = "对不起，该账户的允许登录开始时间还没到。请在" + account.AllowStartTime + "后登录";
                acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (account.AllowEndTime.HasValue && SystemTime.Now() > account.AllowEndTime.Value)
            {
                addVisitingLogCommand.Description = "对不起，该账户的允许登录时间已经过期";
                acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (account.LockEndTime.HasValue || account.LockStartTime.HasValue)
            {
                DateTime lockStartTime = account.LockStartTime ?? DateTime.MinValue;
                DateTime lockEndTime = account.LockEndTime ?? DateTime.MaxValue;
                if (SystemTime.Now() > lockStartTime && SystemTime.Now() < lockEndTime)
                {
                    addVisitingLogCommand.Description = "对不起，该账户暂被锁定";
                    acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                    throw new ValidationException(addVisitingLogCommand.Description);
                }
            }

            if (account.PreviousLoginOn.HasValue && account.PreviousLoginOn.Value >= SystemTime.Now().AddMinutes(5))
            {
                addVisitingLogCommand.Description = "检测到您的上次登录时间在未来。这可能是因为本站点服务器的时间落后导致的，请联系管理员。";
                acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            account.PreviousLoginOn = SystemTime.Now();
            if (!account.FirstLoginOn.HasValue)
            {
                account.FirstLoginOn = SystemTime.Now();
            }
            account.LoginCount = (account.LoginCount ?? 0) + 1;
            account.IpAddress = IpHelper.GetClientIp();

            // 使用账户标识作为会话标识会导致一个账户只有一个会话
            // TODO:支持账户和会话的一对多，为会话级的动态责任分离做准备
            var sessionEntity = AcSessionState.AcMethod.GetAcSessionEntity(acDomain, account.Id);
            IAcSession acSession;
            if (sessionEntity != null)
            {
                acSession = new AcSessionState(acDomain, sessionEntity.Id, AccountState.Create(account));
                sessionEntity.IsAuthenticated = true;
                AcSessionState.AcMethod.UpdateAcSession(acDomain, sessionEntity);
            }
            else
            {
                var accountState = AccountState.Create(account);
                var identity = new AnycmdIdentity(account.LoginName);
                var acSessionEntity = new AcSession
                {
                    Id = account.Id,
                    AccountId = account.Id,
                    AuthenticationType = identity.AuthenticationType,
                    Description = null,
                    IsAuthenticated = identity.IsAuthenticated,
                    IsEnabled = 1,
                    LoginName = account.LoginName
                };
                AcSessionState.AcMethod.AddAcSession(acDomain, acSessionEntity);
                acSession = new AcSessionState(acDomain, account.Id, accountState);
            }
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = acSession;
                bool createPersistentCookie = rememberMe.Equals("rememberMe", StringComparison.OrdinalIgnoreCase);
                FormsAuthentication.SetAuthCookie(account.LoginName, createPersistentCookie);
            }
            else
            {
                Thread.CurrentPrincipal = acSession;
            }
            Guid? visitingLogId = Guid.NewGuid();

            acSession.SetData("UserContext_Current_VisitingLogId", visitingLogId);
            acSession.SetData(acDomain.Config.CurrentAcSessionCacheKey, acSession);

            acDomain.EventBus.Publish(new AccountLoginedEvent(acSession, account));
            acDomain.EventBus.Commit();
            addVisitingLogCommand.StateCode = (int)VisitState.Logged;
            addVisitingLogCommand.ReasonPhrase = VisitState.Logged.ToName();
            addVisitingLogCommand.Description = "登录成功";
            acDomain.MessageDispatcher.DispatchMessage(addVisitingLogCommand);
        }

        private void DoSignOut(IAcDomain acDomain, IAcSession acSession)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return;
            }
            if (!acSession.Identity.IsAuthenticated)
            {
                return;
            }
            var acSessionStorage = acDomain.GetRequiredService<IAcSessionStorage>();
            var acSessionEntity = AcSessionState.AcMethod.GetAcSessionEntity(acDomain, acSession.Id);
            acSessionEntity.IsAuthenticated = false;
            AcSessionState.AcMethod.UpdateAcSession(acDomain, acSessionEntity);
            if (acSession.Account.Id == Guid.Empty)
            {
                Thread.CurrentPrincipal = acSession;
                return;
            }
            if (HttpContext.Current != null)
            {
                FormsAuthentication.SignOut();
            }
            else
            {
                Thread.CurrentPrincipal = acSession;
            }
            acSessionStorage.Clear();
            if (AcSessionState.AcMethod.SignOuted != null)
            {
                AcSessionState.AcMethod.SignOuted(acDomain, acSession.Id);
            }
            var entity = AcSessionState.AcMethod.GetAccountById(acDomain, acSession.Account.Id);
            if (entity != null)
            {
                acDomain.EventBus.Publish(new AccountLogoutedEvent(acSession, entity));
                acDomain.EventBus.Commit();
            }
        }

        private IAccount GetAccount(IAcDomain acDomain, string loginName)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return null;
            }
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Account>("select * from [Account] where LoginName=@LoginName", new { LoginName = loginName }).FirstOrDefault();
            }
        }

        private IAccount GetAccount(IAcDomain acDomain, Guid accountId)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return null;
            }
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Account>("select * from [Account] where Id=@Id", new { Id = accountId }).FirstOrDefault();
            }
        }

        private IAcSessionEntity GetAcSessionEntityById(IAcDomain acDomain, Guid acSessionId)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return null;
            }
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return
                    conn.Query<AcSession>("select * from [AcSession] where Id=@Id", new { Id = acSessionId })
                        .FirstOrDefault();
            }
        }

        private void DoAddAcSession(IAcDomain acDomain, IAcSessionEntity acSessionEntity)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return;
            }
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(
@"insert into AcSession(Id,AccountId,AuthenticationType,Description,IsAuthenticated,IsEnabled,LoginName) 
    values(@Id,@AccountId,@AuthenticationType,@Description,@IsAuthenticated,@IsEnabled,@LoginName)", acSessionEntity);
            }
        }

        private void DoUpdateAcSession(IAcDomain acDomain, IAcSessionEntity acSessionEntity)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return;
            }
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute("update AcSession set IsAuthenticated=@IsAuthenticated where Id=@Id", new { acSessionEntity.IsAuthenticated, acSessionEntity.Id });
            }
        }

        private void DoDeleteAcSession(IAcDomain acDomain, Guid sessionId)
        {
            if (EmptyAcDomain.SingleInstance.Equals(acDomain))
            {
                return;
            }
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute("delete AcSession where Id=@Id", new { Id = sessionId });
            }
        }
        #endregion
    }
}
