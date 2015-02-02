
namespace Anycmd.Engine.Ac
{
    using Abstractions.Rbac;
    using Exceptions;
    using Host;
    using Host.Ac.Identity;
    using Host.Ac.Rbac;
    using Host.Dapper;
    using Host.Impl;
    using Logging;
    using Messages.Identity;
    using Rdb;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using Util;

    /// <summary>
    /// 表示用户会话业务实体。
    /// </summary>
    public class AcSessionState : IAcSession, IStateObject
    {
        public static readonly AcSessionState Empty;

        private readonly IAcDomain _acDomain;
        private readonly Guid _id;
        private readonly Guid _accountId;
        private AccountState _account;
        private AccountPrivilege _accountPrivilege;

        public static Action<IAcDomain, Dictionary<string, object>> SignIn { get; set; }

        public static Action<IAcDomain, IAcSession> SignOut { get; set; }

        public static Action<IAcDomain, Guid> SignOuted { get; set; }

        public static Func<IAcDomain, Guid, Account> GetAccountById { get; set; }

        public static Func<IAcDomain, string, Account> GetAccountByLoginName { get; set; }

        public static Func<IAcDomain, Guid, IAcSessionEntity> GetAcSession { get; set; }

        public static Func<IAcDomain, Guid, AccountState, IAcSession> AddAcSession { get; set; }

        public static Action<IAcDomain, IAcSessionEntity> UpdateAcSession { get; set; } 

        public static Action<IAcDomain, Guid> DeleteAcSession { get; set; }

        static AcSessionState()
        {
            Empty = new AcSessionState(EmptyAcDomain.SingleInstance, Guid.Empty, AccountState.Empty)
            {
                _accountPrivilege = null
            };
            SignIn = DoSignIn;
            SignOut = DoSignOut;
            SignOuted = OnSignOuted;
            GetAccountById = GetAccount;
            GetAccountByLoginName = GetAccount;
            GetAcSession = GetAcSessionById;
            AddAcSession = DoAddAcSession;
            UpdateAcSession = DoUpdateAcSession;
            DeleteAcSession = DoDeleteAcSession;
        }

        private AcSessionState()
        {
        }

