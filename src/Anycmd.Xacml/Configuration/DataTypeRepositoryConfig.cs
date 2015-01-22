
namespace Anycmd.Xacml.Configuration
{
    using System.Xml;

    /// <summary>
    /// Represents a data type repository defined in the configuration file.
    /// </summary>
    public class DataTypeRepositoryConfig : NameTypeConfig
    {
        /// <summary>
        /// Creates a new instance of the DataTypeRepository using the XmlNode specified.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the extension.</param>
        public DataTypeRepositoryConfig(XmlNode configNode)
            : base(configNode)
        {
        }
    }
}
