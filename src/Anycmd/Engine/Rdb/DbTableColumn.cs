
namespace Anycmd.Engine.Rdb
{
    using System;
    using System.Data;
    using Util;

    /// <summary>
    /// 表列
    /// </summary>
    public sealed class DbTableColumn
    {
        private DbTableColumn() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseId"></param>
        /// <param name="reader"></param>
        internal static DbTableColumn Create(Guid databaseId, IDataRecord reader)
        {
            return new DbTableColumn
            {
                DatabaseId = databaseId,
                Id = reader.GetString(reader.GetOrdinal("Id")),
                CatalogName = reader.GetString(reader.GetOrdinal("CatalogName")),
                DateTimePrecision = reader.GetNullableInt32("DateTimePrecision"),
                DefaultValue = reader.GetNullableString("DefaultValue"),
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
                TableName = reader.GetString(reader.GetOrdinal("TableName")),
                TypeName = reader.GetString(reader.GetOrdinal("TypeName"))
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
        public string TableName { get; private set; }
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
        public string DefaultValue { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataColumn ToDataColumn()
        {
            var col = new DataColumn(this.Name);

            switch (this.TypeName)
            {
                case "uniqueidentifier":
                    col.DataType = typeof(Guid);
                    break;
                case "varchar":
                case "nvarchar":
                case "nvarchar(max)":
                case "varchar(max)":
                    col.DataType = typeof(string);
                    if (this.MaxLength.HasValue && this.MaxLength.Value != -1)
                    {
                        col.MaxLength = this.MaxLength.Value;
                    }
                    else
                    {
                        col.MaxLength = int.MaxValue;
                    }
                    break;
                case "datetime":
                case "date":
                    col.DataType = typeof(DateTime);
                    break;
                case "int":
                    col.DataType = typeof(int);
                    break;
                case "bit":
                    col.DataType = typeof(Boolean);
                    break;
                default:
                    break;
            }
            col.Caption = this.Name;
            return col;
        }
    }
}
