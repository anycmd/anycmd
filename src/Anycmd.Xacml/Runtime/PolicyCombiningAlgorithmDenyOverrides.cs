using System;

namespace Anycmd.Xacml.Runtime
{
    using Interfaces;

    /// <summary>
    /// The policy combining algorithm described in the Appendix C.1. This class is a 
    /// translation of the pseudo-code placed in the documentation.
    /// </summary>
    public class PolicyCombiningAlgorithmDenyOverrides : IPolicyCombiningAlgorithm
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PolicyCombiningAlgorithmDenyOverrides()
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
            if (context == null) throw new ArgumentNullException("context");
            if (policies == null) throw new ArgumentNullException("policies");
            bool atLeastOnePermit = false;
            foreach (IMatchEvaluable policy in policies)
            {
                Decision decision = policy.Evaluate(context);
                if (decision == Decision.Deny)
                {
                    context.ProcessingError = false;
                    context.IsMissingAttribute = false;
                    return Decision.Deny;
                }
                if (decision == Decision.Permit)
                {
                    atLeastOnePermit = true;
                    continue;
                }
                if (decision == Decision.NotApplicable)
                {
                    continue;
                }
                if (decision == Decision.Indeterminate)
                {
                    context.ProcessingError = false;
                    context.IsMissingAttribute = false;
                    return Decision.Deny;
                }
            }
            if (atLeastOnePermit)
            {
                context.ProcessingError = false;
                context.IsMissingAttribute = false;
                return Decision.Permit;
            }
            return Decision.NotApplicable;
        }

        #endregion
    }
}

