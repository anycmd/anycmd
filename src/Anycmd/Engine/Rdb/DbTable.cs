
namespace Anycmd.Engine.Rdb
{
    using System;
    using System.Data;
    using Util;

    /// <summary>
    /// 数据库表
    /// </summary>
    public sealed class DbTable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="id"></param>
        /// <param name="catalogName"></param>
        /// <param name="schemaName"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        internal DbTable(Guid databaseId, string id, string catalogName, string schemaName, string name, string description)
        {
            this.DatabaseId = databaseId;
            this.Id = id;
            this.CatalogName = catalogName;
            this.SchemaName = schemaName;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="reader"></param>
        internal DbTable(Guid databaseId, IDataRecord reader)
            : this(databaseId, reader.GetString(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("CatalogName")),
                reader.GetString(reader.GetOrdinal("SchemaName")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetNullableString("Description"))
        {
        }

        /// <summary>
        /// 数据库实体标识
        /// </summary>
        public Guid DatabaseId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public String Id { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string CatalogName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string SchemaName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
