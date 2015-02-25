
namespace Anycmd.Mis.Web.Mvc
{
    using Ac.Queries.Ef;
    using Anycmd.Web;
    using Bus;
    using Edi.Application;
    using Edi.MessageServices;
    using Edi.Queries.Ef;
    using Ef;
    using Engine;
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
            base.AddService(typeof(IAcSessionStorage), new WebAcSessionStorage());
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
            private readonly IAcDomain _acDomain;

            public OntologyMessageHandler(IAcDomain acDomain)
            {
                this._acDomain = acDomain;
            }

            public void Register()
            {
                var messageDispatcher = _acDomain.MessageDispatcher;
                if (messageDispatcher == null)
                {
                    throw new ArgumentNullException("messageDispatcher has not be set of acDomain:{0}".Fmt(_acDomain.Name));
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
                    AppSystemId = _acDomain.AppSystemSet.SelfAppSystem.Id,
                    Description = string.Empty,
                    SortCode = 0
                };
                MenuState parentMenu;
                if (!_acDomain.MenuSet.TryGetMenu(entityMenuId, out parentMenu))
                {
                    _acDomain.Handle(new AddMenuCommand(message.AcSession, entityMenu));
                }
                OntologyDescriptor ontology;
                if (_acDomain.NodeHost.Ontologies.TryGetOntology(message.Source.Id, out ontology))
                {
                    _acDomain.Handle(new AddMenuCommand(message.AcSession, new MenuCreateInput
                    {
                        Id = ontology.Ontology.Id,// 约定
                        ParentId = entityMenu.Id,
                        Name = ontology.Ontology.Name + "管理",
                        Url = string.Format("Edi/Entity/Index?ontologyCode={0}&ontologyID={1}", ontology.Ontology.Code, ontology.Ontology.Id),
                        Icon = ontology.Ontology.Icon,
                        AppSystemId = _acDomain.AppSystemSet.SelfAppSystem.Id,
                        Description = string.Empty,
                        SortCode = ontology.Ontology.SortCode
                    }));
                }
            }

            public void Handle(OntologyUpdatedEvent message)
            {
                OntologyDescriptor ontology;
                if (_acDomain.NodeHost.Ontologies.TryGetOntology(message.Source.Id, out ontology))
                {
                    MenuState menu;
                    if (_acDomain.MenuSet.TryGetMenu(ontology.Ontology.Id, out menu))
                    {
                        _acDomain.Handle(new UpdateMenuCommand(message.AcSession, new MenuUpdateInput
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
                if (_acDomain.NodeHost.Ontologies.TryGetOntology(message.Source.Id, out ontology))
                {
                    MenuState menu;
                    if (_acDomain.MenuSet.TryGetMenu(ontology.Ontology.Id, out menu))
                    {
                        _acDomain.Handle(new RemoveMenuCommand(message.AcSession, ontology.Ontology.Id));
                    }
                }
            }

            private class MenuCreateInput : IMenuCreateIo
            {
                public MenuCreateInput()
                {
                    this.HecpOntology = "Menu";
                    this.HecpVerb = "Create";
                }

                public string HecpOntology { get; private set; }

                public string HecpVerb { get; private set; }

                public Guid? Id { get; set; }

                public Guid AppSystemId { get; set; }

                public string Description { get; set; }

                public string Icon { get; set; }

                public string Name { get; set; }

                public Guid? ParentId { get; set; }

                public int SortCode { get; set; }

                public string Url { get; set; }

                public IAnycmdCommand ToCommand(IAcSession acSession)
                {
                    return new AddMenuCommand(acSession, this);
                }
            }

            private class MenuUpdateInput : IMenuUpdateIo
            {
                public MenuUpdateInput()
                {
                    HecpOntology = "Menu";
                    HecpVerb = "Update";
                }

                public string HecpOntology { get; private set; }

                public string HecpVerb { get; private set; }

                public Guid Id { get; set; }

                public Guid AppSystemId { get; set; }

                public string Description { get; set; }

                public string Icon { get; set; }

                public string Name { get; set; }

                public int SortCode { get; set; }

                public string Url { get; set; }

                public IAnycmdCommand ToCommand(IAcSession acSession)
                {
                    return new UpdateMenuCommand(acSession, this);
                }
            }
        }
    }
}
