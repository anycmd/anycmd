
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class PolicyDocument : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.PolicyDocumentReadWrite _policyDocument;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="policyDocument"></param>
		public PolicyDocument( pol.PolicyDocumentReadWrite policyDocument )
		{
			_policyDocument = policyDocument;

			this.Text = "Policy Document";

			if( _policyDocument.Policy != null )
			{
				this.Nodes.Add( new Policy( _policyDocument.Policy ) );
			}
			else if( _policyDocument.PolicySet != null )
			{
				this.Nodes.Add( new PolicySet( _policyDocument.PolicySet ) );
			}
			this.Expand();
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.PolicyDocumentReadWrite PolicyDocumentDefinition
		{
			get{ return _policyDocument; }
		}
	}
}
