
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Policy : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.PolicyElementReadWrite _policy;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="policy"></param>
		public Policy( pol.PolicyElementReadWrite policy )
		{
			_policy = policy;

			this.Text = string.Format( "Policy ( {0} )", policy.Id );

			if( policy.Target == null )
			{
				this.Nodes.Add( new AnyTarget() );
			}
			else
			{
				this.Nodes.Add( new Target( policy.Target ) );
			}

			foreach( pol.RuleElementReadWrite rule in policy.Rules )
			{
				this.Nodes.Add( new Rule( rule ) );
			}
			
			this.Nodes.Add( new Obligations( _policy.Obligations ) );
			this.Expand();
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.PolicyElementReadWrite PolicyDefinition
		{
			get{ return _policy; }
		}
	}
}
