using System.Xml;

using pol = Anycmd.Xacml.Policy;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Interfaces
{
	/// <summary>
	/// Defines a generic policy repository used to load policy documents.
	/// </summary>
	public interface IPolicyRepository
	{
		/// <summary>
		/// Initialization method called once during the EvaluationEngine startup.
		/// </summary>
		/// <param name="configNode">The XmlNode that defines the repository in the configuration file.</param>
		void Init( XmlNode configNode );

		/// <summary>
		/// Returns a loaded policy document using the PolicyIdReference instance.
		/// </summary>
		/// <param name="policyReference">The policy id that must be loaded.</param>
		/// <returns>The loaded policy document if it's found, otherwise returns null.</returns>
		pol.PolicyElement GetPolicy( pol.PolicyIdReferenceElement policyReference );

		/// <summary>
		/// Returns a loaded policy set document using the PolicySetIdReference instance.
		/// </summary>
		/// <param name="policySetReference">The policy id that must be loaded.</param>
		/// <returns>The loaded policy document if it's found, otherwise returns null.</returns>
		pol.PolicySetElement GetPolicySet( pol.PolicySetIdReferenceElement policySetReference );

		/// <summary>
		/// If the policy document is not provided to the EvaluationEngine in order to evaluate a context request,
		/// the EvaluationEngine will call this method to find a policydocument which target matches the context 
		/// document information.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <returns></returns>
		pol.PolicyDocument Match( rtm.EvaluationContext context );
	}
}
