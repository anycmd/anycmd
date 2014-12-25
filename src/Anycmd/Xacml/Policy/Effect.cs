
namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// The effect used as the result of the Rule evaluation. The Obligation also uses this enumeration to define
	/// when the Obligation must be placed in the Response.
	/// </summary>
	public enum Effect
	{
		/// <summary>
		/// If the rule is true the effect will be Deny.
		/// </summary>
		Deny,
		
		/// <summary>
		/// If the rule is true the effect will be Permit.
		/// </summary>
		Permit
	}
}
