using System.Xml;

namespace Anycmd.Xacml.Interfaces
{
    /// <summary>
    /// Defines a function repository used to load functions that are referenced in the policy 
    /// document but those types are not defined in the specification.
    /// </summary>
    public interface IFunctionRepository
    {
        /// <summary>
        /// Initializes the repository provider using XmlNode that defines the provider in the configuration file.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the provider in the configuration file.</param>
        void Init(XmlNode configNode);

        /// <summary>
        /// Returns an instance of the function using the function id specified.
        /// </summary>
        /// <param name="functionId">The function id referenced in the policy document.</param>
        /// <returns>The function instance or null if the function was not found.</returns>
        IFunction GetFunction(string functionId);
    }

}
