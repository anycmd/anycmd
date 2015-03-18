using System;
using System.Globalization;

namespace Anycmd.Xacml.Runtime.DataTypes
{
	using Functions;
	using Interfaces;

	/// <summary>
	/// A class defining the IPAddress data type.
	/// </summary>
	public class IpAddress : IDataType
	{
		#region Private members

		/// <summary>
		/// The ip address mask.
		/// </summary>
		private string _mask = String.Empty;

		/// <summary>
		/// The full value.
		/// </summary>
		private string _value = String.Empty;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal IpAddress()
		{
		}

		/// <summary>
		/// Creates a new IPAddress using the name as a string.
		/// </summary>
		/// <param name="address">The address as a string.</param>
		public IpAddress( string address )
		{
			_value = address;
			//TODO: add a regular expression to extract the values. 
			//  ipAddress = address [ "/" mask ] [ ":" [ portrange ] ]
			//  portrange = portnumber | "-"portnumber | portnumber"-"[portnumber]
		}

		#endregion

		#region Public methods

		/// <summary>
		/// The HashCode method overloaded because of a compiler warning. The base class is called.
		/// </summary>
		/// <returns>The HashCode calculated at the base class.</returns>
		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		/// <summary>
		/// Equals method overloaded to compare two values of the same data type.
		/// </summary>
		/// <param name="obj">The object to compare with.</param>
		/// <returns>true, if both values are equals, otherwise false.</returns>
		public override bool Equals(object obj)
		{
			IpAddress compareTo = obj as IpAddress;
			if( compareTo == null )
			{
				return base.Equals( obj );
			}

			return String.Compare( _value, compareTo._value, true, CultureInfo.InvariantCulture ) == 0;
			//TODO: check the real equal operation in the last version of the spec.
		}

		#endregion

		#region IDataType Members

		/// <summary>
		/// Return the function that compares two values of this data type.
		/// </summary>
		/// <value>An IFunction instance.</value>
		public IFunction EqualFunction
		{
			get{ return new IpAddressEqual(); }
		}

		/// <summary>
		/// Return the function that verifies if a value is contained within a bag of values of this data type.
		/// </summary>
		/// <value>An IFunction instance.</value>
		public IFunction IsInFunction
		{
			get{ return new IpAddressIsIn(); }
		}

		/// <summary>
		/// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
		/// </summary>
		/// <value>An IFunction instance.</value>
		public IFunction SubsetFunction
		{
			get{ return new IpAddressSubset(); }
		}

		/// <summary>
		/// The string representation of the data type constant.
		/// </summary>
		/// <value>A string with the Uri for the data type.</value>
		public string DataTypeName
		{ 
			get{ return Consts.Schema2.InternalDataTypes.IpAddress; }
		}

		/// <summary>
		/// Return an instance of an IPAddress form the string specified.
		/// </summary>
		/// <param name="value">The string value to parse.</param>
		/// <param name="parNo">The parameter number being parsed.</param>
		/// <returns>An instance of the type.</returns>
		public object Parse( string value, int parNo )
		{
			return new IpAddress( value );
		}

		#endregion
	}
}
