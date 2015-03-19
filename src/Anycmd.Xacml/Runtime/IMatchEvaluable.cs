using System.Collections.Specialized;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// Internal interface used to define a generic evaluable element such a Conditions and Apply.
    /// </summary>
    public interface IMatchEvaluable
    {
        /// <summary>
        /// Validates whether the IEvaluable instance's Target Matches with the context document information. Since 
        /// Policies, PolicySets and Rules defines Targets this method abstracts the Match evaluation.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>A TargetEvaluationValue with the result of the Match.</returns>
        TargetEvaluationValue Match(EvaluationContext context);

        /// <summary>
        /// This method evaluates the IEvaluable instance using the policy and the context documents.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>The decission of the evaluation.</returns>
        Decision Evaluate(EvaluationContext context);

        /// <summary>
        /// All the resources defined in the policy document. It's used to evaluate hierarchical requests.
        /// </summary>
        StringCollection AllResources { get; }
    }
}
