
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Rule : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.RuleElementReadWrite _rule;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rule"></param>
		public Rule( pol.RuleElementReadWrite rule )
		{
			_rule = rule;

			if( _rule.Target == null )
			{
				this.Nodes.Add( new AnyTarget() );
			}
			else
			{
				this.Nodes.Add( new Target( _rule.Target ) );
			}

			if( _rule.Condition != null )
			{
				this.Nodes.Add( new Condition( _rule.Condition ) );
			}

			this.Text = string.Format( "Rule ({0})", rule.Id );
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.RuleElementReadWrite RuleDefinition
		{
			get{ return _rule; }
		}
	}
}
