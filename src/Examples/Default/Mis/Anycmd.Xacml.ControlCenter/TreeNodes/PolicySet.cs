
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class PolicySet : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.PolicySetElementReadWrite _policySet;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="policySet"></param>
		public PolicySet( pol.PolicySetElementReadWrite policySet )
		{
			_policySet = policySet;

			this.Text = string.Format( "PolicySet ( {0} )", _policySet.Id );

			if( _policySet.Target == null )
			{
				this.Nodes.Add( new AnyTarget() );
			}
			else
			{
				this.Nodes.Add( new Target( _policySet.Target ) );
			}

			foreach( object policy in _policySet.Policies )
			{
				if( policy is pol.PolicyElementReadWrite )
				{
					this.Nodes.Add( new Policy( (pol.PolicyElementReadWrite)policy ) );
				}
				else if( policy is pol.PolicySetElementReadWrite )
				{
					this.Nodes.Add( new PolicySet( (pol.PolicySetElementReadWrite)policy ) );
				}
				else if( policy is pol.PolicyIdReferenceElementReadWrite )
				{
					this.Nodes.Add( new PolicyIdReference( (pol.PolicyIdReferenceElementReadWrite)policy ) );
				}
				else if( policy is pol.PolicySetIdReferenceElementReadWrite )
				{
					this.Nodes.Add( new PolicySetIdReference( (pol.PolicySetIdReferenceElementReadWrite)policy ) );
				}
			}
		
			this.Nodes.Add( new Obligations( _policySet.Obligations ) );
			this.Expand();
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.PolicySetElementReadWrite PolicySetDefinition
		{
			get{ return _policySet; }
		}
	}
}
