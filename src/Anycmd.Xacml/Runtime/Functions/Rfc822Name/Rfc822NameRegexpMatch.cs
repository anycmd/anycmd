


namespace Anycmd.Xacml.Runtime.Functions
{
	/// <summary>
	/// Function implementation, in order to check the function behavior use the value of the Id
	/// property to search the function in the specification document.
	/// </summary>
	public class Rfc822NameRegexpMatch : RegexpMatchBase
	{
		#region IFunction Members

		/// <summary>
		/// The id of the function, used only for notification.
		/// </summary>
		public override string Id
		{
			get{ return Consts.Schema2.InternalFunctions.Rfc822NameRegexpMatch; }
		}

		/// <summary>
		/// The data type for which the function is defined.
		/// </summary>
		public override Anycmd.Xacml.Interfaces.IDataType DataType
		{
			get{ return DataTypeDescriptor.Rfc822Name; }
		}

		#endregion
	}
}