        internal AcSessionState(IAcDomain host, Guid sessionId, AccountState account)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }
            if (account == AccountState.Empty)
            {
                Identity = new UnauthenticatedIdentity();
            }
            else
            {
                Identity = new AnycmdIdentity(account.LoginName);
            }
            _acDomain = host;
            _id = sessionId;
            _account = account;
            _accountId = account.Id;
        }

        public AcSessionState(IAcDomain host, IAcSessionEntity acSessionEntity)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (acSessionEntity == null)
            {
                throw new ArgumentNullException("acSessionEntity");
            }
            Identity = new AnycmdIdentity(acSessionEntity.LoginName);
            _acDomain = host;
            _id = acSessionEntity.Id;
            _accountId = acSessionEntity.AccountId;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public IAcDomain AcDomain
        {
            get { return _acDomain; }
        }

        public AccountPrivilege AccountPrivilege
        {
            get
            {
                if (_accountPrivilege != null) return _accountPrivilege;
                _accountPrivilege = new AccountPrivilege(this.AcDomain, this);
                string msg;
                if (!AcDomain.DsdSetSet.CheckRoles(_accountPrivilege.AuthorizedRoles, out msg))
                {
                    throw new ValidationException(msg);
                }
                return _accountPrivilege;
            }
        }

        /// <summary>
        /// 工人
        /// </summary>
        public AccountState Account
        {
            get
            {
                if (_account != null) return _account;
                _account = AccountState.Create(GetAccountById(_acDomain, this._accountId));
                return _account;
            }
        }

        public IIdentity Identity { get; private set; }

        /// <summary>
        /// .NET的IPrincipal接口的IsInRole方法基本是鸡肋。建议不要面向这个接口编程。
        /// </summary>
        /// <param name="role">单个角色标识。不支持复杂的带有分隔符的甚至带有逻辑运算的字符串。</param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            Guid roleId;
            if (!Guid.TryParse(role, out roleId))
            {
                throw new ValidationException("意外的角色标识" + role);
            }

            return this.AccountPrivilege.AuthorizedRoleIds.Contains(roleId);
        }

        #region 静态成员

        #region 私有方法
        private static RdbDescriptor GetAccountDb(IAcDomain acDomain)
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

        private static void OnSignOuted(IAcDomain acDomain, Guid sessionId)
        {
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute("update AcSession set IsAuthenticated=@IsAuthenticated where Id=@Id", new { IsAuthenticated = false, Id = sessionId });
            }
        }

        private static void DoSignIn(IAcDomain acDomain, Dictionary<string, object> args)
        {
            var loginName = args.ContainsKey("loginName") ? (args["loginName"] ?? string.Empty).ToString() : string.Empty;
            var password = args.ContainsKey("password") ? (args["password"] ?? string.Empty).ToString() : string.Empty;
            var rememberMe = args.ContainsKey("rememberMe") ? (args["rememberMe"] ?? string.Empty).ToString() : string.Empty;
            var passwordEncryptionService = acDomain.GetRequiredService<IPasswordEncryptionService>();
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                throw new ValidationException("用户名和密码不能为空");
            }
            var addVisitingLogCommand = new AddVisitingLogCommand(Empty)
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
            var account = GetAccountByLoginName(acDomain, loginName);
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
            DicState dic;
            if (!acDomain.DicSet.TryGetDic("auditStatus", out dic))
            {
                throw new AnycmdException("意外的字典编码auditStatus");
            }
            var auditStatusDic = acDomain.DicSet.GetDicItems(dic);
            if (!auditStatusDic.ContainsKey(auditState))
            {
                auditState = null;
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
                addVisitingLogCommand.Description = "对不起，该账户的允许登录开始时间还没到。请在" + account.AllowStartTime.ToString() + "后登录";
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
            var sessionEntity = GetAcSession(acDomain, account.Id);
            IAcSession acSession;
            if (sessionEntity != null)
            {
                acSession = new AcSessionState(acDomain, sessionEntity.Id, AccountState.Create(account));
                sessionEntity.IsAuthenticated = true;
                UpdateAcSession(acDomain, sessionEntity);
            }
            else
            {
                acSession = AddAcSession(acDomain, account.Id, AccountState.Create(account));
            }
            acSession.SetData("CurrentUser_Wallpaper", account.Wallpaper);
            acSession.SetData("CurrentUser_BackColor", account.BackColor);
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

        private static void DoSignOut(IAcDomain acDomain, IAcSession acSession)
        {
            var acSessionStorage = acDomain.GetRequiredService<IAcSessionStorage>();
            if (!acSession.Identity.IsAuthenticated)
            {
                DeleteAcSession(acDomain, acSession.Account.Id);
                return;
            }
            if (acSession.Account.Id == Guid.Empty)
            {
                Thread.CurrentPrincipal = acSession;
                DeleteAcSession(acDomain, acSession.Account.Id);
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
            if (SignOuted != null)
            {
                SignOuted(acDomain, acSession.Id);
            }
            var entity = GetAccountById(acDomain, acSession.Account.Id);
            if (entity != null)
            {
                acDomain.EventBus.Publish(new AccountLogoutedEvent(acSession, entity));
                acDomain.EventBus.Commit();
            }
        }

        private static Account GetAccount(IAcDomain acDomain, string loginName)
        {
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Account>("select * from [Account] where LoginName=@LoginName", new { LoginName = loginName }).FirstOrDefault();
            }
        }

        private static Account GetAccount(IAcDomain acDomain, Guid accountId)
        {
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Account>("select * from [Account] where Id=@Id", new { Id = accountId }).FirstOrDefault();
            }
        }

        private static IAcSessionEntity GetAcSessionById(IAcDomain acDomain, Guid acSessionId)
        {
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return
                    conn.Query<AcSession>("select * from [AcSession] where Id=@Id", new {Id = acSessionId})
                        .FirstOrDefault();
            }
        }

        /// <summary>
        /// 创建Ac会话
        /// </summary>
        /// <param name="account"></param>
        /// <param name="acDomain"></param>
        /// <param name="sessionId">会话标识。会话级的权限依赖于会话的持久跟踪</param>
        /// <returns></returns>
        private static IAcSession DoAddAcSession(IAcDomain acDomain, Guid sessionId, AccountState account)
        {
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var identity = new AnycmdIdentity(account.LoginName);
                IAcSession user = new AcSessionState(acDomain, sessionId, account);
                conn.Execute(
@"insert into AcSession(Id,AccountId,AuthenticationType,Description,IsAuthenticated,IsEnabled,LoginName) 
    values(@Id,@AccountId,@AuthenticationType,@Description,@IsAuthenticated,@IsEnabled,@LoginName)", new AcSession
                                                                                                   {
                                                                                                       Id = sessionId,
                                                                                                       AccountId = account.Id,
                                                                                                       AuthenticationType = identity.AuthenticationType,
                                                                                                       Description = null,
                                                                                                       IsAuthenticated = identity.IsAuthenticated,
                                                                                                       IsEnabled = 1,
                                                                                                       LoginName = account.LoginName
                                                                                                   });
                return user;
            }
        }

        private static void DoUpdateAcSession(IAcDomain acDomain, IAcSessionEntity acSessionEntity)
        {
            using (var conn = GetAccountDb(acDomain).GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute("update AcSession set IsAuthenticated=@IsAuthenticated where Id=@Id", new { acSessionEntity.IsAuthenticated, acSessionEntity.Id });
            }
        }

        /// <summary>
        /// 删除会话
        /// <remarks>
        /// 会话不应该经常删除，会话级的权限依赖于会话的持久跟踪。用户退出系统只需要清空该用户的内存会话记录和更新数据库中的会话记录为IsAuthenticated为false而不需要删除持久的AcSession。
        /// </remarks>
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="sessionId"></param>
        private static void DoDeleteAcSession(IAcDomain acDomain, Guid sessionId)
        {
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
        #endregion
    }
}
