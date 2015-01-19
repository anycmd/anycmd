
namespace Anycmd.Mis.Web.Mvc
{
    using Ac.Queries.Ef.Identity;
    using Anycmd.Web;
    using Bus;
    using Edi.Application;
    using Edi.MessageServices;
    using Edi.Queries.Ef;
    using Ef;
    using Engine.Ac;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages.Infra;
    using Engine.Edi;
    using Engine.Edi.Messages;
    using Engine.Host;
    using Engine.Host.Impl;
    using Logging;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Util;



    public class MisAcDomain : DefaultAcDomain
    {
        internal MisAcDomain(HttpApplication application)
        {
            application.Application.Add(Constants.ApplicationRuntime.AcDomainCacheKey, this);
        }

        public override void Configure()
        {
            base.Configure();
            base.AddService(typeof(IFunctionListImport), new FunctionListImport());
            base.AddService(typeof(IEfFilterStringBuilder), new EfFilterStringBuilder());
            base.AddService(typeof(ILoggingService), new Log4NetLoggingService(this));
            base.AddService(typeof(IUserSessionStorage), new WebUserSessionStorage());
            this.RegisterRepository(new List<string>
            {
                "EdiEntities",
                "AcEntities",
                "InfraEntities",
                "IdentityEntities"
            }, typeof(AcDomain).Assembly);
            this.RegisterQuery(typeof(BatchQuery).Assembly, typeof(AccountQuery).Assembly);
            this.RegisterEdiCore();
            new OntologyMessageHandler(this).Register();
        }

        public override AcDomain Init()
        {
            base.Init();
            (new ServiceHost(this, "", typeof(MessageService).Assembly)).Init();
            return this;
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
                    _host.Handle(new AddMenuCommand(message.UserSession, entityMenu));
                }
                OntologyDescriptor ontology;
                if (_host.NodeHost.Ontologies.TryGetOntology(message.Source.Id, out ontology))
                {
                    _host.Handle(new AddMenuCommand(message.UserSession, new MenuCreateInput
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
                        _host.Handle(new UpdateMenuCommand(message.UserSession, new MenuUpdateInput
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
                        _host.Handle(new RemoveMenuCommand(message.UserSession, ontology.Ontology.Id));
                    }
                }
            }

            private class MenuCreateInput : IMenuCreateIo
            {
                public MenuCreateInput()
                {
                    this.OntologyCode = "Menu";
                    this.Verb = "Create";
                }

                public string OntologyCode { get; private set; }

                public string Verb { get; private set; }

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
                public MenuUpdateInput()
                {
                    OntologyCode = "Menu";
                    Verb = "Update";
                }

                public string OntologyCode { get; private set; }

                public string Verb { get; private set; }

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
