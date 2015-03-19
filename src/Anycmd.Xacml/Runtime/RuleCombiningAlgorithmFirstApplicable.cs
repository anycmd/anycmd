using Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// The policy combining algorithm described in the Appendix C.3. This class is a 
	/// translation of the pseudo-code placed in the documentation.
	/// </summary>
	public class RuleCombiningAlgorithmFirstApplicable : IRuleCombiningAlgorithm
	{
		#region Constructor

		/// <summary>
		/// Default constructor.
		/// </summary>
		public RuleCombiningAlgorithmFirstApplicable()
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
		public Decision Evaluate( EvaluationContext context, RuleCollection rules )
		{
			var decision = Decision.Indeterminate;
			context.Trace( "Evaluating rules..." );
			context.AddIndent();
			try
			{
				foreach( Rule rule in rules )
				{
					decision = rule.Evaluate( context );
					context.TraceContextValues();

					if( decision == Decision.Deny )
					{
						decision = Decision.Deny;
						return decision;
					}
					if( decision == Decision.Permit )
					{
						decision = Decision.Permit;
						return decision;
					}
					if( decision == Decision.NotApplicable )
					{
						continue;
					}
					if( decision == Decision.Indeterminate )
					{
						decision = Decision.Indeterminate;
						return decision;
					}
				}
				return Decision.NotApplicable;
			}
			finally
			{
				context.Trace( "Rule combination algorithm: {0}", decision.ToString() );
				context.RemoveIndent();
			}
		}

		#endregion
	}
}
