using System.Globalization;
using System.Text.RegularExpressions;

namespace Anycmd.Xacml.Runtime.DataTypes
{
    using Functions;
    using Interfaces;

    /// <summary>
    /// A class defining the YearMonthDuration data type.
    /// </summary>
    public class YearMonthDuration : IDataType
    {
        #region Private members

        /// <summary>
        /// The regular expression used to validate the year month duration as a string value.
        /// </summary>
        private const string PATTERN = @"[\-]?P[0-9]+(Y([0-9]+M)?|M)";

        /// <summary>
        /// The regular expression used to match the year month duration and extract some values.
        /// </summary>
        private const string PATTERN_MATCH = @"(?<n>[\-]?)P((?<y>\d+)Y)?((?<m>\d+)M)?";

        /// <summary>
        /// The years of this duration.
        /// </summary>
        private int _years;

        /// <summary>
        /// The months for this duration.
        /// </summary>
        private int _months;

        /// <summary>
        /// Whether this is a negative duration.
        /// </summary>
        private bool _negative;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal YearMonthDuration()
        {
        }

        /// <summary>
        /// Creates a new YearMonthDuration using the string value.
        /// </summary>
        /// <param name="value">The YearMonthDuration as a string.</param>
        public YearMonthDuration(string value)
        {
            Regex re = new Regex(PATTERN);
            Match m = re.Match(value);
            if (m.Success)
            {
                re = new Regex(PATTERN_MATCH);
                m = re.Match(value);
                if (m.Success)
                {
                    _negative = (m.Groups["n"].Value == "-");
                    _years = int.Parse(m.Groups["y"].Value, CultureInfo.InvariantCulture);
                    _months = int.Parse(m.Groups["m"].Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new EvaluationException(Properties.Resource.exc_bug);
                }
            }
            else
            {
                throw new EvaluationException(string.Format(Properties.Resource.exc_invalid_yearmonth_duration_value, value));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The years of this duration.
        /// </summary>
        public int Years
        {
            get { return _negative ? _years * -1 : _years; }
        }

        /// <summary>
        /// The months for this duration.
        /// </summary>
        public int Months
        {
            get { return _negative ? _months * -1 : _months; }
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
            YearMonthDuration compareTo = obj as YearMonthDuration;
            if (compareTo == null)
            {
                return base.Equals(obj);
            }

            return _months == compareTo._months && _years == compareTo._years && _negative == compareTo._negative;
        }

        #endregion

        #region IDataType Members

        /// <summary>
        /// Return the function that compares two values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction EqualFunction
        {
            get { return new YearMonthDurationEqual(); }
        }

        /// <summary>
        /// Return the function that verifies if a value is contained within a bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction IsInFunction
        {
            get { return new YearMonthDurationIsIn(); }
        }

        /// <summary>
        /// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction SubsetFunction
        {
            get { return new YearMonthDurationSubset(); }
        }

        /// <summary>
        /// The string representation of the data type constant.
        /// </summary>
        /// <value>A string with the Uri for the data type.</value>
        public string DataTypeName
        {
            get { return Consts.Schema1.InternalDataTypes.XQueryYearMonthDuration; }
        }

        /// <summary>
        /// Return an instance of an YearMonthDuration form the string specified.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="parNo">The parameter number being parsed.</param>
        /// <returns>An instance of the type.</returns>
        public object Parse(string value, int parNo)
        {
            return new YearMonthDuration(value);
        }

        #endregion
    }
}
