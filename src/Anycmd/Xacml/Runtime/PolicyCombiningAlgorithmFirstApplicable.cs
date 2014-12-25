
namespace Anycmd.Xacml.Runtime
{
    using Interfaces;

    /// <summary>
    /// The policy combining algorithm described in the Appendix C.3. This class is a 
    /// translation of the pseudo-code placed in the documentation.
    /// </summary>
    public class PolicyCombiningAlgorithmFirstApplicable : IPolicyCombiningAlgorithm
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PolicyCombiningAlgorithmFirstApplicable()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// The evaluation implementation in the pseudo-code described in the specification.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="policies">The policies that must be evaluated.</param>
        /// <returns>The final decission for the combination of the policy evaluation.</returns>
        public Decision Evaluate(EvaluationContext context, IMatchEvaluableCollection policies)
        {
            foreach (IMatchEvaluable policy in policies)
            {
                Decision decision = policy.Evaluate(context);

                context.TraceContextValues();

                if (decision == Decision.Deny)
                {
                    context.ProcessingError = false;
                    context.IsMissingAttribute = false;
                    return Decision.Deny;
                }
                if (decision == Decision.Permit)
                {
                    context.ProcessingError = false;
                    context.IsMissingAttribute = false;
                    return Decision.Permit;
                }
                if (decision == Decision.NotApplicable)
                {
                    continue;
                }
                if (decision == Decision.Indeterminate)
                {
                    return Decision.Indeterminate;
                }
            }
            return Decision.NotApplicable;
        }

        #endregion
    }
}
