
namespace Anycmd.Xacml.Configuration
{
    using System.Xml;

    /// <summary>
    /// Represents a rule combining algorithm repository defined in the configuration file.
    /// </summary>
    public class RuleCombiningAlgorithmRepository : NameTypeConfig
    {
        /// <summary>
        /// Creates a new instance of the RuleCombiningAlgorithmRepository using the XmlNode specified.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the extension.</param>
        public RuleCombiningAlgorithmRepository(XmlNode configNode)
            : base(configNode)
        {
        }
    }
}
