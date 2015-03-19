using Anycmd.Xacml.Interfaces;
using Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// The policy combining algorithm described in the Appendix C.1. This class is a 
    /// translation of the pseudo-code placed in the documentation.
    /// </summary>
    public class RuleCombiningAlgorithmDenyOverrides : IRuleCombiningAlgorithm
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RuleCombiningAlgorithmDenyOverrides()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// The evaluation implementation in the pseudo-code described in the specification.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="rules">The policies that must be evaluated.</param>
        /// <returns>The final decission for the combination of the rule evaluation.</returns>
        public Decision Evaluate(EvaluationContext context, RuleCollection rules)
        {
            var decision = Decision.Indeterminate;

            bool atLeastOneError = false;
            bool potentialDeny = false;
            bool atLeastOnePermit = false;

            context.Trace("Evaluating rules...");
            context.AddIndent();
            try
            {
                foreach (Rule rule in rules)
                {
                    decision = rule.Evaluate(context);

                    context.TraceContextValues();

                    if (decision == Decision.Deny)
                    {
                        decision = Decision.Deny;
                        return decision;
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
                        atLeastOneError = true;
                        if (rule.RuleDefinition.Effect == Effect.Deny)
                        {
                            potentialDeny = true;
                        }
                        continue;
                    }
                }
                if (potentialDeny)
                {
                    decision = Decision.Indeterminate;
                    return decision;
                }
                if (atLeastOnePermit)
                {
                    decision = Decision.Permit;
                    return decision;
                }
                if (atLeastOneError)
                {
                    decision = Decision.Indeterminate;
                    return decision;
                }
                decision = Decision.NotApplicable;
                return decision;
            }
            finally
            {
                context.Trace("Rule combination algorithm: {0}", decision.ToString());
                context.RemoveIndent();
            }
        }

        #endregion
    }
}
