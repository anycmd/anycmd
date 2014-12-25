
namespace Anycmd.Xacml.Interfaces
{
	/// <summary>
	/// Defines a function that can be executed by the EvaluationEngine.
	/// </summary>
	public interface ITypeSpecificFunction
	{
		/// <summary>
		/// Defines the data type for which the function was defined for.
		/// </summary>
		IDataType DataType{ get; }
	}
}
