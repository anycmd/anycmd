
namespace Anycmd.Engine.Rdb
{
    using System;
    using System.Data;

    /// <summary>
    /// 表空间模型
    /// </summary>
    public sealed class DbTableSpace
    {
        private DbTableSpace(
            string name, Int64 rows, string reserved,
            string data, string indexSize, string unUsed)
        {
            this.Name = name;
            this.Rows = rows;
            this.Reserved = reserved;
            this.Data = data;
            this.IndexSize = indexSize;
            this.UnUsed = unUsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static DbTableSpace Create(IDataRecord reader)
        {
            return new DbTableSpace(
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetInt64(reader.GetOrdinal("Rows")),
                reader.GetString(reader.GetOrdinal("Reserved")),
                reader.GetString(reader.GetOrdinal("Data")),
                reader.GetString(reader.GetOrdinal("IndexSize")),
                reader.GetString(reader.GetOrdinal("UnUsed")));
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Int64 Rows { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Reserved { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Data { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string IndexSize { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string UnUsed { get; private set; }
    }
}
