
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class PolicyIdReference : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.PolicyIdReferenceElementReadWrite _policyIdReference;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="policyIdReference"></param>
		public PolicyIdReference( pol.PolicyIdReferenceElementReadWrite policyIdReference )
		{
			_policyIdReference = policyIdReference;

			this.Text = string.Format( "PolicyIdReference: {0}", policyIdReference.PolicyId );
		}
	}
}
