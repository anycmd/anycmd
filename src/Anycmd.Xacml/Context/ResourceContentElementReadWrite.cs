using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents the ResourceContent node found in the context document.
    /// </summary>
    public class ResourceContentElementReadWrite : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The contents of the resource content as a string.
        /// </summary>
        private readonly string _contents;

        /// <summary>
        /// The XmlDocument for the ResourceContent contents.
        /// </summary>
        private XmlDocument _document;

        /// <summary>
        /// The name table allows a faster reading if the xml document.
        /// </summary>
        private readonly XmlNameTable _nameTable;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new resource content using the specified document.
        /// </summary>
        /// <param name="document">The XmlDocument instantiated with resource content.</param>
        /// <param name="schemaVersion">The version of the schema used to validate this document.</param>
        public ResourceContentElementReadWrite(XmlDocument document, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            _document = document;
        }

        /// <summary>
        /// Creates a new ResourceContent using the provided XmlReader instance.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the ResourceContent node.</param>
        /// <param name="schemaVersion">The version of the schema used to validate this document.</param>
        public ResourceContentElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.ContextSchema.ResourceElement.ResourceContent)
            {
                // Load the node contents
                _contents = reader.ReadInnerXml();
                _nameTable = reader.NameTable;
            }
            else
            {
                throw new Exception(string.Format(Properties.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the XmlDocument for the resource content contents.
        /// </summary>
        public virtual XmlDocument XmlDocument
        {
            get
            {
                if (_document != null) return _document;
                _document = _nameTable != null ? new XmlDocument(_nameTable) : new XmlDocument();
                if (!string.IsNullOrEmpty(_contents))
                {
                    _document.LoadXml(_contents);
                }
                return _document;
            }
            set
            {
                _document = value;
            }
        }
        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }
        #endregion
    }
}
