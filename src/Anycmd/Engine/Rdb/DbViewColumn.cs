
namespace Anycmd.Engine.Rdb
{
    using System;
    using System.Data;
    using Util;

    /// <summary>
    /// 视图列
    /// </summary>
    public sealed class DbViewColumn
    {
        private DbViewColumn() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="reader"></param>
        internal static DbViewColumn Create(Guid databaseId, IDataRecord reader)
        {
            return new DbViewColumn
            {
                DatabaseId = databaseId,
                Id = reader.GetString(reader.GetOrdinal("Id")),
                CatalogName = reader.GetString(reader.GetOrdinal("CatalogName")),
                DateTimePrecision = reader.GetNullableInt32("DateTimePrecision"),
                Description = reader.GetNullableString("Description"),
                IsIdentity = reader.GetBoolean(reader.GetOrdinal("IsIdentity")),
                IsNullable = reader.GetBoolean(reader.GetOrdinal("IsNullable")),
                IsPrimaryKey = reader.GetBoolean(reader.GetOrdinal("IsPrimaryKey")),
                IsStoreGenerated = reader.GetBoolean(reader.GetOrdinal("IsStoreGenerated")),
                MaxLength = reader.GetNullableInt32("MaxLength"),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Ordinal = reader.GetInt32(reader.GetOrdinal("Ordinal")),
                Precision = reader.GetNullableInt32("Precision"),
                Scale = reader.GetNullableInt32("Scale"),
                SchemaName = reader.GetString(reader.GetOrdinal("SchemaName")),
                TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
                ViewName = reader.GetString(reader.GetOrdinal("ViewName"))
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid DatabaseId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public System.String Id { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Ordinal { get; private set; }
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
        public string ViewName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsNullable { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int? MaxLength { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Precision { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int? DateTimePrecision { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Scale { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsIdentity { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsStoreGenerated { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsPrimaryKey { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
