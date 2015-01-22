

namespace Anycmd.Xacml.Runtime.Functions
{
	/// <summary>
	/// Function implementation, in order to check the function behavior use the value of the Id
	/// property to search the function in the specification document.
	/// </summary>
	public class HexBinaryIntersection : BaseIntersection
	{
		#region IFunction Members

		/// <summary>
		/// The id of the function, used only for notification.
		/// </summary>
		public override string Id
		{
			get{ return Consts.Schema1.InternalFunctions.HexBinaryIntersection; }
		}

		/// <summary>
		/// Defines the data type for which the function was defined for.
		/// </summary>
		public override Anycmd.Xacml.Interfaces.IDataType DataType
		{
			get{ return DataTypeDescriptor.HexBinary; }
		}

		#endregion
	}
}
