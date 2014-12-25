
namespace Anycmd.Engine.Host.Impl
{
    using Ac;
    using Ac.Identity;
    using Ac.Identity.Messages;
    using Ac.MemorySets;
    using Anycmd.Rdb;
    using Bus;
    using Commands;
    using Dapper;
    using Edi;
    using Engine.Ac;
    using Events;
    using Exceptions;
    using Logging;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using Util;

    /// <summary>
    /// 
    /// </summary>
    public abstract class AcDomain : AnycmdServiceContainer, IAcDomain
    {
        private static readonly object Locker = new object();
        private bool _pluginsLoaded;

        private readonly Guid _id = Guid.NewGuid();

        public Guid Id
        {
            get { return _id; }
        }

        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartedAt { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AfterInitAt { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ReadyAt { get; protected set; }

        protected AcDomain()
        {
            lock (Locker)
            {
                this.Name = "DefaultAcDomain";
                this.StartedAt = DateTime.UtcNow;
                this.SignIn = this.DoSignIn;
                this.SignOut = this.DoSignOut;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual AcDomain Init()
        {
            this.Config = Conventions;
            OnConfigLoad();

            Configure();

            OnAfterInit();

            return this;
        }

        private HostConvention _conventions;

        /// <summary>
        /// Gets the conventions.
        /// </summary>
        /// <value>The conventions.</value>
        public virtual HostConvention Conventions
        {
            get { return _conventions ?? (_conventions = new HostConvention()); }
            set { _conventions = value; }
        }

        private IAppConfig _config;
        /// <summary>
        /// 
        /// </summary>
        public IAppConfig Config
        {
            get
            {
                return _config;
            }
            private set
            {
                _config = value;
                OnAfterConfigChanged();
            }
        }

        public INodeHost NodeHost { get; protected set; }

        #region 属性
        public IUserSession UserSession
        {
            get
            {
                var principal = HttpContext.Current != null ? HttpContext.Current.User : Thread.CurrentPrincipal;
                if (principal.Identity.IsAuthenticated)
                {
                    var storage = this.GetRequiredService<IUserSessionStorage>();
                    var userSession = storage.GetData(Conventions.CurrentUserSessionCacheKey) as IUserSession;
                    if (userSession == null)
                    {
                        if (string.IsNullOrEmpty(principal.Identity.Name))
                        {
                            return UserSessionState.Empty;
                        }
                        var account = this.GetAccountByLoginName(principal.Identity.Name);
                        if (account == null)
                        {
                            return UserSessionState.Empty;
                        }
                        var userSessionRepository = GetRequiredService<IRepository<UserSession>>();
                        var sessionEntity = userSessionRepository.GetByKey(account.Id);
                        if (sessionEntity != null)
                        {
                            userSession = new UserSessionState(this, sessionEntity.Id, principal, AccountState.Create(account));
                        }
                        else
                        {
                            // 使用账户标识作为会话标识会导致一个账户只有一个会话
                            // TODO:支持账户和会话的一对多，为会话级的动态责任分离做准备
                            var userSessionService = GetRequiredService<IUserSessionService>();
                            userSession = userSessionService.CreateSession(this, account.Id, AccountState.Create(account));
                        }
                        storage.SetData(Conventions.CurrentUserSessionCacheKey, userSession);
                    }
                    return userSession;
                }
                else
                {
                    return UserSessionState.Empty;
                }
            }
            private set
            {
                var storage = this.GetRequiredService<IUserSessionStorage>();
                storage.SetData(Conventions.CurrentUserSessionCacheKey, value);
            }
        }

        private ILoggingService _loggingService;
        public ILoggingService LoggingService
        {
            get { return _loggingService ?? (_loggingService = GetRequiredService<ILoggingService>()); }
        }

        public IMessageDispatcher MessageDispatcher { get; protected set; }

        public ICommandBus CommandBus { get; protected set; }

        public IEventBus EventBus { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IRdbs Rdbs { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IDbTables DbTables { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IDbViews DbViews { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IDbTableColumns DbTableColumns { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IDbViewColumns DbViewColumns { get; protected set; }

        /// <summary>
        /// 应用系统。
        /// </summary>
        public IAppSystemSet AppSystemSet { get; protected set; }

        /// <summary>
        /// 系统按钮
        /// </summary>
        public IButtonSet ButtonSet { get; protected set; }

        /// <summary>
        /// 系统用户/账户
        /// </summary>
        public ISysUserSet SysUsers { get; protected set; }

        /// <summary>
        /// 系统字典
        /// </summary>
        public IDicSet DicSet { get; protected set; }

        /// <summary>
        /// 系统模型
        /// </summary>
        public IEntityTypeSet EntityTypeSet { get; protected set; }

        /// <summary>
        /// 系统操作
        /// </summary>
        public IFunctionSet FunctionSet { get; protected set; }

        /// <summary>
        /// 系统组织结构
        /// </summary>
        public IOrganizationSet OrganizationSet { get; protected set; }

        /// <summary>
        /// 系统界面视图
        /// </summary>
        public IUiViewSet UiViewSet { get; protected set; }

        /// <summary>
        /// 系统资源
        /// </summary>
        public IResourceTypeSet ResourceTypeSet { get; protected set; }

        /// <summary>
        /// 权限集
        /// </summary>
        public IPrivilegeSet PrivilegeSet { get; protected set; }

        /// <summary>
        /// 菜单集
        /// </summary>
        public IMenuSet MenuSet { get; protected set; }

        /// <summary>
        /// 角色集
        /// </summary>
        public IRoleSet RoleSet { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public ISsdSetSet SsdSetSet { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public IDsdSetSet DsdSetSet { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IGroupSet GroupSet { get; protected set; }
        #endregion

        /// <summary>
        /// Config has changed
        /// </summary>
        public virtual void OnAfterConfigChanged()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public virtual void SetConfig(IAppConfig config)
        {
            Config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnConfigLoad()
        {
        }

        /// <summary>
        /// 当Configure方法调用后。
        /// </summary>
        public void OnAfterInit()
        {
            AfterInitAt = DateTime.UtcNow;

            LoadPlugin(Conventions.Plugins.ToArray());
            _pluginsLoaded = true;

            ReadyAt = DateTime.UtcNow;
        }

        public T DeserializeFromString<T>(string value)
        {
            // TODO:移除对ServiceStack.Text的依赖
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
        }

        public string SerializeToString<T>(T value)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString<T>(value);
        }

        /// <summary>
        /// this.DirectEventBus.Publish(evnt);
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="evnt"></param>
        public void PublishEvent<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            this.EventBus.Publish(evnt);
        }

        /// <summary>
        /// this.DirectEventBus.Commit();
        /// </summary>
        public void CommitEventBus()
        {
            this.EventBus.Commit();
        }

        /// <summary>
        /// this.DirectCommandBus.Publish(command);
        /// this.DirectCommandBus.Commit();
        /// </summary>
        /// <param name="command"></param>
        public void Handle(ISysCommand command)
        {
            this.CommandBus.Publish(command);
            this.CommandBus.Commit();
        }

        /// <summary>
        /// Retrieves the service of type <c>T</c> from the provider.
        /// If the service cannot be found, this method returns <c>null</c>.
        /// </summary>
        public T GetService<T>()
        {
            return (T)this.GetService(typeof(T));
        }

        /// <summary>
        /// Retrieves the service of type <c>T</c> from the provider.
        /// If the service cannot be found, a <see cref="ServiceNotFoundException"/> will be thrown.
        /// </summary>
        public T GetRequiredService<T>()
        {
            return (T)GetRequiredService(typeof(T));
        }

        /// <summary>
        /// Retrieves the service of type <paramref name="serviceType"/> from the provider.
        /// If the service cannot be found, a <see cref="ServiceNotFoundException"/> will be thrown.
        /// </summary>
        public object GetRequiredService(Type serviceType)
        {
            object service = this.GetService(serviceType);
            if (service == null)
                throw new ServiceNotFoundException(serviceType);
            return service;
        }

        public Action<Dictionary<string, object>> SignIn { get; set; }
        public Action SignOut { get; set; }

        #region DoSignIn
        protected virtual void DoSignIn(Dictionary<string, object> args)
        {
            var loginName = args.ContainsKey("loginName") ? (args["loginName"] ?? string.Empty).ToString() : string.Empty;
            var password = args.ContainsKey("password") ? (args["password"] ?? string.Empty).ToString() : string.Empty;
            var rememberMe = args.ContainsKey("rememberMe") ? (args["rememberMe"] ?? string.Empty).ToString() : string.Empty;
            var passwordEncryptionService = GetRequiredService<IPasswordEncryptionService>();
            var userSessionRepository = GetRequiredService<IRepository<UserSession>>();
            var userSessionService = GetRequiredService<IUserSessionService>();
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                throw new ValidationException("用户名和密码不能为空");
            }
            if (this.UserSession.Principal.Identity.IsAuthenticated)
            {
                return;
            }
            var addVisitingLogCommand = new AddVisitingLogCommand
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
            var account = GetAccountByLoginName(loginName);
            if (account == null)
            {
                addVisitingLogCommand.Description = "用户名错误";
                MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            else
            {
                addVisitingLogCommand.AccountId = account.Id;
            }
            if (password != account.Password)
            {
                addVisitingLogCommand.Description = "密码错误";
                MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (account.IsEnabled == 0)
            {
                addVisitingLogCommand.Description = "对不起，该账户已被禁用";
                MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            string auditState = account.AuditState == null ? account.AuditState : account.AuditState.ToLower();
            DicState dic;
            if (!DicSet.TryGetDic("auditStatus", out dic))
            {
                throw new CoreException("意外的字典编码auditStatus");
            }
            var auditStatusDic = DicSet.GetDicItems(dic);
            if (!auditStatusDic.ContainsKey(auditState))
            {
                auditState = null;
            }
            if (auditState == null
                || auditState == "notaudit")
            {
                addVisitingLogCommand.Description = "对不起，该账户尚未审核";
                MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (auditState == "auditnotpass")
            {
                addVisitingLogCommand.Description = "对不起，该账户未通过审核";
                MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (account.AllowStartTime.HasValue && SystemTime.Now() < account.AllowStartTime.Value)
            {
                addVisitingLogCommand.Description = "对不起，该账户的允许登录开始时间还没到。请在" + account.AllowStartTime.ToString() + "后登录";
                MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (account.AllowEndTime.HasValue && SystemTime.Now() > account.AllowEndTime.Value)
            {
                addVisitingLogCommand.Description = "对不起，该账户的允许登录时间已经过期";
                MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                throw new ValidationException(addVisitingLogCommand.Description);
            }
            if (account.LockEndTime.HasValue || account.LockStartTime.HasValue)
            {
                DateTime lockStartTime = account.LockStartTime ?? DateTime.MinValue;
                DateTime lockEndTime = account.LockEndTime ?? DateTime.MaxValue;
                if (SystemTime.Now() > lockStartTime && SystemTime.Now() < lockEndTime)
                {
                    addVisitingLogCommand.Description = "对不起，该账户暂被锁定";
                    MessageDispatcher.DispatchMessage(addVisitingLogCommand);
                    throw new ValidationException(addVisitingLogCommand.Description);
                }
            }

            if (account.PreviousLoginOn.HasValue && account.PreviousLoginOn.Value >= SystemTime.Now().AddMinutes(5))
            {
                addVisitingLogCommand.Description = "检测到您的上次登录时间在未来。这可能是因为本站点服务器的时间落后导致的，请联系管理员。";
                MessageDispatcher.DispatchMessage(addVisitingLogCommand);
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
            var sessionEntity = userSessionRepository.GetByKey(account.Id);
            IUserSession userSession;
            if (sessionEntity != null)
            {
                var principal = new AnycmdPrincipal(this, new AnycmdIdentity(sessionEntity.AuthenticationType, true, sessionEntity.LoginName));
                userSession = new UserSessionState(this, sessionEntity.Id, principal, AccountState.Create(account));
                sessionEntity.IsAuthenticated = true;
                userSessionRepository.Update(sessionEntity);
            }
            else
            {
                userSession = userSessionService.CreateSession(this, account.Id, AccountState.Create(account));
            }
            this.UserSession = userSession;
            userSession.SetData("CurrentUser_Wallpaper", account.Wallpaper);
            userSession.SetData("CurrentUser_BackColor", account.BackColor);
            if (HttpContext.Current != null)
            {
                bool createPersistentCookie = rememberMe.Equals("rememberMe", StringComparison.OrdinalIgnoreCase);
                FormsAuthentication.SetAuthCookie(account.LoginName, createPersistentCookie);
                HttpContext.Current.User = userSession.Principal;
            }
            else
            {
                Thread.CurrentPrincipal = userSession.Principal;
            }
            Guid? visitingLogId = Guid.NewGuid();
            this.UserSession.SetData("UserContext_Current_VisitingLogId", visitingLogId);
            userSessionRepository.Context.Commit();
            EventBus.Publish(new AccountLoginedEvent(account));
            EventBus.Commit();
            addVisitingLogCommand.StateCode = (int)VisitState.Logged;
            addVisitingLogCommand.ReasonPhrase = VisitState.Logged.ToName();
            addVisitingLogCommand.Description = "登录成功";
            MessageDispatcher.DispatchMessage(addVisitingLogCommand);
        }
        #endregion

        #region DoSignOut
        protected virtual void DoSignOut()
        {
            var userSessionStorage = GetRequiredService<IUserSessionStorage>();
            var userSessionService = GetRequiredService<IUserSessionService>();
            if (!this.UserSession.Principal.Identity.IsAuthenticated)
            {
                userSessionService.DeleteSession(this, this.UserSession.Account.Id);
                return;
            }
            if (this.UserSession.Account.Id == Guid.Empty)
            {
                Thread.CurrentPrincipal = new AnycmdPrincipal(this, new UnauthenticatedIdentity());
                userSessionService.DeleteSession(this, this.UserSession.Account.Id);
                return;
            }
            if (HttpContext.Current != null)
            {
                FormsAuthentication.SignOut();
            }
            else
            {
                Thread.CurrentPrincipal = new AnycmdPrincipal(this, new UnauthenticatedIdentity());
            }
            userSessionStorage.Clear();
            OnSignOuted(this.UserSession.Id);
            var entity = this.GetAccountById(this.UserSession.Account.Id);
            if (entity != null)
            {
                EventBus.Publish(new AccountLogoutedEvent(entity));
                EventBus.Commit();
            }
        }
        #endregion

        #region AccountQuery
        protected internal virtual void OnSignOuted(Guid sessionId)
        {
            using (var conn = GetAccountDb().GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute("update UserSession set IsAuthenticated=@IsAuthenticated where Id=@Id", new { IsAuthenticated = false, Id = sessionId });
            }
        }

        protected internal virtual Account GetAccountByLoginName(string loginName)
        {
            using (var conn = GetAccountDb().GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Account>("select * from [Account] where LoginName=@LoginName", new { LoginName = loginName }).FirstOrDefault();
            }
        }

        protected internal virtual Account GetAccountById(Guid accountId)
        {
            using (var conn = GetAccountDb().GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Account>("select * from [Account] where Id=@ContractorId", new { ContractorId = accountId }).FirstOrDefault();
            }
        }
        #endregion

        #region plugin
        /// <summary>
        /// 根据插件类型获取域内插件地址
        /// </summary>
        /// <param name="pluginType"></param>
        /// <returns></returns>
        public virtual string GetPluginBaseDirectory(PluginType pluginType)
        {
            return Conventions.PluginBaseDirectory(pluginType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugins"></param>
        protected virtual void LoadPlugin(params IPlugin[] plugins)
        {
            foreach (var plugin in plugins)
            {
                try
                {
                    plugin.Register(this);
                }
                catch (Exception ex)
                {
                    LoggingService.Error("Error loading plugin " + plugin.GetType().Name, ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plugins"></param>
        protected void AddPlugin(params IPlugin[] plugins)
        {
            if (_pluginsLoaded)
            {
                LoadPlugin(plugins);
            }
            else
            {
                foreach (var plugin in plugins)
                {
                    Conventions.Plugins.Add(plugin);
                }
            }
        }
        #endregion

        private RdbDescriptor GetAccountDb()
        {

            EntityTypeState entityType;
            if (!this.EntityTypeSet.TryGetEntityType("Ac", "Account", out entityType))
            {
                throw new Exceptions.CoreException("意外的实体类型码Ac.Account");
            }
            RdbDescriptor db;
            if (!this.Rdbs.TryDb(entityType.DatabaseId, out db))
            {
                throw new Exceptions.CoreException("意外的账户数据库标识" + entityType.DatabaseId);
            }
            return db;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(obj, this))
            {
                return true;
            }
            if (!(obj is IAcDomain))
            {
                return false;
            }
            return ((IAcDomain)obj).Id == this.Id;
        }
    }
}
