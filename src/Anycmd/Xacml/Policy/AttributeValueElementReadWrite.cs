using System;
using System.Xml;

using cor = Anycmd.Xacml;
using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Defines a read/write AttributeValue found in the Policy document. This node is used to define constants which are 
    /// defined as string values and are converted to the correct data type during policy evaluation. 
    /// </summary>
    public class AttributeValueElementReadWrite : StringValueBase, inf.IExpression
    {
        #region Private members

        /// <summary>
        /// The data type of the value.
        /// </summary>
        private string _dataType;

        /// <summary>
        /// The contents of the AttributeValue element which represents the value of the constant.
        /// </summary>
        private string _contents;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new AttributeValue using the data type and the contents provided.
        /// </summary>
        /// <param name="dataType">The data type id.</param>
        /// <param name="contents">The contents of the attribute value.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeValueElementReadWrite(string dataType, string contents, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _dataType = dataType;
            _contents = contents;
        }

        /// <summary>
        /// Creates an instance of the AttributeValue class using the XmlReader and the name of the node that 
        /// defines the AttributeValue. 
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the node AttributeValue.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeValueElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.Schema1.AttributeValueElement.AttributeValue &&
                ValidateSchema(reader, schemaVersion))
            {
                if (reader.HasAttributes)
                {
                    // Load all the attributes
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.LocalName == Consts.Schema1.AttributeValueElement.DataType)
                        {
                            _dataType = reader.GetAttribute(Consts.Schema1.AttributeValueElement.DataType);
                        }
                    }
                    reader.MoveToElement();
                }
                // Load the node contents
                _contents = reader.ReadInnerXml();
            }
            else
            {
                throw new Exception(string.Format(cor.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the contents of the AttributeValue as a string.
        /// </summary>
        public virtual string Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        /// <summary>
        /// The data type of the value.
        /// </summary>
        public virtual string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        /// <summary>
        /// Overrides the Value of the abstract class StringValue.
        /// </summary>
        public override string Value
        {
            set { _contents = value; }
            get { return _contents; }
        }

        /// <summary>
        /// Gets the DataType of the AttributeValue as a string.
        /// </summary>
        public override string DataTypeValue
        {
            set { _dataType = value; }
            get { return _dataType; }
        }
        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Overrided method because its string contents will be used as the value to hash.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _contents.GetHashCode();
        }

        /// <summary>
        /// Resturns a string representation of the AttributeValue and the data type for tracing purposes.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[" + this._dataType + "]:" + _contents;
        }

        /// <summary>
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.WriteStartElement(Consts.Schema1.AttributeValueElement.AttributeValue);
            writer.WriteAttributeString(Consts.Schema1.AttributeValueElement.DataType, this._dataType);
            writer.WriteString(this._contents);
            writer.WriteEndElement();
        }


        #endregion
    }
}
