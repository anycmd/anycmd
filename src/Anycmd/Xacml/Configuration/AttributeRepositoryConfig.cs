
namespace Anycmd.Xacml.Configuration
{
    using System.Xml;

    /// <summary>
    /// Defines an attribute repository found in the configuration file. This extension extends the NameTypeConfig 
    /// base class which loads a standard extension configuration format.
    /// </summary>
    public class AttributeRepositoryConfig : NameTypeConfig
    {
        /// <summary>
        /// Creates a new AttributeRepositoryConfig using the XmlNode specified. Also calls the base class 
        /// constructor with the XmlNode.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the extension.</param>
        public AttributeRepositoryConfig(XmlNode configNode)
            : base(configNode)
        {
        }
    }
}
