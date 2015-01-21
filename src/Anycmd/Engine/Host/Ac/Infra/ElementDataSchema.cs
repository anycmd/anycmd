
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Rdb;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementDataSchema
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        public ElementDataSchema(DbTableColumn col)
        {
            this.IsNullable = col.IsNullable;
            this.TypeName = col.TypeName;
            this.MaxLength = col.MaxLength;
        }

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
    }
}
