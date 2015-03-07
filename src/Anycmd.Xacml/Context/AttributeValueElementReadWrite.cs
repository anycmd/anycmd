using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    using Policy;

    /// <summary>
    /// Represents an attribute value element found in the context document. This class extends the abstract base 
    /// class StringValue because this class contains string data which may be converted to a typed value.
    /// </summary>
    public class AttributeValueElementReadWrite : StringValueBase
    {
        #region Private members

        /// <summary>
        /// The contents of the attribute value as string.
        /// </summary>
        private string _contents;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the AttributeValue using the XmlReader provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the AttributeValue node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeValueElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.ContextSchema.AttributeElement.AttributeValue &&
                ValidateSchema(reader, schemaVersion))
            {
                // Load the node contents
                _contents = reader.ReadInnerXml();
            }
            else
            {
                throw new Exception(string.Format(Properties.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        /// <summary>
        /// Creates an instance of an attribute value using the given string as its contents.
        /// </summary>
        /// <param name="value">The string value for this attribute element.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeValueElementReadWrite(string value, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            _contents = value;
        }

        /// <summary>
        /// Clones an attribute value element into a new element.
        /// </summary>
        /// <param name="attributeValueElement">The value element to clone.</param>
        public AttributeValueElementReadWrite(AttributeValueElementReadWrite attributeValueElement)
            : base(XacmlSchema.Context, attributeValueElement.SchemaVersion)
        {
            if (attributeValueElement == null) throw new ArgumentNullException("attributeValueElement");
            _contents = attributeValueElement._contents;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The contents as string value.
        /// </summary>
        public virtual string Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        /// <summary>
        /// The contents as string value.
        /// </summary>
        public override string Value
        {
            set { _contents = value; }
            get { return _contents; }
        }

        /// <summary>
        /// The data type of the value, it returns null, since the AttributeValue node does specify the data type.
        /// </summary>
        public override string DataTypeValue
        {
            set { }
            get { return null; }
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
