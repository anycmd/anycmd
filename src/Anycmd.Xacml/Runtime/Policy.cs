using System;
using System.Collections;
using System.Collections.Specialized;
using Anycmd.Xacml.Policy.TargetItems;
using Anycmd.Xacml.Interfaces;
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// References a policy during runtime evaluation.
    /// </summary>
    public class Policy : IMatchEvaluable, IObligationsContainer
    {
        #region Private members

        /// <summary>
        /// All the resources in the policy.
        /// </summary>
        private readonly StringCollection _allResources = new StringCollection();

        /// <summary>
        /// All the rules in this policy.
        /// </summary>
        private readonly RuleCollection _rules = new RuleCollection();

        /// <summary>
        /// The final decission for this policy.
        /// </summary>
        private Decision _evaluationValue;

        /// <summary>
        /// The policy defined in the policy document.
        /// </summary>
        private readonly pol.PolicyElement _policy;

        /// <summary>
        ///	The target during the evaluation process.
        /// </summary>
        private readonly Target _target;

        /// <summary>
        /// The obligations set to this policy.
        /// </summary>
        private pol.ObligationCollection _obligations = new pol.ObligationCollection();

        /// <summary>
        /// The reference for all the evaluated variables.
        /// </summary>
        private IDictionary _variables;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new runtime policy evaluation.
        /// </summary>
        /// <param name="policy">The policy document.</param>
        public Policy(pol.PolicyElement policy)
        {
            if (policy == null) throw new ArgumentNullException("policy");
            _policy = policy;

            // Chechs the target for this policy.
            if (policy.Target != null)
            {
                _target = new Target((pol.TargetElement)policy.Target);

                // Load all the resources for this policy.
                foreach (ResourceElement resource in policy.Target.Resources.ItemsList)
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

            // Load all the Rules and creates a new runtime rule.
            foreach (pol.RuleElement rule in policy.Rules)
            {
                var ruleEv = new Rule(rule);
                _rules.Add(ruleEv);

                foreach (string rName in ruleEv.AllResources)
                {
                    if (!_allResources.Contains(rName))
                    {
                        _allResources.Add(rName);
                    }
                }
            }
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
        /// Match the target of this policy.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>The retult evaluation of the policy target.</returns>
        public TargetEvaluationValue Match(EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            var targetEvaluationValue = TargetEvaluationValue.Indeterminate;
            context.Trace("Evaluating Target...");
            context.AddIndent();
            try
            {
                // Evaluate the policy target
                targetEvaluationValue = TargetEvaluationValue.Match;
                if (_target != null)
                {
                    targetEvaluationValue = _target.Evaluate(context);
                }
                return targetEvaluationValue;
            }
            finally
            {
                context.TraceContextValues();
                context.RemoveIndent();
                context.Trace("Target: {0}", targetEvaluationValue);
            }
        }

        /// <summary>
        /// Evaluates the policy.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>The decission result for this policy.</returns>
        public Decision Evaluate(EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            context.Trace("Evaluating policy: {0}", _policy.Description);
            context.AddIndent();
            context.CurrentPolicy = this;
            try
            {
                // Evaluate the variables
                if (this._policy.SchemaVersion == XacmlVersion.Version20)
                {
                    if (_variables == null)
                    {
                        context.Trace("Evaluating variables...");
                        _variables = new Hashtable();

                        foreach (pol.VariableDefinitionElement variableDef in _policy.VariableDefinitions.Values)
                        {
                            var variable = new VariableDefinition(variableDef);
                            _variables.Add(variableDef.Id, variable);
                        }
                    }
                }

                // Matches the target.
                TargetEvaluationValue targetEvaluationValue = Match(context);

                // If the target matches.
                if (targetEvaluationValue == TargetEvaluationValue.Match)
                {
                    context.Trace("Rule combination algorithm: {0}", _policy.RuleCombiningAlgorithm);

                    // Evaluate all rules and apply rule combination
                    IRuleCombiningAlgorithm rca = EvaluationEngine.CreateRuleCombiningAlgorithm(_policy.RuleCombiningAlgorithm);
                    _evaluationValue = rca.Evaluate(context, _rules);
                }
                else if (targetEvaluationValue == TargetEvaluationValue.NoMatch)
                {
                    _evaluationValue = Decision.NotApplicable;
                }
                else if (targetEvaluationValue == TargetEvaluationValue.Indeterminate)
                {
                    _evaluationValue = Decision.Indeterminate;
                }

                context.Trace("Policy: {0}", _evaluationValue);

                // Copy all the obligations.
                _obligations = new pol.ObligationCollection();
                if (_evaluationValue != Decision.Indeterminate &&
                    _evaluationValue != Decision.NotApplicable &&
                    _policy.Obligations != null && _policy.Obligations.Count != 0)
                {
                    foreach (pol.ObligationElement obl in _policy.Obligations)
                    {
                        if ((obl.FulfillOn == pol.Effect.Deny && _evaluationValue == Decision.Deny) ||
                            (obl.FulfillOn == pol.Effect.Permit && _evaluationValue == Decision.Permit))
                        {
                            context.Trace("Adding obligation: {0} ", obl.ObligationId);
                            _obligations.Add(obl);
                        }
                    }
                }

                return _evaluationValue;
            }
            finally
            {
                context.RemoveIndent();
                context.CurrentPolicy = null;
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// All the obligations satisfied in the inner policies.
        /// </summary>
        public pol.ObligationCollection Obligations
        {
            get { return _obligations; }
        }

        /// <summary>
        /// The evaluated variable definitions.
        /// </summary>
        public IDictionary VariableDefinition
        {
            get { return _variables; }
        }

        #endregion
    }
}
