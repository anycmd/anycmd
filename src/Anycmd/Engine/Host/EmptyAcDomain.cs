
namespace Anycmd.Engine.Host
{
    using Ac;
    using Ac.Infra;
    using Ac.MemorySets;
    using Anycmd.Rdb;
    using Bus;
    using Edi;
    using Edi.Handlers;
    using Edi.MemorySets;
    using Engine.Ac;
    using Hecp;
    using Logging;
    using System;
    using System.Collections.Generic;

    internal class EmptyAcDomain : AnycmdServiceContainer, IAcDomain
    {
        public static readonly IAcDomain SingleInstance = new EmptyAcDomain();

        public EmptyAcDomain()
        {
            this.NodeHost = new EmptyNodeHost(this);
            this.SignIn = (args) =>
            {
            };
            this.SignOut = () =>
            {
            };
        }

        public IAppSystemSet AppSystemSet
        {
            get { return Ac.MemorySets.Impl.AppSystemSet.Empty; }
        }

        public IButtonSet ButtonSet
        {
            get { return Ac.MemorySets.Impl.ButtonSet.Empty; }
        }

        public ICommandBus CommandBus
        {
            get { return EmptyCommandBus.Empty; }
        }

        public IAppConfig Config
        {
            get
            {
                return EmptyAppConfig.Empty;
            }
        }

        public IUserSession CreateSession(Guid sessionId, AccountState account)
        {
            return UserSessionState.Empty;
        }

        public IDbTableColumns DbTableColumns
        {
            get { return Rdb.DbTableColumns.Empty; }
        }

        public IDbTables DbTables
        {
            get { return Rdb.DbTables.Empty; }
        }

        public IDbViewColumns DbViewColumns
        {
            get { return Rdb.DbViewColumns.Empty; }
        }

        public IDbViews DbViews
        {
            get { return Rdb.DbViews.Empty; }
        }

        public void DeleteSession(Guid sessionId)
        {

        }

        public IDicSet DicSet
        {
            get { return Ac.MemorySets.Impl.DicSet.Empty; }
        }

        public IEntityTypeSet EntityTypeSet
        {
            get { return Ac.MemorySets.Impl.EntityTypeSet.Empty; }
        }

        public IEventBus EventBus
        {
            get { return EmptyEventBus.Empty; }
        }

        public IFunctionSet FunctionSet
        {
            get { return Ac.MemorySets.Impl.FunctionSet.Empty; }
        }

        public IGroupSet GroupSet
        {
            get { return Ac.MemorySets.Impl.GroupSet.Empty; }
        }

        public Guid Id
        {
            get { return Guid.Empty; }
        }

        public IMenuSet MenuSet
        {
            get { return Ac.MemorySets.Impl.MenuSet.Empty; }
        }

        public IMessageDispatcher MessageDispatcher
        {
            get { return EmptyMessageDispatcher.Empty; }
        }

        public string Name
        {
            get { return "EmptyAcDomain"; }
        }

        public IOrganizationSet OrganizationSet
        {
            get { return Ac.MemorySets.Impl.OrganizationSet.Empty; }
        }

        public IUiViewSet UiViewSet
        {
            get { return Ac.MemorySets.Impl.UiViewSet.Empty; }
        }

        public IPrivilegeSet PrivilegeSet
        {
            get { return Ac.MemorySets.Impl.PrivilegeSet.Empty; }
        }

        public IRdbs Rdbs
        {
            get { return Rdb.Rdbs.Empty; }
        }

        public IResourceTypeSet ResourceTypeSet
        {
            get { return Ac.MemorySets.Impl.ResourceTypeSet.Empty; }
        }

