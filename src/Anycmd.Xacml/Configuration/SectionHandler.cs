
namespace Anycmd.Xacml.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The .Net configuration section handler used to load the configuration of the EvaluationEngine.
    /// </summary>
    public class SectionHandler : IConfigurationSectionHandler
    {
        #region Constructor

        /// <summary>
        /// Default public constructor.
        /// </summary>
        public SectionHandler()
        {
        }

        #endregion

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// Called by the .Net configuration infraestructure to load the configuration from the specified XmlNode.
        /// </summary>
        /// <param name="parent">not used</param>
        /// <param name="configContext">not used</param>
        /// <param name="section">The XmlNode of the configuration section.</param>
        /// <returns>A new instance of the EvaluationEngine configuration.</returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return new ConfigurationRoot(section);
        }

        #endregion
    }
}
