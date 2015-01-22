
namespace Anycmd.Xacml.Configuration
{
    using System;
    using System.Diagnostics;
    using System.Xml;

    /// <summary>
    /// Abstract base class used to load any extension which format is name="" type="".
    /// </summary>
    public abstract class NameTypeConfig
    {
        #region Private members

        /// <summary>
        /// The name of the extension.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// The type name of the extension.
        /// </summary>
        private readonly string _typeName;

        /// <summary>
        /// The instantiated .Net type of the extension.
        /// </summary>
        private readonly Type _type;

        /// <summary>
        /// The XmlNode for the configuration of the extension.
        /// </summary>
        private readonly XmlNode _node;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance in the NamedTypeConfig using the XmlNode provided.
        /// </summary>
        /// <param name="configNode">The XmlNode for the extension configuration.</param>
        protected NameTypeConfig(XmlNode configNode)
        {
            if (configNode == null) throw new ArgumentNullException("configNode");
            _node = configNode;
            Debug.Assert(configNode.Attributes != null, "configNode.Attributes != null");
            _name = configNode.Attributes["name"].Value;
            _typeName = configNode.Attributes["type"].Value;
            _type = Type.GetType(_typeName);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The name of the extension
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// The type name of the extension.
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
        }

        /// <summary>
        /// The instantiated type for the extension.
        /// </summary>
        public Type Type
        {
            get { return _type; }
        }

        /// <summary>
        /// The XmlNode with the extension configuration.
        /// </summary>
        public XmlNode XmlNode
        {
            get { return _node; }
        }

        #endregion
    }
}
