
namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// Represents the Decission result for an Evaluate method.
	/// </summary>
	public enum Decision
	{
		/// <summary>
		/// The evaluation is unable to process.
		/// </summary>
		Indeterminate, 
		
		/// <summary>
		/// The evaluation have succeeded.
		/// </summary>
		Permit, 
		
		/// <summary>
		/// The evaluation have not succeeded.
		/// </summary>
		Deny, 
		
		/// <summary>
		/// The evaluation is not applicable to the context document.
		/// </summary>
		NotApplicable
	}
}
