using System.Xml;

namespace Anycmd.Xacml.Runtime.DataTypes
{
    using Interfaces;

    /// <summary>
    /// A class defining the DateTime data type.
    /// </summary>
    public class DateTime : IDataType
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DateTime()
        {
        }

        #endregion

        #region IDataType Members

        /// <summary>
        /// Return the function that compares two values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction EqualFunction
        {
            get { return new Functions.DateTimeDataType.Equal(); }
        }

        /// <summary>
        /// Return the function that verifies if a value is contained within a bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction IsInFunction
        {
            get { return new Functions.DateTimeDataType.IsIn(); }
        }

        /// <summary>
        /// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction SubsetFunction
        {
            get { return new Functions.DateTimeDataType.Subset(); }
        }

        /// <summary>
        /// The string representation of the data type constant.
        /// </summary>
        /// <value>A string with the Uri for the data type.</value>
        public string DataTypeName
        {
            get { return Consts.Schema1.InternalDataTypes.XsdDateTime; }
        }

        /// <summary>
        /// Return an instance of an DateTime form the string specified.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="parNo">The parameter number being parsed.</param>
        /// <returns>An instance of the type.</returns>
        public object Parse(string value, int parNo)
        {
            return XmlConvert.ToDateTime(value, "yyyy-MM-ddTHH:mm:sszzzzzz");
        }

        #endregion
    }
}
