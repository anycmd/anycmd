
namespace Anycmd.Xacml.Runtime
{
    using DataTypes;

    /// <summary>
    /// Helper class used to find the internal descriptor for the data types found in the 
    /// context or policy documents.
    /// </summary>
    public static class DataTypeDescriptor
    {
        #region Private members

        static DataTypeDescriptor()
        {
            DnsName = new DnsNameDataType();
            IpAddress = new IpAddress();
            Bag = new Bag();
            Time = new Time();
            String = new StringDataType();
            Rfc822Name = new Rfc822Name();
            X500Name = new X500Name();
            Double = new DoubleDataType();
            YearMonthDuration = new YearMonthDuration();
            DaytimeDuration = new DaytimeDuration();
            DateTime = new DateTime();
            Date = new DateDataType();
            Integer = new IntegerDataType();
            Boolean = new BooleanDataType();
            HexBinary = new HexBinary();
            Base64Binary = new Base64Binary();
            AnyUri = new AnyUri();
            Function = new FunctionDataType();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The Function data type descriptor.
        /// </summary>
        public static FunctionDataType Function { get; private set; }

        /// <summary>
        /// The AnyUri data type descriptor.
        /// </summary>
        public static AnyUri AnyUri { get; private set; }

        /// <summary>
        /// The Base64Binary data type descriptor.
        /// </summary>
        public static Base64Binary Base64Binary { get; private set; }

        /// <summary>
        /// The HexBinary data type descriptor.
        /// </summary>
        public static HexBinary HexBinary { get; private set; }

        /// <summary>
        /// The Boolean data type descriptor.
        /// </summary>
        public static BooleanDataType Boolean { get; private set; }

        /// <summary>
        /// The Integer data type descriptor.
        /// </summary>
        public static IntegerDataType Integer { get; private set; }

        /// <summary>
        /// The Date data type descriptor.
        /// </summary>
        public static DateDataType Date { get; private set; }

        /// <summary>
        /// The DateTime data type descriptor.
        /// </summary>
        public static DateTime DateTime { get; private set; }

        /// <summary>
        /// The DaytimeDuration data type descriptor.
        /// </summary>
        public static DaytimeDuration DaytimeDuration { get; private set; }

        /// <summary>
        /// The YearMonthDuration data type descriptor.
        /// </summary>
        public static YearMonthDuration YearMonthDuration { get; private set; }

        /// <summary>
        /// The Double data type descriptor.
        /// </summary>
        public static DoubleDataType Double { get; private set; }

        /// <summary>
        /// The X500Name data type descriptor.
        /// </summary>
        public static X500Name X500Name { get; private set; }

        /// <summary>
        /// The Rfc822Name data type descriptor.
        /// </summary>
        public static Rfc822Name Rfc822Name { get; private set; }

        /// <summary>
        /// The String data type descriptor.
        /// </summary>
        public static StringDataType String { get; private set; }

        /// <summary>
        /// The Time data type descriptor.
        /// </summary>
        public static Time Time { get; private set; }

        /// <summary>
        /// The Bag data type descriptor.
        /// </summary>
        public static Bag Bag { get; private set; }

        /// <summary>
        /// The ipAddress data type descriptor.
        /// </summary>
        public static IpAddress IpAddress { get; private set; }

        /// <summary>
        /// The DnsName data type descriptor.
        /// </summary>
        public static DnsNameDataType DnsName { get; private set; }

        #endregion
    }
}