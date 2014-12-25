using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents the ResourceContent node found in the context document.
    /// </summary>
    public class ResourceContentElement : ResourceContentElementReadWrite
    {
        #region Constructors

        /// <summary>
        /// Creates a new resource content using the specified document.
        /// </summary>
        /// <param name="document">The XmlDocument instantiated with resource content.</param>
        /// <param name="schemaVersion">The version of the schema used to validate this document.</param>
        public ResourceContentElement(XmlDocument document, XacmlVersion schemaVersion)
            : base(document, schemaVersion)
        {
        }

        /// <summary>
        /// Creates a new ResourceContent using the provided XmlReader instance.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the ResourceContent node.</param>
        /// <param name="schemaVersion">The version of the schema used to validate this document.</param>
        public ResourceContentElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the XmlDocument for the resource content contents.
        /// </summary>
        public override XmlDocument XmlDocument
        {
            set { throw new NotSupportedException(); }
        }
        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return true; }
        }
        #endregion
    }
}
