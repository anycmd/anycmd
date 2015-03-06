using System.Xml;

namespace Anycmd.Xacml.Interfaces
{
    /// <summary>
    /// Defines a data type repository used to load data types that are referenced in the policy or context 
    /// documents but those types are not defined in the specification.
    /// </summary>
    public interface IDataTypeRepository
    {
        /// <summary>
        /// Initializes the repository provider using XmlNode that defines the provider in the configuration file.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the provider in the configuration file.</param>
        void Init(XmlNode configNode);

        /// <summary>
        /// Returns an instance of the data type definition using the data type id specified.
        /// </summary>
        /// <param name="typeId">The data type id referenced in the context or policy document.</param>
        /// <returns>The data type descriptor or null if the data type was not found.</returns>
        IDataType GetDataType(string typeId);
    }
}
