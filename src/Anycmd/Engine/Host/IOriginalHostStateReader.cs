
namespace Anycmd.Engine.Host
{
    using Ac;
    using Ac.Identity;
    using Ac.Infra;
    using Ac.Rbac;
    using Engine.Rdb;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是用以读取系统原始状态的状态读取器。
    /// </summary>
    public interface IOriginalHostStateReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<RDatabase> GetAllRDatabases();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IList<DbTableColumn> GetTableColumns(RdbDescriptor db);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IList<DbTable> GetDbTables(RdbDescriptor db);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IList<DbViewColumn> GetViewColumns(RdbDescriptor db);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IList<DbView> GetDbViews(RdbDescriptor db);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Catalog> GetCatalogs();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<AppSystem> GetAllAppSystems();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Button> GetAllButtons();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<EntityType> GetAllEntityTypes();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Property> GetAllProperties();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Function> GetAllFunctions();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Group> GetAllGroups();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Menu> GetAllMenus();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<UiView> GetAllUiViews();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<UiViewButton> GetAllUiViewButtons();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Privilege> GetPrivileges();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Role> GetAllRoles();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<SsdSet> GetAllSsdSets();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<DsdSet> GetAllDsdSets();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<SsdRole> GetAllSsdRoles();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<DsdRole> GetAllDsdRoles();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Account> GetAllDevAccounts();
    }
}
