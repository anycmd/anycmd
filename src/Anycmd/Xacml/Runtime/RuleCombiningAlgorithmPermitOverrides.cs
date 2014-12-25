using System;
using inf = Anycmd.Xacml.Interfaces;
using pol = Anycmd.Xacml.Policy;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// The policy combining algorithm described in the Appendix C.2. This class is a 
    /// translation of the pseudo-code placed in the documentation.
    /// </summary>
    public class RuleCombiningAlgorithmPermitOverrides : inf.IRuleCombiningAlgorithm
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RuleCombiningAlgorithmPermitOverrides()
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
        public rtm.Decision Evaluate(rtm.EvaluationContext context, RuleCollection rules)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (rules == null) throw new ArgumentNullException("rules");
            Decision decision = Decision.Indeterminate;
            bool atLeastOneError = false;
            bool potentialPermit = false;
            bool atLeastOneDeny = false;

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
                        atLeastOneDeny = true;
                        continue;
                    }
                    if (decision == Decision.Permit)
                    {
                        decision = Decision.Permit;
                        return decision;
                    }
                    if (decision == Decision.NotApplicable)
                    {
                        continue;
                    }
                    if (decision == Decision.Indeterminate)
                    {
                        atLeastOneError = true;
                        if (rule.RuleDefinition.Effect == pol.Effect.Permit)
                        {
                            potentialPermit = true;
                        }
                        continue;
                    }
                }
                if (potentialPermit)
                {
                    decision = Decision.Indeterminate;
                    return decision;
                }
                if (atLeastOneDeny)
                {
                    decision = Decision.Deny;
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
