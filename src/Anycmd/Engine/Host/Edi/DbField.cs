
namespace Anycmd.Engine.Host.Edi
{
    using System.Data;

    /// <summary>
    /// 
    /// </summary>
    public sealed class DbField
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="allowDbNull"></param>
        /// <param name="maxLength"></param>
        public DbField(string name, DbType dbType, bool allowDbNull, int? maxLength)
        {
            this.Name = name;
            this.DbType = dbType;
            this.AllowDbNull = allowDbNull;
            this.MaxLength = maxLength;
        }

        /// <summary>
        /// 字段名。如果是关系数据库则对应的是数据库表中的列名。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DbType DbType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowDbNull { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int? MaxLength { get; private set; }
    }
}
