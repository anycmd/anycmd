
namespace Anycmd.Tests
{
    using Engine.Ac;
    using Engine.Host;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Engine.Host.Ac.Infra;
    using Engine.Host.Ac.Rbac;
    using Engine.Host.Impl;
    using Logging;
    using Moq;
    using Rdb;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MoqAcDomain : DefaultAcDomain
    {
        public MoqAcDomain()
        {
            UserSessionState.SignOuted = OnSignOuted;
            UserSessionState.GetAccountById = GetAccountById;
            UserSessionState.GetAccountByLoginName = GetAccountByLoginName;
        }

        public override void Configure()
        {
            base.Configure();
            this.RegisterRepository(typeof(AcDomain).Assembly);
            AddService(typeof(ILoggingService), new Log4NetLoggingService(this));
            AddService(typeof(IUserSessionStorage), new SimpleUserSessionStorage());
            var accountId = Guid.NewGuid();
            this.GetRequiredService<IRepository<Account>>().Add(new Account
            {
                Id = accountId,
                LoginName = "LoginName1",
                Password = "111111",
                AuditState = string.Empty,
                BackColor = string.Empty,
                AllowEndTime = null,
                AllowStartTime = null,
                AnswerQuestion = string.Empty,
                Description = string.Empty,
                FirstLoginOn = null,
                DeletionStateCode = 0,
                IpAddress = string.Empty,
                Lang = string.Empty,
                IsEnabled = 1,
                LastPasswordChangeOn = null,
                LockEndTime = null,
                LockStartTime = null,
                LoginCount = null,
                MacAddress = string.Empty,
                OpenId = string.Empty,
                PreviousLoginOn = null,
                NumberId = 10,
                Question = string.Empty,
                Theme = string.Empty,
                Wallpaper = string.Empty,
                SecurityLevel = 0,
                Code = "user1",
                CommunicationPassword = string.Empty,
                Email = string.Empty,
                Mobile = string.Empty,
                PublicKey = string.Empty,
                Qq = string.Empty,
                Name = "user1",
                QuickQuery = string.Empty,
                QuickQuery1 = string.Empty,
                QuickQuery2 = string.Empty,
                SignedPassword = string.Empty,
                Telephone = string.Empty,
                OrganizationCode = string.Empty
            });
            this.GetRequiredService<IRepository<Account>>().Context.Commit();
            var appSystemId = Guid.NewGuid();
            this.GetRequiredService<IRepository<AppSystem>>().Add(new AppSystem
            {
                Id = appSystemId,
                Name = "test",
                Code = "test",
                PrincipalId = this.GetRequiredService<IRepository<Account>>().AsQueryable().First().Id
            });
            this.GetRequiredService<IRepository<AppSystem>>().Context.Commit();
            this.GetRequiredService<IRepository<ResourceType>>().Add(new ResourceType
            {
                Code = "Resource1",
                Id = Guid.NewGuid(),
                Icon = string.Empty,
                Description = string.Empty,
                Name = "测试1",
                SortCode = 10,
                AppSystemId = appSystemId
            });
            this.GetRequiredService<IRepository<ResourceType>>().Context.Commit();
            RemoveService(typeof(IOriginalHostStateReader));
            var moAcDomainBootstrap = new Mock<IOriginalHostStateReader>();
            moAcDomainBootstrap.Setup<IList<RDatabase>>(a => a.GetAllRDatabases()).Returns(new List<RDatabase>
            {
                new RDatabase
                {
                    Id=Guid.NewGuid(),
                    CatalogName="test",
                    DataSource=".",
                    Description="test",
                    IsTemplate=false,
                    Password=string.Empty,
                    Profile=string.Empty,
                    UserId=string.Empty,
                    RdbmsType="SqlServer",
                    ProviderName=string.Empty
                }
            });
            moAcDomainBootstrap.Setup<IList<DbTableColumn>>(a => a.GetTableColumns(It.IsAny<RdbDescriptor>())).Returns(new List<DbTableColumn>());
            moAcDomainBootstrap.Setup<IList<DbTable>>(a => a.GetDbTables(It.IsAny<RdbDescriptor>())).Returns(new List<DbTable>());
            moAcDomainBootstrap.Setup<IList<DbViewColumn>>(a => a.GetViewColumns(It.IsAny<RdbDescriptor>())).Returns(new List<DbViewColumn>());
            moAcDomainBootstrap.Setup<IList<DbView>>(a => a.GetDbViews(It.IsAny<RdbDescriptor>())).Returns(new List<DbView>());
            moAcDomainBootstrap.Setup<IList<Organization>>(a => a.GetOrganizations()).Returns(this.GetRequiredService<IRepository<Organization>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<AppSystem>>(a => a.GetAllAppSystems()).Returns(this.GetRequiredService<IRepository<AppSystem>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Button>>(a => a.GetAllButtons()).Returns(this.GetRequiredService<IRepository<Button>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Dic>>(a => a.GetAllDics()).Returns(this.GetRequiredService<IRepository<Dic>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<DicItem>>(a => a.GetAllDicItems()).Returns(this.GetRequiredService<IRepository<DicItem>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<EntityType>>(a => a.GetAllEntityTypes()).Returns(this.GetRequiredService<IRepository<EntityType>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Property>>(a => a.GetAllProperties()).Returns(this.GetRequiredService<IRepository<Property>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Function>>(a => a.GetAllFunctions()).Returns(this.GetRequiredService<IRepository<Function>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Group>>(a => a.GetAllGroups()).Returns(this.GetRequiredService<IRepository<Group>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Menu>>(a => a.GetAllMenus()).Returns(this.GetRequiredService<IRepository<Menu>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<UiView>>(a => a.GetAllUiViews()).Returns(this.GetRequiredService<IRepository<UiView>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<UiViewButton>>(a => a.GetAllUiViewButtons()).Returns(this.GetRequiredService<IRepository<UiViewButton>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Privilege>>(a => a.GetPrivileges()).Returns(this.GetRequiredService<IRepository<Privilege>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<ResourceType>>(a => a.GetAllResources()).Returns(this.GetRequiredService<IRepository<ResourceType>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Role>>(a => a.GetAllRoles()).Returns(this.GetRequiredService<IRepository<Role>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<SsdSet>>(a => a.GetAllSsdSets()).Returns(this.GetRequiredService<IRepository<SsdSet>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<DsdSet>>(a => a.GetAllDsdSets()).Returns(this.GetRequiredService<IRepository<DsdSet>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<SsdRole>>(a => a.GetAllSsdRoles()).Returns(this.GetRequiredService<IRepository<SsdRole>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<DsdRole>>(a => a.GetAllDsdRoles()).Returns(this.GetRequiredService<IRepository<DsdRole>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Account>>(a => a.GetAllDevAccounts()).Returns(this.GetRequiredService<IRepository<Account>>().AsQueryable().ToList());
            AddService(typeof(IOriginalHostStateReader), moAcDomainBootstrap.Object);
        }

        private static void OnSignOuted(IAcDomain acDomain, Guid sessionId)
        {
            var repository = acDomain.GetRequiredService<IRepository<UserSession>>();
            var entity = repository.GetByKey(sessionId);
            if (entity == null) return;
            entity.IsAuthenticated = false;
            repository.Update(entity);
        }

        private static Account GetAccountById(IAcDomain acDomain, Guid accountId)
        {
            var repository = acDomain.GetRequiredService<IRepository<Account>>();
            return repository.GetByKey(accountId);
        }

        private static Account GetAccountByLoginName(IAcDomain acDomain, string loginName)
        {
            var repository = acDomain.GetRequiredService<IRepository<Account>>();
            return repository.AsQueryable().FirstOrDefault(a => string.Equals(a.LoginName, loginName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
