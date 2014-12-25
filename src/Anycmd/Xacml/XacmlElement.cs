using System;
using System.Xml;

namespace Anycmd.Xacml
{
    /// <summary>
    /// Base class for every element so common methods can be placed in this class.
    /// </summary>
    public abstract class XacmlElement
    {
        #region Private members

        /// <summary>
        /// The version of the schema that was used to validate.
        /// </summary>
        private XacmlVersion _schemaVersion;

        /// <summary>
        /// The schema that defines the element.
        /// </summary>
        private XacmlSchema _schema;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="schema">The schema that defines the element.</param>
        /// <param name="schemaVersion">The version of the schema used to load the element.</param>
        protected XacmlElement(XacmlSchema schema, XacmlVersion schemaVersion)
        {
            _schema = schema;
            _schemaVersion = schemaVersion;
        }
        /// <summary>
        /// Blank constructor.
        /// </summary>
        protected XacmlElement()
        {
        }
        #endregion

        #region Public properties

        /// <summary>
        /// The version of the schema that was used to validate.
        /// </summary>
        public XacmlVersion SchemaVersion
        {
            get { return _schemaVersion; }
        }

        /// <summary>
        /// The schema that defines the element.
        /// </summary>
        public XacmlSchema Schema
        {
            get { return _schema; }
        }

        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public abstract bool IsReadOnly
        {
            get;
        }

        /// <summary>
        /// Return the string for the namespace using the schema and the version of this element.
        /// </summary>
        internal protected string XmlDocumentSchema
        {
            get
            {
                if (_schema == XacmlSchema.Context)
                {
                    if (this.SchemaVersion == XacmlVersion.Version10 || this.SchemaVersion == XacmlVersion.Version11)
                    {
                        return Consts.Schema1.Namespaces.Context;
                    }
                    else if (this.SchemaVersion == XacmlVersion.Version20)
                    {
                        return Consts.Schema2.Namespaces.Context;
                    }
                }
                else if (_schema == XacmlSchema.Policy)
                {
                    if (this.SchemaVersion == XacmlVersion.Version10 || this.SchemaVersion == XacmlVersion.Version11)
                    {
                        return Consts.Schema1.Namespaces.Policy;
                    }
                    else if (this.SchemaVersion == XacmlVersion.Version20)
                    {
                        return Consts.Schema2.Namespaces.Policy;
                    }
                }
                throw new EvaluationException("invalid schema and version information."); //TODO: resources
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Validates the schema using the version parameter.
        /// </summary>
        /// <param name="reader">The reader positioned in an element with namespace.</param>
        /// <param name="version">The version used to validate the document.</param>
        /// <returns><c>true</c>, if the schema corresponds to the namespace defined in the element.</returns>
        protected bool ValidateSchema(XmlReader reader, XacmlVersion version)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (_schema == XacmlSchema.Policy)
            {
                if (version == XacmlVersion.Version11 || version == XacmlVersion.Version10)
                {
                    return (reader.NamespaceURI == Consts.Schema1.Namespaces.Policy);
                }
                else if (version == XacmlVersion.Version20)
                {
                    return (reader.NamespaceURI == Consts.Schema2.Namespaces.Policy);
                }
            }
            else
            {
                if (version == XacmlVersion.Version11 || version == XacmlVersion.Version10)
                {
                    return (reader.NamespaceURI == Consts.Schema1.Namespaces.Context);
                }
                else if (version == XacmlVersion.Version20)
                {
                    return (reader.NamespaceURI == Consts.Schema2.Namespaces.Context);
                }
            }
            throw new EvaluationException("Invalid version provided"); //TODO: resources
        }

        #endregion
    }
}
