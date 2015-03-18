using System;

namespace Anycmd.Xacml.Runtime
{
    using Interfaces;

    /// <summary>
    /// The policy combining algorithm described in the Appendix C.4. This class is a 
    /// translation of the pseudo-code placed in the documentation.
    /// </summary>
    public class PolicyCombiningAlgorithmOnlyOneApplicable : IPolicyCombiningAlgorithm
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PolicyCombiningAlgorithmOnlyOneApplicable()
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
            Boolean atLeastOne = false;
            Policy selectedPolicy = null;
            TargetEvaluationValue appResult;
            for (int i = 0; i < policies.Count; i++)
            {
                Policy tempPolicy = (Policy)policies[i];
                appResult = appResult = tempPolicy.Match(context);
                if (appResult == TargetEvaluationValue.Indeterminate)
                {
                    context.ProcessingError = true;
                    context.TraceContextValues();
                    return Decision.Indeterminate;
                }
                if (appResult == TargetEvaluationValue.Match)
                {
                    if (atLeastOne)
                    {
                        context.ProcessingError = true;
                        context.TraceContextValues();
                        return Decision.Indeterminate;
                    }
                    else
                    {
                        atLeastOne = true;
                        selectedPolicy = (Policy)policies[i];
                    }
                }
                if (appResult == TargetEvaluationValue.NoMatch)
                {
                    continue;
                }
            }
            if (atLeastOne)
            {
                Decision retValue = selectedPolicy.Evaluate(context);

                context.TraceContextValues();

                if (retValue == Decision.Deny || retValue == Decision.Permit)
                {
                    context.ProcessingError = false;
                    context.IsMissingAttribute = false;
                }
                return retValue;
            }
            else
            {
                return Decision.NotApplicable;
            }
        }

        #endregion
    }
}
