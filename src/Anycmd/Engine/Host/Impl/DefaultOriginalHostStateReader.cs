
namespace Anycmd.Engine.Host.Impl
{
    using Ac;
    using Ac.Identity;
    using Ac.Infra;
    using Ac.Rbac;
    using Engine.Ac.Abstractions;
    using Engine.Rdb;
    using Repositories;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Util;

    public class DefaultOriginalHostStateReader : IOriginalHostStateReader
    {
        /// <summary>
        /// 数据库连接字符串引导库连接字符串
        /// </summary>
        private readonly string _bootConnString = ConfigurationManager.AppSettings["BootDbConnString"];

        /// <summary>
        /// 数据库连接字符串引导库连接字符串
        /// </summary>
        public string BootConnString { get { return _bootConnString; } }

        private readonly IAcDomain _host;

        public DefaultOriginalHostStateReader(IAcDomain host)
        {
            this._host = host;
        }

        public IList<RDatabase> GetAllRDatabases()
        {
            return _host.RetrieveRequiredService<IRdbMetaDataService>().GetDatabases();
        }

        public IList<DbTableColumn> GetTableColumns(RdbDescriptor db)
        {
            return _host.RetrieveRequiredService<IRdbMetaDataService>().GetTableColumns(db);
        }

        public IList<DbTable> GetDbTables(RdbDescriptor db)
        {
            return _host.RetrieveRequiredService<IRdbMetaDataService>().GetDbTables(db);
        }

        public IList<DbViewColumn> GetViewColumns(RdbDescriptor db)
        {
            return _host.RetrieveRequiredService<IRdbMetaDataService>().GetViewColumns(db);
        }

        public IList<DbView> GetDbViews(RdbDescriptor db)
        {
            return _host.RetrieveRequiredService<IRdbMetaDataService>().GetDbViews(db);
        }

        public IList<Catalog> GetCatalogs()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Catalog>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<AppSystem> GetAllAppSystems()
        {
            var repository = _host.RetrieveRequiredService<IRepository<AppSystem>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Button> GetAllButtons()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Button>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Dic> GetAllDics()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Dic>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<DicItem> GetAllDicItems()
        {
            var repository = _host.RetrieveRequiredService<IRepository<DicItem>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<EntityType> GetAllEntityTypes()
        {
            var repository = _host.RetrieveRequiredService<IRepository<EntityType>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Property> GetAllProperties()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Property>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Function> GetAllFunctions()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Function>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Group> GetAllGroups()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Group>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Menu> GetAllMenus()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Menu>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<UiView> GetAllUiViews()
        {
            var repository = _host.RetrieveRequiredService<IRepository<UiView>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<UiViewButton> GetAllUiViewButtons()
        {
            var repository = _host.RetrieveRequiredService<IRepository<UiViewButton>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Privilege> GetPrivileges()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Privilege>>();
            using (var context = repository.Context)
            {
                var subjectType = UserAcSubjectType.Account.ToName();
                return repository.AsQueryable().Where(a=>a.SubjectType != subjectType).ToList();
            }
        }

        public IList<ResourceType> GetAllResources()
        {
            var repository = _host.RetrieveRequiredService<IRepository<ResourceType>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Role> GetAllRoles()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Role>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<SsdSet> GetAllSsdSets()
        {
            var repository = _host.RetrieveRequiredService<IRepository<SsdSet>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<DsdSet> GetAllDsdSets()
        {
            var repository = _host.RetrieveRequiredService<IRepository<DsdSet>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<SsdRole> GetAllSsdRoles()
        {
            var repository = _host.RetrieveRequiredService<IRepository<SsdRole>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<DsdRole> GetAllDsdRoles()
        {
            var repository = _host.RetrieveRequiredService<IRepository<DsdRole>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToList();
            }
        }

        public IList<Account> GetAllDevAccounts()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Account>>();
            using (var context = repository.Context)
            {
                return repository.Context.Query<DeveloperId>().Join(repository.Context.Query<Account>(), d => d.Id, a => a.Id, (d, a) => a).ToList();
            }
        }
    }
}
