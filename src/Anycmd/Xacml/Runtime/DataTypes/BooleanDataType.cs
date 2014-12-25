using Anycmd.Xacml.Runtime.Functions;
using System.Xml;
using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Runtime.DataTypes
{
	/// <summary>
	/// A class defining the Boolean data type.
	/// </summary>
	public class BooleanDataType : inf.IDataType 
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal BooleanDataType()
		{
		}

		#endregion

		#region IDataType members

		/// <summary>
		/// Return the function that compares two values of this data type.
		/// </summary>
		/// <value>An IFunction instance.</value>
		public inf.IFunction EqualFunction
		{
			get{ return new BooleanEqual(); }
		}

		/// <summary>
		/// Return the function that verifies if a value is contained within a bag of values of this data type.
		/// </summary>
		/// <value>An IFunction instance.</value>
		public inf.IFunction IsInFunction
		{
			get{ return new BooleanIsIn(); }
		}

		/// <summary>
		/// Return the function that verifies if all the values in a bag are contained within another bag of values of this data type.
		/// </summary>
		/// <value>An IFunction instance.</value>
		public inf.IFunction SubsetFunction
		{
			get{ return new BooleanSubset(); }
		}

		/// <summary>
		/// The string representation of the data type constant.
		/// </summary>
		/// <value>A string with the Uri for the data type.</value>
		public string DataTypeName
		{ 
			get{ return Consts.Schema1.InternalDataTypes.XsdBoolean; }
		}

		/// <summary>
		/// Return an instance of an Boolean form the string specified.
		/// </summary>
		/// <param name="value">The string value to parse.</param>
		/// <param name="parNo">The parameter number being parsed.</param>
		/// <returns>An instance of the type.</returns>
		public object Parse( string value, int parNo )
		{
			return XmlConvert.ToBoolean( value );
		}

		#endregion
	}

}
