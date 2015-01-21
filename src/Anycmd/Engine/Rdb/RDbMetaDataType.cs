
namespace Anycmd.Engine.Rdb
{

    /// <summary>
    /// 数据库元数据类型
    /// </summary>
    public enum RDbMetaDataType
    {
        Undefined = 0,
        /// <summary>
        /// 数据库实例
        /// </summary>
        Database = 1,
        /// <summary>
        /// 数据库表
        /// </summary>
        Table = 2,
        /// <summary>
        /// 数据库视图
        /// </summary>
        View = 3,
        /// <summary>
        /// 表列
        /// </summary>
        TableColumn = 4,
        /// <summary>
        /// 视图列
        /// </summary>
        ViewColumn = 5,
        /// <summary>
        /// 
        /// </summary>
        Procedure = 6,
        /// <summary>
        /// 
        /// </summary>
        Function = 7
    }
}
