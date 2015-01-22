using System;

namespace Anycmd.Xacml.Runtime.DataTypes
{
    using Functions;
    using Interfaces;

    /// <summary>
    /// A class defining the Base64Binary data type.
    /// </summary>
    public class Base64Binary : IDataType
    {
        #region Private members

        /// <summary>
        /// The byte array.
        /// </summary>
        private byte[] _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal Base64Binary()
        {
        }

        /// <summary>
        /// Creates a Base64Binary with the byte array.
        /// </summary>
        /// <param name="value">The byte array.</param>
        public Base64Binary(byte[] value)
        {
            _value = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// The HashCode method overloaded because of a compiler warning. The base class is called.
        /// </summary>
        /// <returns>The HashCode calculated at the base class.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Equals method overloaded to compare two values of the same data type.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>true, if both values are equals, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            Base64Binary compareTo = obj as Base64Binary;
            if (compareTo == null)
            {
                return base.Equals(obj);
            }

            if (_value.Length != compareTo._value.Length)
            {
                return false;
            }

            for (int idx = 0; idx < _value.Length; idx++)
            {
                if (_value[idx] != compareTo._value[idx])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region IDataType Members

        /// <summary>
        /// Return the function that compares two values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction EqualFunction
        {
            get { return new Base64BinaryEqual(); }
        }

        /// <summary>
        /// Return the function that verifies if a value is contained within a bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction IsInFunction
        {
            get { return new Base64BinaryIsIn(); }
        }

        /// <summary>
        /// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction SubsetFunction
        {
            get { return new Base64BinarySubset(); }
        }

        /// <summary>
        /// The string representation of the data type constant.
        /// </summary>
        /// <value>A string with the Uri for the data type.</value>
        public string DataTypeName
        {
            get { return Consts.Schema1.InternalDataTypes.XsdBase64Binary; }
        }

        /// <summary>
        /// Return an instance of an Base64Binary form the string specified.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>An instance of the type.</returns>
        /// <param name="parNo">The parameter number being parsed.</param>
        public object Parse(string value, int parNo)
        {
            return new Base64Binary(Convert.FromBase64String(value));
        }

        #endregion
    }
}
