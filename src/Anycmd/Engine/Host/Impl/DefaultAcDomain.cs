
namespace Anycmd.Engine.Host.Impl
{
    using Ac;
    using Ac.Identity;
    using Ac.Infra;
    using Ac.MemorySets;
    using Ac.MessageHandlers;
    using Ac.Rbac;
    using Bus;
    using Bus.DirectBus;
    using Edi;
    using Edi.Entities;
    using Edi.Handlers;
    using Edi.MessageHandlers;
    using Engine.Rdb;
    using IdGenerators;
    using Logging;
    using Query;
    using Rdb.MemorySets;
    using Rdb.MessageHandlers;

    /// <summary>
    /// 系统实体宿主。
    /// </summary>
    public class DefaultAcDomain : AcDomain, IHandler<MemorySetInitingEvent>, IHandler<MemorySetInitializedEvent>
    {
        public DefaultAcDomain()
        {
            base.MessageDispatcher = new MessageDispatcher();
            base.CommandBus = new DirectCommandBus(this.MessageDispatcher);
            this.EventBus = new DirectEventBus(this.MessageDispatcher);
            this.IdGenerator = new SequentialIdGenerator();
            this.SequenceIdGenerator = new SequentialIdGenerator();

            base.Rdbs = new Rdbs(this, new DbTables(this), new DbViews(this), new DbTableColumns(this), new DbViewColumns(this));
            base.AppSystemSet = new AppSystemSet(this);
            base.ButtonSet = new ButtonSet(this);
            base.SysUserSet = new SysUserSet(this);
            base.DicSet = new DicSet(this);
            base.EntityTypeSet = new EntityTypeSet(this);
            base.FunctionSet = new FunctionSet(this);
            base.OrganizationSet = new OrganizationSet(this);
            base.UiViewSet = new UiViewSet(this);
            base.ResourceTypeSet = new ResourceTypeSet(this);
            base.PrivilegeSet = new PrivilegeSet(this);
            base.MenuSet = new MenuSet(this);
            base.RoleSet = new RoleSet(this);
            base.SsdSetSet = new SsdSetSet(this);
            base.DsdSetSet = new DsdSetSet(this);
            base.GroupSet = new GroupSet(this);
            this.NodeHost = new DefaultNodeHost(this);
            this.MessageDispatcher.Register((IHandler<MemorySetInitingEvent>)this);
            this.MessageDispatcher.Register((IHandler<MemorySetInitializedEvent>)this);
        }

        public DefaultAcDomain(IAppConfig config)
            : base(config)
        {

        }

        public virtual void Handle(MemorySetInitingEvent message)
        {

        }

        public virtual void Handle(MemorySetInitializedEvent message)
        {

        }

