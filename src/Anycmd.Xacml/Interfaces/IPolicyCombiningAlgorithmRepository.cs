using System.Xml;


namespace Anycmd.Xacml.Interfaces
{
	/// <summary>
	/// Defines an abstract policy combining algorithm repository that is called by the EvaluationEngine when an 
	/// policy combining algorithm can't be found in the context document.
	/// </summary>
	public interface IPolicyCombiningAlgorithmRepository
	{
		/// <summary>
		/// Initializes the repository provider using XmlNode that defines the provider in the configuration file.
		/// </summary>
		/// <param name="configNode">The XmlNode that defines the provider in the configuration file.</param>
		void Init( XmlNode configNode );

		/// <summary>
		/// This method is called by the EvalationEngine when the policy combining algorith used is not defined in 
		/// the specification.
		/// </summary>
		/// <param name="policyCombiningAlgorithmId">The id of the policy combining algorithm that can't be found.</param>
		/// <returns>An instance of a policy combining algorithm.</returns>
		IPolicyCombiningAlgorithm GetPolicyCombiningAlgorithm( string policyCombiningAlgorithmId );
	}
}
