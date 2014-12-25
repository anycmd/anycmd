
namespace Anycmd.Tests
{
    using Engine.Host;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Engine.Host.Ac.Infra;
    using Engine.Host.Impl;
    using Logging;
    using Model;
    using Moq;
    using Rdb;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TestHelper
    {
        public static IAcDomain GetAcDomain()
        {
            var host = new MoqAcDomain();
            host.RegisterRepository(typeof(AcDomain).Assembly);
            host.AddService(typeof(ILoggingService), new Log4NetLoggingService(host));
            host.AddService(typeof(IUserSessionStorage), new SimpleUserSessionStorage());
            Guid accountId = Guid.NewGuid();
            host.GetRequiredService<IRepository<Account>>().Add(new Account
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
            host.GetRequiredService<IRepository<Account>>().Context.Commit();
            Guid appSystemId = Guid.NewGuid();
            host.GetRequiredService<IRepository<AppSystem>>().Add(new AppSystem
            {
                Id = appSystemId,
                Name = "test",
                Code = "test",
                PrincipalId = host.GetRequiredService<IRepository<Account>>().AsQueryable().First().Id
            });
            host.GetRequiredService<IRepository<AppSystem>>().Context.Commit();
            host.GetRequiredService<IRepository<ResourceType>>().Add(new ResourceType
                {
                    Code = "Resource1",
                    Id = Guid.NewGuid(),
                    Icon = string.Empty,
                    Description = string.Empty,
                    Name = "测试1",
                    SortCode = 10,
                    AppSystemId = appSystemId
                });
            host.GetRequiredService<IRepository<ResourceType>>().Context.Commit();
            host.RemoveService(typeof(IOriginalHostStateReader));
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
            moAcDomainBootstrap.Setup<IList<Organization>>(a => a.GetOrganizations()).Returns(host.GetRequiredService<IRepository<Organization>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<AppSystem>>(a => a.GetAllAppSystems()).Returns(host.GetRequiredService<IRepository<AppSystem>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Button>>(a => a.GetAllButtons()).Returns(host.GetRequiredService<IRepository<Button>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Dic>>(a => a.GetAllDics()).Returns(host.GetRequiredService<IRepository<Dic>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<DicItem>>(a => a.GetAllDicItems()).Returns(host.GetRequiredService<IRepository<DicItem>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<EntityType>>(a => a.GetAllEntityTypes()).Returns(host.GetRequiredService<IRepository<EntityType>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Property>>(a => a.GetAllProperties()).Returns(host.GetRequiredService<IRepository<Property>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Function>>(a => a.GetAllFunctions()).Returns(host.GetRequiredService<IRepository<Function>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Group>>(a => a.GetAllGroups()).Returns(host.GetRequiredService<IRepository<Group>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Menu>>(a => a.GetAllMenus()).Returns(host.GetRequiredService<IRepository<Menu>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<UiView>>(a => a.GetAllUiViews()).Returns(host.GetRequiredService<IRepository<UiView>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<UiViewButton>>(a => a.GetAllUiViewButtons()).Returns(host.GetRequiredService<IRepository<UiViewButton>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<PrivilegeBigram>>(a => a.GetPrivilegeBigrams()).Returns(host.GetRequiredService<IRepository<PrivilegeBigram>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<ResourceType>>(a => a.GetAllResources()).Returns(host.GetRequiredService<IRepository<ResourceType>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Role>>(a => a.GetAllRoles()).Returns(host.GetRequiredService<IRepository<Role>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<SsdSet>>(a => a.GetAllSsdSets()).Returns(host.GetRequiredService<IRepository<SsdSet>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<DsdSet>>(a => a.GetAllDsdSets()).Returns(host.GetRequiredService<IRepository<DsdSet>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<SsdRole>>(a => a.GetAllSsdRoles()).Returns(host.GetRequiredService<IRepository<SsdRole>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<DsdRole>>(a => a.GetAllDsdRoles()).Returns(host.GetRequiredService<IRepository<DsdRole>>().AsQueryable().ToList());
            moAcDomainBootstrap.Setup<IList<Account>>(a => a.GetAllDevAccounts()).Returns(host.GetRequiredService<IRepository<Account>>().AsQueryable().ToList());
            host.AddService(typeof(IOriginalHostStateReader), moAcDomainBootstrap.Object);
            host.Init();

            return host;
        }

        public static Mock<TRepository> GetMoqRepository<TEntity, TRepository>(this IAcDomain host)
            where TEntity : class, IAggregateRoot
            where TRepository : class, IRepository<TEntity>
        {

            var moRepository = new Mock<TRepository>();
            var context = new MoqRepositoryContext(host);
            moRepository.Setup<IRepositoryContext>(a => a.Context).Returns(context);
            moRepository.Setup(a => a.Add(It.IsAny<TEntity>()));
            moRepository.Setup(a => a.Remove(It.IsAny<TEntity>()));
            moRepository.Setup(a => a.Update(It.IsAny<TEntity>()));
            moRepository.Setup<TEntity>(a => a.GetByKey(It.IsAny<Guid>())).Returns((TEntity)null);
            moRepository.Setup<IQueryable<TEntity>>(a => a.AsQueryable()).Returns(new List<TEntity>().AsQueryable());

            return moRepository;
        }

        public static void RegisterRepository(this IAcDomain host, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(IAggregateRoot).IsAssignableFrom(type))
                    {
                        var repositoryType = typeof(MoqCommonRepository<>);
                        var genericInterface = typeof(IRepository<>);
                        repositoryType = repositoryType.MakeGenericType(type);
                        genericInterface = genericInterface.MakeGenericType(type);
                        var repository = Activator.CreateInstance(repositoryType, host);
                        host.AddService(genericInterface, repository);
                    }
                }
            }
        }
    }
}
