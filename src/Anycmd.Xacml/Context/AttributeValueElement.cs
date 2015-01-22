using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents an attribute value element found in the context document. This class extends the abstract base 
    /// class StringValue because this class contains string data which may be converted to a typed value.
    /// </summary>
    public class AttributeValueElement : AttributeValueElementReadWrite
    {

        #region Constructors

        /// <summary>
        /// Creates an instance of the AttributeValue using the XmlReader provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the AttributeValue node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeValueElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }

        /// <summary>
        /// Creates an instance of an attribute value using the given string as its contents.
        /// </summary>
        /// <param name="value">The string value for this attribute element.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeValueElement(string value, XacmlVersion schemaVersion)
            : base(value, schemaVersion)
        {
        }

        /// <summary>
        /// Clones an attribute value element into a new element.
        /// </summary>
        /// <param name="attributeValueElement">The value element to clone.</param>
        public AttributeValueElement(AttributeValueElement attributeValueElement)
            : base(attributeValueElement)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The contents as string value.
        /// </summary>
        public override string Contents
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The contents as string value.
        /// </summary>
        public override string Value
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The data type of the value, it returns null, since the AttributeValue node does specify the data type.
        /// </summary>
        public override string DataTypeValue
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
