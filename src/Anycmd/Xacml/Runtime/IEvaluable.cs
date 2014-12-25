

namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// Internal interface used to define a generic evaluable element such a PolicySets, Policies and Rules.
	/// </summary>
	public interface IEvaluable
	{
		/// <summary>
		/// Evaluates and returns an EvaluationValue.
		/// </summary>
		/// <param name="context">The evaluation context.</param>
		/// <returns>The result of the evaluation.</returns>
		EvaluationValue Evaluate( EvaluationContext context );
	}
}
