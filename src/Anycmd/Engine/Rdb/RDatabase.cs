
namespace Anycmd.Engine.Rdb
{
    using System;

    /// <summary>
    /// SQLServer数据库实体
    /// </summary>
    public sealed class RDatabase : IRDatabase
    {
        public RDatabase() { }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTemplate { get; set; }

        /// <summary>
        /// 数据库标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 类别码
        /// </summary>
        public string RdbmsType { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 数据库连接用户名
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 数据库连接用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 数据库连接属性
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// 数据访问提供程序名称
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public Guid? CreateUserId { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public Guid? ModifiedUserId { get; set; }
    }
}
