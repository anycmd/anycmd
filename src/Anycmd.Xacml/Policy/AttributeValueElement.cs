using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Defines a read-only AttributeValue found in the Policy document. This node is used to define constants which are 
    /// defined as string values and are converted to the correct data type during policy evaluation. 
    /// </summary>
    public class AttributeValueElement : AttributeValueElementReadWrite
    {
        #region Constructor

        /// <summary>
        /// Creates a new AttributeValue using the data type and the contents provided.
        /// </summary>
        /// <param name="dataType">The data type id.</param>
        /// <param name="contents">The contents of the attribute value.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeValueElement(string dataType, string contents, XacmlVersion schemaVersion)
            : base(dataType, contents, schemaVersion)
        {
        }

        /// <summary>
        /// Creates an instance of the AttributeValue class using the XmlReader and the name of the node that 
        /// defines the AttributeValue. 
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the node AttributeValue.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeValueElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the contents of the AttributeValue as a string.
        /// </summary>
        public override string Contents
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The data type of the value.
        /// </summary>
        public override string DataType
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Overrides the Value of the abstract class StringValue.
        /// </summary>
        public override string Value
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the DataType of the AttributeValue as a string.
        /// </summary>
        public override string DataTypeValue
        {
            set { throw new NotSupportedException(); }
        }

        #endregion
    }
}