        public IRoleSet RoleSet
        {
            get { return Ac.MemorySets.Impl.RoleSet.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ISsdSetSet SsdSetSet
        {
            get { return Ac.MemorySets.Impl.SsdSetSet.Empty; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IDsdSetSet DsdSetSet
        {
            get { return Ac.MemorySets.Impl.DsdSetSet.Empty; }
        }
        public Action<Dictionary<string, object>> SignIn { get; set; }
        public Action SignOut { get; set; }

        public ISysUserSet SysUsers
        {
            get { return Ac.MemorySets.Impl.SysUserSet.Empty; }
        }

        public IUserSession UserSession
        {
            get { return UserSessionState.Empty; }
        }

        public string BuildInPluginsBaseDirectory
        {
            get { return string.Empty; }
        }

        public ILoggingService LoggingService
        {
            get { return EmptyLoggingService.Instance; }
        }

        public string GetPluginBaseDirectory(PluginType pluginType)
        {
            return string.Empty;
        }

        public INodeHost NodeHost { get; private set; }

        private class EmptyNodeHost : INodeHost
        {
            private readonly IAcDomain _host;

            public EmptyNodeHost(IAcDomain host)
            {
                this._host = host;
            }

            public IStateCodes StateCodes
            {
                get
                {
                    return Edi.MemorySets.Impl.StateCodes.Empty;
                }
            }

            public IProcesseSet Processs
            {
                get { return Edi.MemorySets.Impl.ProcesseSet.Empty; }
            }

            public INodeSet Nodes
            {
                get { return Edi.MemorySets.Impl.NodeSet.Empty; }
            }

            public IInfoDicSet InfoDics
            {
                get { return Edi.MemorySets.Impl.InfoDicSet.Empty; }
            }

            public IOntologySet Ontologies
            {
                get { return Edi.MemorySets.Impl.OntologySet.Empty; }
            }

            public IInfoStringConverterSet InfoStringConverters
            {
                get { return Edi.MemorySets.Impl.InfoStringConverterSet.Empty; }
            }

            public IInfoRuleSet InfoRules
            {
                get { return Edi.MemorySets.Impl.InfoRuleSet.Empty; }
            }

            public IMessageProviderSet MessageProviders
            {
                get { return Edi.MemorySets.Impl.MessageProviderSet.Empty; }
            }

            public IEntityProviderSet EntityProviders
            {
                get { return Edi.MemorySets.Impl.EntityProviderSet.Empty; }
            }

            public IMessageTransferSet Transfers
            {
                get { return Edi.MemorySets.Impl.MessageTransferSet.Empty; }
            }

            public IMessageProducer MessageProducer
            {
                get { return new DefaultMessageProducer(); }
            }

            public IHecpHandler HecpHandler
            {
                get
                {
                    return new HecpHandler();
                }

            }

            public List<Func<HecpContext, ProcessResult>> PreHecpRequestFilters
            {
                get { return new List<Func<HecpContext, ProcessResult>>(); }
            }

            public List<Func<MessageContext, ProcessResult>> GlobalEdiMessageHandingFilters
            {
                get { return new List<Func<MessageContext, ProcessResult>>(); }
            }

            public List<Func<MessageContext, ProcessResult>> GlobalEdiMessageHandledFilters
            {
                get { return new List<Func<MessageContext, ProcessResult>>(); }
            }

            public List<Func<HecpContext, ProcessResult>> GlobalHecpResponseFilters
            {
                get { return new List<Func<HecpContext, ProcessResult>>(); }
            }

            public ProcessResult ApplyPreHecpRequestFilters(HecpContext context)
            {
                return ProcessResult.Ok;
            }

            public ProcessResult ApplyEdiMessageHandingFilters(MessageContext context)
            {
                return ProcessResult.Ok;
            }

            public ProcessResult ApplyEdiMessageHandledFilters(MessageContext context)
            {
                return ProcessResult.Ok;
            }

            public ProcessResult ApplyHecpResponseFilters(HecpContext context)
            {
                return ProcessResult.Ok;
            }
        }

        private class EmptyAppConfig : IAppConfig
        {
            public static readonly IAppConfig Empty = new EmptyAppConfig();

            public bool EnableClientCache
            {
                get { return false; }
            }

            public bool EnableOperationLog
            {
                get { return false; }
            }

            public string SelfAppSystemCode
            {
                get { return string.Empty; }
            }

            public string SqlServerTableColumnsSelect
            {
                get { return string.Empty; }
            }

            public string SqlServerTablesSelect
            {
                get { return string.Empty; }
            }

            public string SqlServerViewColumnsSelect
            {
                get { return string.Empty; }
            }

            public string SqlServerViewsSelect
            {
                get { return string.Empty; }
            }

            public int TicksTimeout
            {
                get { return 0; }
            }


            public string InfoFormat
            {
                get { return string.Empty; }
            }

            public string EntityArchivePath
            {
                get { return string.Empty; }
            }

            public string EntityBackupPath
            {
                get { return string.Empty; }
            }

            public bool ServiceIsAlive
            {
                get { return false; }
            }

            public bool TraceIsEnabled
            {
                get { return false; }
            }

            public int BeatPeriod
            {
                get { return int.MaxValue; }
            }

            public string CenterNodeId
            {
                get { return string.Empty; }
            }

            public string ThisNodeId
            {
                get { return string.Empty; }
            }

            public ConfigLevel AuditLevel
            {
                get { return ConfigLevel.Invalid; }
            }

            public AuditType ImplicitAudit
            {
                get { return AuditType.Invalid; }
            }

            public ConfigLevel AclLevel
            {
                get { return ConfigLevel.Invalid; }
            }

            public AllowType ImplicitAllow
            {
                get { return AllowType.Invalid; }
            }

            public ConfigLevel EntityLogonLevel
            {
                get { return ConfigLevel.Invalid; }
            }

            public EntityLogon ImplicitEntityLogon
            {
                get { return EntityLogon.Invalid; }
            }
        }

        private class EmptyLoggingService : ILoggingService
        {
            public static readonly ILoggingService Instance = new EmptyLoggingService();

            public void Log(IAnyLog anyLog)
            {

            }

            public void Log(IAnyLog[] anyLogs)
            {

            }

            public IAnyLog Get(Guid id)
            {
                return new AnyLog(id)
                {
                };
            }

            public IList<IAnyLog> GetPlistAnyLogs(List<Query.FilterData> filters, Query.PagingInput paging)
            {
                return new List<IAnyLog>();
            }

            public IList<OperationLog> GetPlistOperationLogs(Guid? targetId, DateTime? leftCreateOn, DateTime? rightCreateOn, List<Query.FilterData> filters, Query.PagingInput paging)
            {
                return new List<OperationLog>();
            }

            public IList<ExceptionLog> GetPlistExceptionLogs(List<Query.FilterData> filters, Query.PagingInput paging)
            {
                return new List<ExceptionLog>();
            }

            public void ClearAnyLog()
            {

            }

            public void ClearExceptionLog()
            {

            }

            public void Debug(object message)
            {

            }

            public void DebugFormatted(string format, params object[] args)
            {

            }

            public void Info(object message)
            {

            }

            public void InfoFormatted(string format, params object[] args)
            {

            }

            public void Warn(object message)
            {

            }

            public void Warn(object message, Exception exception)
            {

            }

            public void WarnFormatted(string format, params object[] args)
            {

            }

            public void Error(object message)
            {

            }

            public void Error(object message, Exception exception)
            {

            }

            public void ErrorFormatted(string format, params object[] args)
            {

            }

            public void Fatal(object message)
            {

            }

            public void Fatal(object message, Exception exception)
            {

            }

            public void FatalFormatted(string format, params object[] args)
            {

            }

            public bool IsDebugEnabled
            {
                get { return false; }
            }

            public bool IsInfoEnabled
            {
                get { return false; }
            }

            public bool IsWarnEnabled
            {
                get { return false; }
            }

            public bool IsErrorEnabled
            {
                get { return false; }
            }

            public bool IsFatalEnabled
            {
                get { return false; }
            }
        }

        private class EmptyCommandBus : ICommandBus
        {
            public static readonly ICommandBus Empty = new EmptyCommandBus();

            public void Publish<TMessage>(TMessage message) where TMessage : Bus.IMessage
            {

            }

            public void Publish<TMessage>(IEnumerable<TMessage> messages) where TMessage : Bus.IMessage
            {

            }

            public void Clear()
            {

            }

            public bool DistributedTransactionSupported
            {
                get { return false; }
            }

            public bool Committed
            {
                get { return true; }
            }

            public void Commit()
            {

            }

            public void Rollback()
            {

            }

            public void Dispose()
            {

            }
        }

        private class EmptyEventBus : IEventBus
        {
            public static readonly IEventBus Empty = new EmptyEventBus();

            public void Publish<TMessage>(TMessage message) where TMessage : Bus.IMessage
            {

            }

            public void Publish<TMessage>(IEnumerable<TMessage> messages) where TMessage : Bus.IMessage
            {

            }

            public void Clear()
            {

            }

            public bool DistributedTransactionSupported
            {
                get { return false; }
            }

            public bool Committed
            {
                get { return true; }
            }

            public void Commit()
            {

            }

            public void Rollback()
            {

            }

            public void Dispose()
            {

            }
        }

        private class EmptyMessageDispatcher : IMessageDispatcher
        {
            public static readonly IMessageDispatcher Empty = new EmptyMessageDispatcher();

            public void Clear()
            {

            }

            public void DispatchMessage<T>(T message) where T : Bus.IMessage
            {

            }

            public void Register<T>(IHandler<T> handler) where T : Bus.IMessage
            {

            }

            public void UnRegister<T>(IHandler<T> handler) where T : Bus.IMessage
            {

            }

            public event EventHandler<MessageDispatchEventArgs> Dispatching;

            public event EventHandler<MessageDispatchEventArgs> DispatchFailed;

            public event EventHandler<MessageDispatchEventArgs> Dispatched;
        }
    }
}
