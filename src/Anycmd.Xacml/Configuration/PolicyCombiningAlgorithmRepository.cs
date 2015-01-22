
namespace Anycmd.Xacml.Configuration
{
    using System.Xml;

    /// <summary>
    /// Represents a policy combining algorithm repository defined in the configuration file.
    /// </summary>
    public class PolicyCombiningAlgorithmRepository : NameTypeConfig
    {
        /// <summary>
        /// Creates a new instance of the PolicyCombiningAlgorithmRepository using the XmlNode specified.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the extension.</param>
        public PolicyCombiningAlgorithmRepository(XmlNode configNode)
            : base(configNode)
        {
        }
    }
}
