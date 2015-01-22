using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents an Envirnonment node found in the context document. This class extends the abstract base class 
    /// TargetItem which loads the "target item" definition.
    /// </summary>
    public class EnvironmentElementReadWrite : TargetItemBase
    {
        #region Constructors

        /// <summary>
        /// Creates an Environment using the specified arguments.
        /// </summary>
        /// <param name="attributes">The attribute list.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public EnvironmentElementReadWrite(AttributeReadWriteCollection attributes, XacmlVersion schemaVersion)
            : base(attributes, schemaVersion)
        {
        }

        /// <summary>
        /// Creates an instance of the Environment class using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Environment node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public EnvironmentElementReadWrite(XmlReader reader, XacmlVersion schemaVersion) :
            base(reader, Consts.ContextSchema.RequestElement.Environment, schemaVersion)
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// This method is called by the TargetItem class when an attribute is found. This class ignores this method.
        /// </summary>
        /// <param name="namespaceName">The namespace for the attribute.</param>
        /// <param name="attributeName">The attribute name found.</param>
        /// <param name="attributeValue">The attribute value found.</param>
        protected override void AttributeFound(string namespaceName, string attributeName, string attributeValue)
        {
        }

        /// <summary>
        /// This method is called by the TargetItem class when an element is found. This class ignores this method.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the element.</param>
        protected override void NodeFound(XmlReader reader)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Writes the element in the provided writer.
        /// </summary>
        /// <param name="writer">The instance to write the Xml to.</param>
        public void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.WriteStartElement(Consts.ContextSchema.RequestElement.Environment);
            foreach (AttributeElementReadWrite attr in this.Attributes)
            {
                writer.WriteStartElement(Consts.ContextSchema.AttributeElement.Attribute);
                writer.WriteAttributeString(Consts.ContextSchema.AttributeElement.AttributeId, attr.AttributeId);
                writer.WriteAttributeString(Consts.ContextSchema.AttributeElement.DataType, attr.DataType);
                if (!string.IsNullOrEmpty(attr.Issuer))
                {
                    writer.WriteAttributeString(Consts.ContextSchema.AttributeElement.Issuer, attr.Issuer);
                }
                foreach (AttributeValueElementReadWrite attVal in attr.AttributeValues)
                {
                    writer.WriteElementString(Consts.ContextSchema.AttributeElement.AttributeValue, attVal.Value);
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}