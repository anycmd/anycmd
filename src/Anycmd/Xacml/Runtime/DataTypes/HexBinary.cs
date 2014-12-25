using System;
using System.Globalization;

namespace Anycmd.Xacml.Runtime.DataTypes
{
	using Functions;
	using Interfaces;

	/// <summary>
	/// A class defining the HexBinary data type.
	/// </summary>
	public class HexBinary : IDataType
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
		internal HexBinary()
		{
		}

		/// <summary>
		/// Creates a HexBinary with the byte array.
		/// </summary>
		/// <param name="value">The byte array.</param>
		public HexBinary( byte[] value )
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
			HexBinary compareTo = obj as HexBinary;
			if( compareTo == null )
			{
				return base.Equals( obj );
			}

			if( _value.Length != compareTo._value.Length )
			{
				return false;
			}

			for( int idx = 0; idx < _value.Length; idx++ )
			{
				if( _value[ idx ] != compareTo._value[ idx ] )
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
			get{ return new HexBinaryEqual(); }
		}

		/// <summary>
		/// Return the function that verifies if a value is contained within a bag of values of this data type.
		/// </summary>
		/// <value>An IFunction instance.</value>
		public IFunction IsInFunction
		{
			get{ return new HexBinaryIsIn(); }
		}

		/// <summary>
		/// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
		/// </summary>
		/// <value>An IFunction instance.</value>
		public IFunction SubsetFunction
		{
			get{ return new HexBinarySubset(); }
		}

		/// <summary>
		/// The string representation of the data type constant.
		/// </summary>
		/// <value>A string with the Uri for the data type.</value>
		public string DataTypeName
		{ 
			get{ return Consts.Schema1.InternalDataTypes.XsdHexBinary; }
		}

		/// <summary>
		/// Return an instance of an HexBinary form the string specified.
		/// </summary>
		/// <param name="value">The string value to parse.</param>
		/// <param name="parNo">The parameter number being parsed.</param>
		/// <returns>An instance of the type.</returns>
		public object Parse( string value, int parNo )
		{
			if (value == null || value.Length == 0) throw new ArgumentNullException("value");
			byte[] buff = new byte[ value.Length / 2 ];
			for( int idx = 0; idx < buff.Length; idx++ )
			{
				buff[ idx ] = Byte.Parse( value.Substring( (idx*2), 2 ), NumberStyles.HexNumber, CultureInfo.InvariantCulture );
			}
			return new HexBinary( buff );
		}

		#endregion
	}
}
