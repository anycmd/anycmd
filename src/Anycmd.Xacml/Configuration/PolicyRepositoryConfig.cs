
namespace Anycmd.Xacml.Configuration
{
    using System.Xml;

    /// <summary>
    /// Defines a policy repository found in the configuration file. This extension extends the NameTypeConfig 
    /// base class which loads a standard extension configuration format.
    /// </summary>
    public class PolicyRepositoryConfig : NameTypeConfig
    {
        /// <summary>
        /// Creates a new PolicyRepositoryConfig using the XmlNode specified. Also calls the base class 
        /// constructor with the XmlNode.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the extension.</param>
        public PolicyRepositoryConfig(XmlNode configNode)
            : base(configNode)
        {
        }

    }
}
