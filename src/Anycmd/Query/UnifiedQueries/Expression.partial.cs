
namespace Anycmd.Query.UnifiedQueries
{
    using System;
    using System.Xml.Serialization;

    public partial class Expression
    {
        [XmlIgnore]
        public Type ClrType
        {
            get
            {
                var typeName = string.Format("System.{0}", this.typeField);
                return System.Type.GetType(typeName);
            }
        }

        public object GetValue()
        {
            switch (this.typeField)
            {
                case DataTypes.Boolean:
                    return Convert.ToBoolean(this.valueField);
                case DataTypes.Char:
                    return this.valueField[0];
                case DataTypes.Decimal:
                    return Convert.ToDecimal(this.valueField);
                case DataTypes.Double:
                    return Convert.ToDouble(this.valueField);
                case DataTypes.Float:
                    return Convert.ToSingle(this.valueField);
                case DataTypes.Int16:
                    return Convert.ToInt16(this.valueField);
                case DataTypes.Int32:
                    return Convert.ToInt32(this.valueField);
                case DataTypes.Int64:
                    return Convert.ToInt64(this.valueField);
                case DataTypes.String:
                    return this.valueField;
                case DataTypes.UInt16:
                    return Convert.ToUInt16(this.valueField);
                case DataTypes.UInt32:
                    return Convert.ToUInt32(this.valueField);
                case DataTypes.UInt64:
                    return Convert.ToUInt64(this.valueField);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("({0} {1} {2})", this.Name, this.Operator, this.Value);
        }
    }
}
