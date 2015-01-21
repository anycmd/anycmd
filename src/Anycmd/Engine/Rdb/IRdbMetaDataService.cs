
namespace Anycmd.Engine.Rdb
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是关系数据库元数据提供程序。提供访问数据库表、视图、列元数据的方法。
    /// </summary>
    public interface IRdbMetaDataService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        IList<DbTableSpace> GetTableSpaces(RdbDescriptor db, string sortField, string sortOrder);

        #region database
        /// <summary>
        /// 获取给定Id的数据库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RDatabase GetDatabase(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataSource"></param>
        /// <param name="description"></param>
        void UpdateDatabase(Guid id, string dataSource, string description);

        /// <summary>
        /// 获取模板数据库列表
        /// </summary>
        /// <returns></returns>
        IList<RDatabase> GetDatabases();
        #endregion

        #region table
        /// <summary>
        /// 获取给定数据的表
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IList<DbTable> GetDbTables(RdbDescriptor db);
        #endregion

        #region view

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        string GetViewDefinition(RdbDescriptor db, DbView view);

        /// <summary>
        /// 获取给定数据库的视图
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IList<DbView> GetDbViews(RdbDescriptor db);
        #endregion

        #region tableColumn
        /// <summary>
        /// 获取给定数据库的表列
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IList<DbTableColumn> GetTableColumns(RdbDescriptor db);
        #endregion

        #region viewColumn
        /// <summary>
        /// 获取给定数据库的视图列
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IList<DbViewColumn> GetViewColumns(RdbDescriptor db);
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="metaDataType"></param>
        /// <param name="id"></param>
        /// <param name="description"></param>
        void CrudDescription(RdbDescriptor db, RDbMetaDataType metaDataType, string id, string description);
    }
}
