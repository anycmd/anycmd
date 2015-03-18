
namespace Anycmd.Xacml.Runtime
{
    using Interfaces;

    /// <summary>
    /// The policy combining algorithm described in the Appendix C.2. This class is a 
    /// translation of the pseudo-code placed in the documentation.
    /// </summary>
    public class PolicyCombiningAlgorithmPermitOverrides : IPolicyCombiningAlgorithm
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PolicyCombiningAlgorithmPermitOverrides()
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
        public Decision Evaluate(EvaluationContext context, MatchEvaluableCollection policies)
        {
            bool atLeastOneError = false;
            bool atLeastOneDeny = false;
            foreach (IMatchEvaluable policy in policies)
            {
                Decision decision = policy.Evaluate(context);

                context.TraceContextValues();

                if (decision == Decision.Deny)
                {
                    atLeastOneDeny = true;
                    continue;
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
                    atLeastOneError = true;
                    continue;
                }
            }
            if (atLeastOneDeny)
            {
                context.ProcessingError = false;
                context.IsMissingAttribute = false;
                return Decision.Deny;
            }
            if (atLeastOneError)
            {
                return Decision.Indeterminate;
            }
            return Decision.NotApplicable;
        }

        #endregion
    }
}
