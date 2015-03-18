
namespace Anycmd.Xacml.Runtime.Functions
{
	using Interfaces;

	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseMathOperation : FunctionBase, ITypeSpecificFunction
	{
		#region IFunction Members

		/// <summary>
		/// The data type of the return value.
		/// </summary>
		public override IDataType Returns
		{
			get{ return DataType; }
		}

		/// <summary>
		/// Defines the data types for the function arguments.
		/// </summary>
		public override IDataType[] Arguments
		{
			get
			{
				return new[]{ DataType, DataType };
			}
		}

		/// <summary>
		/// Defines the data type for which the function was defined for.
		/// </summary>
		public abstract IDataType DataType { get; }

		#endregion
	}
}
