
namespace Anycmd.Xacml.Context
{
    using System;
    using System.Xml;

    /// <summary>
    /// Represents an attribute found in the context document. This class extends the abstract base class 
    /// StringValue which represents a value found in the context document as a string which must be converted to 
    /// a supported data type.
    /// </summary>
    public class AttributeElement : AttributeElementReadWrite
    {
        #region Constructors

        /// <summary>
        /// Creates an Attribute with the values specified.
        /// </summary>
        /// <param name="attributeId">The attribute id.</param>
        /// <param name="dataType">The data type id.</param>
        /// <param name="issuer">The issuer name.</param>
        /// <param name="issueInstant">The issuer instant.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeElement(string attributeId, string dataType, string issuer, string issueInstant, string value, XacmlVersion schemaVersion)
            : base(attributeId, dataType, issuer, issueInstant, value, schemaVersion)
        {
        }

        /// <summary>
        /// Clones an Attribute from another attribute.
        /// </summary>
        /// <param name="attributeElement">The attribute id.</param>
        public AttributeElement(AttributeElement attributeElement)
            : base(attributeElement)
        {
        }
        /// <summary>
        /// Creates an Attribute instance using the XmlReader provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the Attribute node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The attribute id.
        /// </summary>
        public override string AttributeId
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The data type of the attribute.
        /// </summary>
        public override string DataType
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The issuer of the attribute.
        /// </summary>
        public override string Issuer
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The time when the attribute was issued.
        /// </summary>
        public override string IssueInstant
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The attribute values.
        /// </summary>
        public override AttributeValueElementReadWriteCollection AttributeValues
        {
            set { throw new NotSupportedException(); }
            get
            {
                return new AttributeValueElementCollection(base.AttributeValues);
            }
        }

        /// <summary>
        /// The value of the attribute.
        /// </summary>
        public override string Value
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The data type for the attribute value.
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
