
namespace Anycmd.Xacml.Configuration
{
    using System.Xml;

    /// <summary>
    /// Represents a function repository defined in the configuration file.
    /// </summary>
    public class FunctionRepositoryConfig : NameTypeConfig
    {
        /// <summary>
        /// Creates a new instance of the FunctionRepository using the XmlNode specified.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the extension.</param>
        public FunctionRepositoryConfig(XmlNode configNode)
            : base(configNode)
        {
        }
    }
}