        public override void Configure()
        {
            this.AddDefaultService<IOriginalHostStateReader>(new RdbOriginalHostStateReader(this));
            this.AddDefaultService<IRdbMetaDataService>(new SqlServerMetaDataService(this));
            this.AddDefaultService<ISqlFilterStringBuilder>(new SqlFilterStringBuilder());
            this.AddDefaultService<ISecurityService>(new DefaultSecurityService());
            this.AddDefaultService<IPasswordEncryptionService>(new PasswordEncryptionService(this));
            this.AddDefaultService<IUserSessionService>(new DefaultUserSessionService());
            this.AddDefaultService<IRbacService>(new DefaultRbacService(this));

            base.MessageDispatcher.Register(new AccountLoginedEventHandler(this));
            base.MessageDispatcher.Register(new AccountLogoutedEventHandler(this));
            base.MessageDispatcher.Register(new AddVisitingLogCommandHandler(this));
            base.MessageDispatcher.Register(new AddAccountCommandHandler(this));
            base.MessageDispatcher.Register(new UpdateAccountCommandHandler(this));
            base.MessageDispatcher.Register(new RemoveAccountCommandHandler(this));
            base.MessageDispatcher.Register(new AddPasswordCommandHandler(this));
            base.MessageDispatcher.Register(new ChangePasswordCommandHandler(this));
            base.MessageDispatcher.Register(new SaveHelpCommandHandler(this));
            base.MessageDispatcher.Register(new UpdateDatabaseCommandHandler(this));
            base.MessageDispatcher.Register(new UpdateDbTableColumnCommandHandler(this));
            base.MessageDispatcher.Register(new UpdateDbTableCommandHandler(this));
            base.MessageDispatcher.Register(new UpdateDbViewColumnCommandHandler(this));
            base.MessageDispatcher.Register(new UpdateDbViewCommandHandler(this));

            this.MessageDispatcher.Register(new OperatedEventHandler(this));

            this.AddService(typeof(INodeHostBootstrap), new FastNodeHostBootstrap(this));
            this.MessageDispatcher.Register(new AddBatchCommandHandler(this));
            this.MessageDispatcher.Register(new UpdateBatchCommandHandler(this));
            this.MessageDispatcher.Register(new UpdateInfoRuleCommandHandler(this));
            this.MessageDispatcher.Register(new RemoveBatchCommandHandler(this));
            this.MessageDispatcher.Register(new UpdateStateCodeCommandHandler(this));

            this.Map(EntityTypeMap.Create<Action>("Edi"));
            this.Map(EntityTypeMap.Create<Archive>("Edi"));
            this.Map(EntityTypeMap.Create<Batch>("Edi"));
            this.Map(EntityTypeMap.Create<Element>("Edi"));
            this.Map(EntityTypeMap.Create<InfoDic>("Edi"));
            this.Map(EntityTypeMap.Create<InfoDicItem>("Edi"));
            this.Map(EntityTypeMap.Create<InfoGroup>("Edi"));
            this.Map(EntityTypeMap.Create<InfoRule>("Edi"));
            this.Map(EntityTypeMap.Create<Node>("Edi"));
            this.Map(EntityTypeMap.Create<NodeElementAction>("Edi"));
            this.Map(EntityTypeMap.Create<NodeElementCare>("Edi"));
            this.Map(EntityTypeMap.Create<NodeTopic>("Edi"));
            this.Map(EntityTypeMap.Create<NodeOntologyCare>("Edi"));
            this.Map(EntityTypeMap.Create<NodeOntologyOrganization>("Edi"));
            this.Map(EntityTypeMap.Create<Ontology>("Edi"));
            this.Map(EntityTypeMap.Create<OntologyOrganization>("Edi"));
            this.Map(EntityTypeMap.Create<Plugin>("Edi"));
            this.Map(EntityTypeMap.Create<Process>("Edi"));
            this.Map(EntityTypeMap.Create<StateCode>("Edi"));
            this.Map(EntityTypeMap.Create<Topic>("Edi"));
            this.Map(EntityTypeMap.Create<MessageEntity>("Edi", "Command"));

            // TODO:实现一个良好的插件架构
            // TODO:参考InfoRule模块完成命令插件模块的配置
            //var plugins = Resolver.Resolve<IPluginImporter>().GetPlugins();
            //if (plugins != null) {
            //    foreach (var item in plugins) {
            //        this.Plugins.Add(item);
            //    }
            //}

            this.Map(EntityTypeMap.Create<RDatabase>("Ac"));
            this.Map(EntityTypeMap.Create<DbTable>("Ac"));
            this.Map(EntityTypeMap.Create<DbView>("Ac"));
            this.Map(EntityTypeMap.Create<DbTableColumn>("Ac"));
            this.Map(EntityTypeMap.Create<DbViewColumn>("Ac"));
            this.Map(EntityTypeMap.Create<DbTableSpace>("Ac", "TableSpace"));
            this.Map(EntityTypeMap.Create<ExceptionLog>("Ac"));
            this.Map(EntityTypeMap.Create<OperationLog>("Ac"));
            this.Map(EntityTypeMap.Create<OperationHelp>("Ac", "Help"));
            this.Map(EntityTypeMap.Create<AnyLog>("Ac"));

            this.Map(EntityTypeMap.Create<AppSystem>("Ac"));
            this.Map(EntityTypeMap.Create<Button>("Ac"));
            this.Map(EntityTypeMap.Create<Dic>("Ac"));
            this.Map(EntityTypeMap.Create<DicItem>("Ac"));
            this.Map(EntityTypeMap.Create<EntityType>("Ac"));
            this.Map(EntityTypeMap.Create<Function>("Ac"));
            this.Map(EntityTypeMap.Create<Menu>("Ac"));
            this.Map(EntityTypeMap.Create<OperationHelp>("Ac"));
            this.Map(EntityTypeMap.Create<Organization>("Ac"));
            this.Map(EntityTypeMap.Create<UiView>("Ac"));
            this.Map(EntityTypeMap.Create<UiViewButton>("Ac"));
            this.Map(EntityTypeMap.Create<Property>("Ac"));
            this.Map(EntityTypeMap.Create<ResourceType>("Ac"));

            this.Map(EntityTypeMap.Create<Account>("Ac"));
            this.Map(EntityTypeMap.Create<DeveloperId>("Ac"));
            this.Map(EntityTypeMap.Create<VisitingLog>("Ac"));

            this.Map(EntityTypeMap.Create<Group>("Ac"));
            this.Map(EntityTypeMap.Create<Role>("Ac"));
            this.Map(EntityTypeMap.Create<SsdSet>("Ac"));
            this.Map(EntityTypeMap.Create<DsdSet>("Ac"));
            this.Map(EntityTypeMap.Create<Privilege>("Ac"));
        }

        private void AddDefaultService<T>(object service)
        {
            if (this.GetService(typeof(T)) == null)
            {
                this.AddService(typeof(T), service);
            }
        }
    }
}
