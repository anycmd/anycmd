
namespace Anycmd.Engine.Host.Impl
{
    using Ac;
    using Ac.Identity;
    using Ac.Infra;
    using Ac.MemorySets;
    using Ac.MessageHandlers;
    using Anycmd.Rdb;
    using Bus;
    using Bus.DirectBus;
    using Edi;
    using Edi.Entities;
    using Edi.Handlers;
    using Edi.MessageHandlers;
    using Engine.Ac;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using Engine.Edi;
    using Engine.Edi.Messages;
    using Logging;
    using Query;
    using Rdb;
    using System;
    using Util;

    /// <summary>
    /// 系统实体宿主。
    /// </summary>
    public class DefaultAcDomain : AcDomain
    {
        public DefaultAcDomain()
        {
            base.MessageDispatcher = new MessageDispatcher();
            base.CommandBus = new DirectCommandBus(this.MessageDispatcher);
            this.EventBus = new DirectEventBus(this.MessageDispatcher);

            base.Rdbs = new Rdbs(this);
            base.DbTables = new DbTables(this);
            base.DbViews = new DbViews(this);
            base.DbTableColumns = new DbTableColumns(this);
            base.DbViewColumns = new DbViewColumns(this);
            base.AppSystemSet = new AppSystemSet(this);
            base.ButtonSet = new ButtonSet(this);
            base.SysUsers = new SysUserSet(this);
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

            new OntologyMessageHandler(this).Register();

            base.MessageDispatcher.Register(new AccountLoginedEventHandler(this));
            base.MessageDispatcher.Register(new AccountLogoutedEventHandler(this));
            base.MessageDispatcher.Register(new AddVisitingLogCommandHandler(this));
            base.MessageDispatcher.Register(new AddAccountCommandHandler(this));
            base.MessageDispatcher.Register(new UpdateAccountCommandHandler(this));
            base.MessageDispatcher.Register(new RemoveAccountCommandHandler(this));
            base.MessageDispatcher.Register(new AddPasswordCommandHandler(this));
            base.MessageDispatcher.Register(new ChangePasswordCommandHandler(this));
            base.MessageDispatcher.Register(new SaveHelpCommandHandler(this));

            this.MessageDispatcher.Register(new OperatedEventHandler(this));

            this.AddService(typeof(INodeHostBootstrap), new FastNodeHostBootstrap(this));
            this.MessageDispatcher.Register(new AddBatchCommandHandler(this));
            this.MessageDispatcher.Register(new UpdateBatchCommandHandler(this));
            this.MessageDispatcher.Register(new RemoveBatchCommandHandler(this));

            this.Map(EntityTypeMap.Create<Edi.Entities.Action>("Edi"));
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
            this.Map(EntityTypeMap.Create<PrivilegeBigram>("Ac"));
        }

        private void AddDefaultService<T>(object service)
        {
            if (this.GetService(typeof(T)) == null)
            {
                this.AddService(typeof(T), service);
            }
        }

        private class OntologyMessageHandler :
            IHandler<OntologyAddedEvent>,
            IHandler<OntologyUpdatedEvent>,
            IHandler<OntologyRemovedEvent>
        {
            private readonly IAcDomain _host;

            public OntologyMessageHandler(IAcDomain host)
            {
                this._host = host;
            }

            public void Register()
            {
                var messageDispatcher = _host.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of host:{0}".Fmt(_host.Name));
                }
                messageDispatcher.Register((IHandler<OntologyAddedEvent>)this);
                messageDispatcher.Register((IHandler<OntologyUpdatedEvent>)this);
                messageDispatcher.Register((IHandler<OntologyRemovedEvent>)this);
            }

            public void Handle(OntologyAddedEvent message)
            {
                var entityMenuId = new Guid("9BE27152-808E-427D-B8C6-D7ABC600A963");
                var entityMenu = new MenuCreateInput
                {
                    Id = entityMenuId,
                    ParentId = null,
                    Name = "实体管理",
                    Url = string.Empty,
                    Icon = string.Empty,
                    AppSystemId = _host.AppSystemSet.SelfAppSystem.Id,
                    Description = string.Empty,
                    SortCode = 0
                };
                MenuState parentMenu;
                if (!_host.MenuSet.TryGetMenu(entityMenuId, out parentMenu))
                {
                    _host.Handle(new AddMenuCommand(entityMenu));
                }
                OntologyDescriptor ontology;
                if (_host.NodeHost.Ontologies.TryGetOntology(message.Source.Id, out ontology))
                {
                    _host.Handle(new AddMenuCommand(new MenuCreateInput
                    {
                        Id = ontology.Ontology.Id,// 约定
                        ParentId = entityMenu.Id,
                        Name = ontology.Ontology.Name + "管理",
                        Url = string.Format("Edi/Entity/Index?ontologyCode={0}&ontologyID={1}", ontology.Ontology.Code, ontology.Ontology.Id),
                        Icon = ontology.Ontology.Icon,
                        AppSystemId = _host.AppSystemSet.SelfAppSystem.Id,
                        Description = string.Empty,
                        SortCode = ontology.Ontology.SortCode
                    }));
                }
            }

            public void Handle(OntologyUpdatedEvent message)
            {
                OntologyDescriptor ontology;
                if (_host.NodeHost.Ontologies.TryGetOntology(message.Source.Id, out ontology))
                {
                    MenuState menu;
                    if (_host.MenuSet.TryGetMenu(ontology.Ontology.Id, out menu))
                    {
                        _host.Handle(new UpdateMenuCommand(new MenuUpdateInput
                        {
                            Id = ontology.Ontology.Id,
                            AppSystemId = menu.AppSystemId,
                            Description = menu.Description,
                            Icon = menu.Icon,
                            Name = ontology.Ontology.Name + "管理",
                            SortCode = menu.SortCode,
                            Url = string.Format("Edi/Entity/Index?ontologyCode={0}&ontologyID={1}", ontology.Ontology.Code, ontology.Ontology.Id)
                        }));
                    }
                }
            }

            public void Handle(OntologyRemovedEvent message)
            {
                OntologyDescriptor ontology;
                if (_host.NodeHost.Ontologies.TryGetOntology(message.Source.Id, out ontology))
                {
                    MenuState menu;
                    if (_host.MenuSet.TryGetMenu(ontology.Ontology.Id, out menu))
                    {
                        _host.Handle(new RemoveMenuCommand(ontology.Ontology.Id));
                    }
                }
            }

            private class MenuCreateInput : IMenuCreateIo
            {
                public Guid? Id { get; set; }

                public Guid AppSystemId { get; set; }

                public string Description { get; set; }

                public string Icon { get; set; }

                public string Name { get; set; }

                public Guid? ParentId { get; set; }

                public int SortCode { get; set; }

                public string Url { get; set; }
            }

            private class MenuUpdateInput : IMenuUpdateIo
            {
                public Guid Id { get; set; }

                public Guid AppSystemId { get; set; }

                public string Description { get; set; }

                public string Icon { get; set; }

                public string Name { get; set; }

                public int SortCode { get; set; }

                public string Url { get; set; }
            }
        }
    }
}
