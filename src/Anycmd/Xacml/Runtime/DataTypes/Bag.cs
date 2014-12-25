using System;

namespace Anycmd.Xacml.Runtime.DataTypes
{
    using Interfaces;

    /// <summary>
    /// A class defining the Bag data type.
    /// </summary>
    public class Bag : IDataType
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal Bag()
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
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Return the function that verifies if a value is contained within a bag of values of this data type.
        /// </summary>
        /// <returns>An IFunction instance.</returns>
        public IFunction IsInFunction
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction SubsetFunction
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// The string representation of the data type constant.
        /// </summary>
        /// <value>A string with the Uri for the data type.</value>
        public string DataTypeName
        {
            get { return null; } //TODO: fix this.
        }

        /// <summary>
        /// Return an instance of an AnyUri form the string specified.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>An instance of the type.</returns>
        /// <param name="parNo">The parameter number being parsed.</param>
        public object Parse(string value, int parNo)
        {
            return null; //TODO: this is an internal error.
        }

        #endregion
    }
}
