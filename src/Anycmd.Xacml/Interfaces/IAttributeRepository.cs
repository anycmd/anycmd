using System.Xml;
using ctx = Anycmd.Xacml.Context;
using pol = Anycmd.Xacml.Policy;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Interfaces
{
	/// <summary>
	/// Defines an abstract attribute repository that is called by the EvaluationEngine when an attribute can't be 
	/// found in the context document.
	/// </summary>
	public interface IAttributeRepository
	{
		/// <summary>
		/// Initializes the repository provider using XmlNode that defines the provider in the configuration file.
		/// </summary>
		/// <param name="configNode">The XmlNode that defines the provider in the configuration file.</param>
		void Init( XmlNode configNode );

		/// <summary>
		/// This method is called by the EvalationEngine when it's unable to find an attribute in the context 
		/// document.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="designator">The AttributeDesignator instance that identifies the attribute.</param>
		/// <returns>An instance of the Attribute definition and value, or null if the attribute can't be found 
		/// in the repository.</returns>
		ctx.AttributeElement GetAttribute( rtm.EvaluationContext context, pol.AttributeDesignatorBase designator );
	}
}
