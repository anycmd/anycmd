
namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// This enum defines the possible values that can be returned during Target matching.
	/// </summary>
	public enum TargetEvaluationValue
	{ 
		/// <summary>
		/// The founds an indeterminate situation.
		/// </summary>
		Indeterminate,

		/// <summary>
		/// The Target matches the context document.
		/// </summary>
		Match, 
		
		/// <summary>
		/// The Target does not matches the context document.
		/// </summary>
		NoMatch
	}
}
