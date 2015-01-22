

namespace Anycmd.Xacml.Runtime
{
    using DataTypes;

    /// <summary>
    /// Helper class used to find the internal descriptor for the data types found in the 
    /// context or policy documents.
    /// </summary>
    public sealed class DataTypeDescriptor
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        private DataTypeDescriptor()
        {
        }

        #endregion

        #region Private members

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static FunctionDataType _function = new FunctionDataType();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static AnyUri _anyUri = new AnyUri();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static Base64Binary _base64Binary = new Base64Binary();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static HexBinary _hexBinary = new HexBinary();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static BooleanDataType _boolean = new BooleanDataType();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static IntegerDataType _integer = new IntegerDataType();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static DateDataType _date = new DateDataType();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static DateTime _dateTime = new DateTime();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static DaytimeDuration _dayTimeDuration = new DaytimeDuration();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static YearMonthDuration _yearMonthDuration = new YearMonthDuration();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static DoubleDataType _double = new DoubleDataType();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static Rfc822Name _rfc822Name = new Rfc822Name();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static X500Name _x500Name = new X500Name();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static StringDataType _string = new StringDataType();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static Time _time = new Time();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static Bag _bag = new Bag();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static DnsNameDataType _dnsName = new DnsNameDataType();

        /// <summary>
        /// The only instance of the internal data type.
        /// </summary>
        private static IpAddress _ipAddress = new IpAddress();

        #endregion

        #region Public properties

        /// <summary>
        /// The Function data type descriptor.
        /// </summary>
        static public FunctionDataType Function
        {
            get { return _function; }
        }

        /// <summary>
        /// The AnyUri data type descriptor.
        /// </summary>
        static public AnyUri AnyUri
        {
            get { return _anyUri; }
        }

        /// <summary>
        /// The Base64Binary data type descriptor.
        /// </summary>
        static public Base64Binary Base64Binary
        {
            get { return _base64Binary; }
        }

        /// <summary>
        /// The HexBinary data type descriptor.
        /// </summary>
        static public HexBinary HexBinary
        {
            get { return _hexBinary; }
        }

        /// <summary>
        /// The Boolean data type descriptor.
        /// </summary>
        static public BooleanDataType Boolean
        {
            get { return _boolean; }
        }

        /// <summary>
        /// The Integer data type descriptor.
        /// </summary>
        static public IntegerDataType Integer
        {
            get { return _integer; }
        }

        /// <summary>
        /// The Date data type descriptor.
        /// </summary>
        static public DateDataType Date
        {
            get { return _date; }
        }

        /// <summary>
        /// The DateTime data type descriptor.
        /// </summary>
        static public DateTime DateTime
        {
            get { return _dateTime; }
        }

        /// <summary>
        /// The DaytimeDuration data type descriptor.
        /// </summary>
        static public DaytimeDuration DaytimeDuration
        {
            get { return _dayTimeDuration; }
        }

        /// <summary>
        /// The YearMonthDuration data type descriptor.
        /// </summary>
        static public YearMonthDuration YearMonthDuration
        {
            get { return _yearMonthDuration; }
        }

        /// <summary>
        /// The Double data type descriptor.
        /// </summary>
        static public DoubleDataType Double
        {
            get { return _double; }
        }

        /// <summary>
        /// The X500Name data type descriptor.
        /// </summary>
        static public X500Name X500Name
        {
            get { return _x500Name; }
        }

        /// <summary>
        /// The Rfc822Name data type descriptor.
        /// </summary>
        static public Rfc822Name Rfc822Name
        {
            get { return _rfc822Name; }
        }

        /// <summary>
        /// The String data type descriptor.
        /// </summary>
        static public StringDataType String
        {
            get { return _string; }
        }

        /// <summary>
        /// The Time data type descriptor.
        /// </summary>
        static public Time Time
        {
            get { return _time; }
        }

        /// <summary>
        /// The Bag data type descriptor.
        /// </summary>
        static public Bag Bag
        {
            get { return _bag; }
        }

        /// <summary>
        /// The ipAddress data type descriptor.
        /// </summary>
        static public IpAddress IPAddress
        {
            get { return _ipAddress; }
        }

        /// <summary>
        /// The DnsName data type descriptor.
        /// </summary>
        static public DnsNameDataType DnsName
        {
            get { return _dnsName; }
        }
        #endregion
    }
}