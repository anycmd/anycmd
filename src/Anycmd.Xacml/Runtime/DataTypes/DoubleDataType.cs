using System;
using System.Xml;

namespace Anycmd.Xacml.Runtime.DataTypes
{
    using Functions;
    using Interfaces;

    /// <summary>
    /// A class defining the Double data type.
    /// </summary>
    public class DoubleDataType : IDataType
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal DoubleDataType()
        {
        }

        #endregion

        #region IDataType members

        /// <summary>
        /// Return the function that compares two values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction EqualFunction
        {
            get { return new DoubleEqual(); }
        }

        /// <summary>
        /// Return the function that verifies if a value is contained within a bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction IsInFunction
        {
            get { return new DoubleIsIn(); }
        }

        /// <summary>
        /// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction SubsetFunction
        {
            get { return new DoubleSubset(); }
        }

        /// <summary>
        /// The string representation of the data type constant.
        /// </summary>
        /// <value>A string with the Uri for the data type.</value>
        public string DataTypeName
        {
            get { return Consts.Schema1.InternalDataTypes.XsdDouble; }
        }

        /// <summary>
        /// Return an instance of an Double form the string specified.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="parNo">The parameter number being parsed.</param>
        /// <returns>An instance of the type.</returns>
        public object Parse(string value, int parNo)
        {
            try
            {
                return XmlConvert.ToDouble(value);
            }
            catch (Exception e)
            {
                throw new EvaluationException(string.Format(Properties.Resource.exc_invalid_datatype_in_stringvalue, parNo, DataTypeName), e);
            }
        }

        #endregion
    }
}
