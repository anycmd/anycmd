
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Interfaces
{
	/// <summary>
	/// This interface allows extension of the rule combining algorithms and is also 
	/// responsible for as a factory of the derived classes.
	/// </summary>
	public interface IRuleCombiningAlgorithm
	{
		/// <summary>
		/// Abstract method that should be implemented by the derived classe where the evaluation 
		/// logic is implemented.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="rules">The rules that will be evaulated.</param>
		/// <returns>The final decission if the evaluation.</returns>
		rtm.Decision Evaluate( rtm.EvaluationContext context, rtm.RuleCollection rules );
	}
}
