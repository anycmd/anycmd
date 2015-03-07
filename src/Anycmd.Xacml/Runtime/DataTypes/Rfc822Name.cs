using System;
using System.Globalization;

namespace Anycmd.Xacml.Runtime.DataTypes
{
    using Functions;
    using Interfaces;

    /// <summary>
    /// A class defining the Rfc822Name data type.
    /// <remarks>µç×ÓÓÊ¼þ ID</remarks>
    /// </summary>
    public class Rfc822Name : IDataType
    {
        #region Private members

        /// <summary>
        /// The full name.
        /// </summary>
        private string _fullName;

        /// <summary>
        /// The domain part of the name.
        /// </summary>
        private string _domainPart;

        /// <summary>
        /// The local part of the name.
        /// </summary>
        private string _localPart;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal Rfc822Name()
        {
        }

        /// <summary>
        /// Creates a new Rfc822Name using the name as a string.
        /// </summary>
        /// <param name="name">The name as a string.</param>
        public Rfc822Name(string name)
        {
            if (name == null || name.Length == 0) throw new ArgumentNullException("name");
            int atIdx = name.IndexOf('@');
            if (atIdx != -1)
            {
                _localPart = name.Substring(0, atIdx);
                _domainPart = name.Substring(atIdx + 1).ToLower(CultureInfo.InvariantCulture);
                _fullName = _localPart + "@" + _domainPart;
            }
            else
            {
                _domainPart = name.Substring(atIdx + 1).ToLower(CultureInfo.InvariantCulture);
                _fullName = _domainPart;
            }
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
            Rfc822Name compareTo = obj as Rfc822Name;
            if (compareTo == null)
            {
                return base.Equals(obj);
            }

            return (_fullName == compareTo._fullName);
        }


        /// <summary>
        /// Matches this full name with the name provided as a parameter.
        /// </summary>
        /// <param name="compareTo">A name to compare with.</param>
        /// <returns>true, if the names match, otherwise false.</returns>
        public bool Matches(string compareTo)
        {
            if (compareTo == null || compareTo.Length == 0) throw new ArgumentNullException("compareTo");
            int atIdx = compareTo.IndexOf('@');
            if (atIdx == -1)
            {
                // Match domainName only
                return (_domainPart == compareTo);
            }
            else
            {
                //Match fullName
                return (_fullName == compareTo);
            }
        }
        #endregion

        #region IDataType Members

        /// <summary>
        /// Return the function that compares two values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction EqualFunction
        {
            get { return new Rfc822NameEqual(); }
        }

        /// <summary>
        /// Return the function that verifies if a value is contained within a bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction IsInFunction
        {
            get { return new Rfc822NameIsIn(); }
        }

        /// <summary>
        /// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction SubsetFunction
        {
            get { return new Rfc822NameSubset(); }
        }

        /// <summary>
        /// The string representation of the data type constant.
        /// </summary>
        /// <value>A string with the Uri for the data type.</value>
        public string DataTypeName
        {
            get { return Consts.Schema1.InternalDataTypes.Rfc822Name; }
        }

        /// <summary>
        /// Return an instance of an Rfc822Name form the string specified.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="parNo">The parameter number being parsed.</param>
        /// <returns>An instance of the type.</returns>
        public object Parse(string value, int parNo)
        {
            try
            {
                return new Rfc822Name(value);
            }
            catch (Exception e)
            {
                throw new EvaluationException(string.Format(Properties.Resource.exc_invalid_datatype_in_stringvalue, parNo, DataTypeName), e);
            }
        }

        #endregion
    }
}
