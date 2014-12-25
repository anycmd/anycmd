using System.Globalization;
using System.Text.RegularExpressions;

namespace Anycmd.Xacml.Runtime.DataTypes
{
    using Functions;
    using Interfaces;

    /// <summary>
    /// A class defining the DaytimeDuration data type.
    /// </summary>
    public class DaytimeDuration : IDataType
    {
        #region Private members

        /// <summary>
        /// The regular expression used to validate the day time duration as a string value.
        /// </summary>
        private const string PATTERN = @"[\-]?P([0-9]+D(T([0-9]+(H([0-9]+(M([0-9]+(\.[0-9]*)?S|\.[0-9]+S)?|(\.[0-9]*)?S)|(\.[0-9]*)?S)?|M([0-9](\.[0-9]*)?S|\.[0-9]+S)?|(\.[0-9]*)?S)|\.[0-9]+S))?|T([0-9]+(H([0-9]+(M([0-9]+(\.[0-9]*)?S|\.[0-9]+S)?|(\.[0-9]*)?S)|(\.[0-9]*)?S)?|M([0-9]+(\.[0-9]*)?S|\.[0-9]+S)?|(\.[0-9]*)?S)|\.[0-9]+S))";

        /// <summary>
        /// The regular expression used to match the day time duration and extract some values.
        /// </summary>
        private const string PATTERN_MATCH = @"(?<n>[\-]?)P((?<d>(\d+|\.\d+|\d+\.\d+))D)?T((?<h>(\d+|\.\d+|\d+\.\d+))H)?((?<m>(\d+|\.\d+|\d+\.\d+))M)?((?<s>(\d+|\.\d+|\d+\.\d+))S)?";

        /// <summary>
        /// The original value found in the document.
        /// </summary>
        private string _durationValue;

        /// <summary>
        /// The days of the duration.
        /// </summary>
        private int _days;

        /// <summary>
        /// The hours of the duration.
        /// </summary>
        private int _hours;

        /// <summary>
        /// The minutes of the duration.
        /// </summary>
        private int _minutes;

        /// <summary>
        /// The seconds of the duration.
        /// </summary>
        private int _seconds;

        /// <summary>
        /// Whether is a negative duration.
        /// </summary>
        private bool _negative;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal DaytimeDuration()
        {
        }

        /// <summary>
        /// Creates a new DaytimeDuration using the string value.
        /// </summary>
        /// <param name="value">The DaytimeDuration as a string.</param>
        public DaytimeDuration(string value)
        {
            _durationValue = value;
            Regex re = new Regex(PATTERN);
            Match m = re.Match(value);
            if (m.Success)
            {
                re = new Regex(PATTERN_MATCH);
                m = re.Match(value);
                if (m.Success)
                {
                    _negative = (m.Groups["n"].Value == "-");
                    _days = int.Parse(m.Groups["d"].Value, CultureInfo.InvariantCulture);
                    _hours = int.Parse(m.Groups["h"].Value, CultureInfo.InvariantCulture);
                    _minutes = int.Parse(m.Groups["m"].Value, CultureInfo.InvariantCulture);
                    _seconds = int.Parse(m.Groups["s"].Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new EvaluationException(Resource.exc_bug);
                }
            }
            else
            {
                throw new EvaluationException(string.Format(Resource.exc_invalid_daytime_duration_value, value));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The days of the duration.
        /// </summary>
        public int Days
        {
            get { return _negative ? _days * -1 : _days; }
        }

        /// <summary>
        /// The hours of the duration.
        /// </summary>
        public int Hours
        {
            get { return _negative ? _hours * -1 : _hours; }
        }

        /// <summary>
        /// The minutes of the duration.
        /// </summary>
        public int Minutes
        {
            get { return _negative ? _minutes * -1 : _minutes; }
        }

        /// <summary>
        /// The seconds of the duration.
        /// </summary>
        public int Seconds
        {
            get { return _negative ? _seconds * -1 : _seconds; }
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
            DaytimeDuration compareTo = obj as DaytimeDuration;
            if (compareTo == null)
            {
                return base.Equals(obj);
            }

            return _days == compareTo._days && _hours == compareTo._hours && _minutes == compareTo._minutes && _seconds == compareTo._seconds && _negative == compareTo._negative;
        }

        #endregion

        #region IDataType Members

        /// <summary>
        /// Return the function that compares two values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction EqualFunction
        {
            get { return new DaytimeDurationEqual(); }
        }

        /// <summary>
        /// Return the function that verifies if a value is contained within a bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction IsInFunction
        {
            get { return new DaytimeDurationIsIn(); }
        }

        /// <summary>
        /// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
        /// </summary>
        /// <value>An IFunction instance.</value>
        public IFunction SubsetFunction
        {
            get { return new DaytimeDurationSubset(); }
        }

        /// <summary>
        /// The string representation of the data type constant.
        /// </summary>
        /// <value>A string with the Uri for the data type.</value>
        public string DataTypeName
        {
            get { return Consts.Schema1.InternalDataTypes.XQueryDaytimeDuration; }
        }

        /// <summary>
        /// Return an instance of an DaytimeDuration form the string specified.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <param name="parNo">The parameter number being parsed.</param>
        /// <returns>An instance of the type.</returns>
        public object Parse(string value, int parNo)
        {
            return new DaytimeDuration(value);
        }

        #endregion
    }
}
