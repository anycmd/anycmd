
namespace Anycmd.Xacml.Runtime
{
    using Interfaces;
    using System;
    using System.Collections.Specialized;
    using Xacml.Policy;
    using Xacml.Policy.TargetItems;

    /// <summary>
    /// The runtime class for the policy set.
    /// </summary>
    public class PolicySet : IMatchEvaluable, IObligationsContainer
    {
        #region Private members

        /// <summary>
        /// All the resources in the policy.
        /// </summary>
        private readonly StringCollection _allResources = new StringCollection();

        /// <summary>
        /// All the policies that belongs to this policy set.
        /// </summary>
        private readonly MatchEvaluableCollection _policies = new MatchEvaluableCollection();

        /// <summary>
        /// The final decission for this policy set.
        /// </summary>
        private Decision _evaluationValue;

        /// <summary>
        /// The policy set defined in the context document.
        /// </summary>
        private readonly PolicySetElement _policySet;

        /// <summary>
        /// The target during the evaluation process.
        /// </summary>
        private readonly Target _target;

        /// <summary>
        /// The obligations set to this policy.
        /// </summary>
        private ObligationCollection _obligations;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new runtime policy set evaluation.
        /// </summary>
        /// <param name="engine">The evaluation engine.</param>
        /// <param name="policySet">The policy set defined in the policy document.</param>
        public PolicySet(EvaluationEngine engine, PolicySetElement policySet)
        {
            if (engine == null) throw new ArgumentNullException("engine");
            if (policySet == null) throw new ArgumentNullException("policySet");
            _policySet = policySet;

            // Create a runtime target of this policy set.
            if (policySet.Target != null)
            {
                _target = new Target((TargetElement)policySet.Target);

                foreach (ResourceElement resource in policySet.Target.Resources.ItemsList)
                {
                    foreach (ResourceMatchElement rmatch in resource.Match)
                    {
                        if (!_allResources.Contains(rmatch.AttributeValue.Contents))
                        {
                            _allResources.Add(rmatch.AttributeValue.Contents);
                        }
                    }
                }
            }

            // Add all the policies (or policy set) inside this policy set.
            foreach (object child in policySet.Policies)
            {
                var childPolicySet = child as PolicySetElement;
                var childPolicyElement = child as PolicyElement;
                var childPolicySetIdReference = child as PolicySetIdReferenceElement;
                var childPolicyIdReferenceElement = child as PolicyIdReferenceElement;
                if (childPolicySet != null)
                {
                    var policySetEv = new PolicySet(engine, childPolicySet);
                    foreach (string rName in policySetEv.AllResources)
                    {
                        if (!_allResources.Contains(rName))
                        {
                            _allResources.Add(rName);
                        }
                    }
                    _policies.Add(policySetEv);
                }
                else if (childPolicyElement != null)
                {
                    var policyEv = new Policy(childPolicyElement);
                    foreach (string rName in policyEv.AllResources)
                    {
                        if (!_allResources.Contains(rName))
                        {
                            _allResources.Add(rName);
                        }
                    }
                    _policies.Add(policyEv);
                }
                else if (childPolicySetIdReference != null)
                {
                    PolicySetElement policySetDefinition = EvaluationEngine.Resolve(childPolicySetIdReference);
                    if (policySetDefinition != null)
                    {
                        var policySetEv = new PolicySet(engine, policySetDefinition);
                        foreach (string rName in policySetEv.AllResources)
                        {
                            if (!_allResources.Contains(rName))
                            {
                                _allResources.Add(rName);
                            }
                        }
                        _policies.Add(policySetEv);
                    }
                    else
                    {
                        throw new EvaluationException(string.Format(Properties.Resource.exc_policyset_reference_not_resolved, ((PolicySetIdReferenceElement)child).PolicySetId));
                    }
                }
                else if (childPolicyIdReferenceElement != null)
                {
                    PolicyElement policyDefinition = EvaluationEngine.Resolve(childPolicyIdReferenceElement);
                    if (policyDefinition != null)
                    {
                        var policyEv = new Policy(policyDefinition);
                        foreach (string rName in policyEv.AllResources)
                        {
                            if (!_allResources.Contains(rName))
                            {
                                _allResources.Add(rName);
                            }
                        }
                        _policies.Add(policyEv);
                    }
                    else
                    {
                        throw new EvaluationException(string.Format(Properties.Resource.exc_policy_reference_not_resolved, ((PolicyIdReferenceElement)child).PolicyId));
                    }
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The obligations that have been fulfilled for this policy set.
        /// </summary>
        public ObligationCollection Obligations
        {
            get { return _obligations; }
        }

        #endregion

        #region IMatchEvaluable members

        /// <summary>
        /// All the resources for this policy.
        /// </summary>
        public StringCollection AllResources
        {
            get { return _allResources; }
        }

        /// <summary>
        /// Match the target of this policy set.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>The retult evaluation of the policy set target.</returns>
        public TargetEvaluationValue Match(EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            // Evaluate the policy target
            var targetEvaluationValue = TargetEvaluationValue.Match;
            if (_target != null)
            {
                targetEvaluationValue = _target.Evaluate(context);
                context.TraceContextValues();
            }
            return targetEvaluationValue;
        }

        /// <summary>
        /// Evaluates the policy set.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>The decission result for this policy set.</returns>
        public Decision Evaluate(EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            context.Trace("Evaluating policySet: {0}", _policySet.Description);
            context.CurrentPolicySet = this;
            try
            {
                context.Trace("Evaluating Target...");
                context.AddIndent();

                // Evaluate the policy target
                TargetEvaluationValue targetEvaluationValue = Match(context);

                context.RemoveIndent();
                context.Trace("Target: {0}", targetEvaluationValue);

                ProcessTargetEvaluationValue(context, targetEvaluationValue);

                context.Trace("PolicySet: {0}", _evaluationValue);

                // If the policy evaluated to Deny or Permit add the obligations depending on its fulfill value.
                ProcessObligations(context);

                return _evaluationValue;
            }
            finally
            {
                context.CurrentPolicySet = null;
            }
        }

        /// <summary>
        /// Process the obligations for the policy.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        private void ProcessObligations(EvaluationContext context)
        {
            _obligations = new ObligationCollection();
            if (_evaluationValue != Decision.Indeterminate &&
                _evaluationValue != Decision.NotApplicable &&
                _policySet.Obligations != null &&
                _policySet.Obligations.Count != 0)
            {
                foreach (ObligationElement obl in _policySet.Obligations)
                {
                    if ((obl.FulfillOn == Effect.Deny && _evaluationValue == Decision.Deny) ||
                        (obl.FulfillOn == Effect.Permit && _evaluationValue == Decision.Permit))
                    {
                        context.Trace("Adding obligation: {0} ", obl.ObligationId);
                        _obligations.Add(obl);
                    }
                }

                // Get all obligations from child policies
                foreach (IMatchEvaluable child in _policies)
                {
                    var oblig = child as IObligationsContainer;
                    if (oblig != null && oblig.Obligations != null)
                    {
                        foreach (ObligationElement childObligation in oblig.Obligations)
                        {
                            if ((childObligation.FulfillOn == Effect.Deny && _evaluationValue == Decision.Deny) ||
                                (childObligation.FulfillOn == Effect.Permit && _evaluationValue == Decision.Permit))
                            {
                                _obligations.Add(childObligation);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process the match result.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="targetEvaluationValue">The match evaluation result.</param>
        private void ProcessTargetEvaluationValue(EvaluationContext context, TargetEvaluationValue targetEvaluationValue)
        {
            if (targetEvaluationValue == TargetEvaluationValue.Match)
            {
                try
                {
                    context.Trace("Evaluating policies...");
                    context.AddIndent();

                    context.Trace("Policy combination algorithm: {0}", _policySet.PolicyCombiningAlgorithm);

                    // Evaluate all policies and apply rule combination
                    IPolicyCombiningAlgorithm pca = EvaluationEngine.CreatePolicyCombiningAlgorithm(_policySet.PolicyCombiningAlgorithm);

                    if (pca == null)
                    {
                        throw new EvaluationException("the policy combining algorithm does not exists."); //TODO: resources
                    }

                    _evaluationValue = pca.Evaluate(context, _policies);

                    // Update the flags for general evaluation status.
                    context.TraceContextValues();

                    context.Trace("Policy combination algorithm: {0}", _evaluationValue.ToString());
                }
                finally
                {
                    context.RemoveIndent();
                }
            }
            else if (targetEvaluationValue == TargetEvaluationValue.NoMatch)
            {
                _evaluationValue = Decision.NotApplicable;
            }
            else if (targetEvaluationValue == TargetEvaluationValue.Indeterminate)
            {
                _evaluationValue = Decision.Indeterminate;
            }
        }

        #endregion
    }
}
