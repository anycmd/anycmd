using System;
using System.Xml;

using cor = Anycmd.Xacml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read/write AttributeAssignment node found in the Policy document. This element is not usefull in the 
    /// specification because they are only copied to the context if the Obligation is satified.
    /// </summary>
    public class AttributeAssignmentElementReadWrite : StringValueBase
    {
        #region Private members

        /// <summary>
        /// The id of the attribute.
        /// </summary>
        private string _attributeId;

        /// <summary>
        /// The data type of the value.
        /// </summary>
        private string _dataType;

        /// <summary>
        /// The contents of the AttributeValue element which represents the value of the constant.
        /// </summary>
        private string _contents;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the ReadWriteAttributeAssignment using the provided XmlReader.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the AttributeAssignament node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeAssignmentElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.Schema1.ObligationElement.AttributeAssignment &&
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
                        else if (reader.LocalName == Consts.Schema1.AttributeAssignmentElement.AttributeId)
                        {
                            _attributeId = reader.GetAttribute(Consts.Schema1.AttributeAssignmentElement.AttributeId);
                        }
                    }
                    reader.MoveToElement();
                }

                // Load the node contents
                _contents = reader.ReadInnerXml();
            }
            else
            {
                throw new Exception(string.Format(Properties.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }
        /// <summary>
        /// Creates an instance of the ReadWriteAttributeAssignment using the provided values
        /// </summary>
        /// <param name="attributeId"></param>
        /// <param name="dataType"></param>
        /// <param name="contents"></param>
        /// <param name="version"></param>
        public AttributeAssignmentElementReadWrite(string attributeId, string dataType, string contents, XacmlVersion version) :
            base(XacmlSchema.Policy, version)
        {
            _attributeId = attributeId;
            _dataType = dataType;
            _contents = contents;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Overrided method because the hash used is the hash of the contents string.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _contents.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of the attribute assignment specifying the data type and the value.
        /// </summary>
        /// <returns>The string representation of the atrtibute assignment.</returns>
        public override string ToString()
        {
            return "[" + this._dataType + "]:" + _contents;
        }

        #endregion

        #region Public properties


        /// <summary>
        /// Gets the value of the assignent.
        /// </summary>
        public override string Value
        {
            set { _contents = value; }
            get { return _contents; }
        }

        /// <summary>
        /// Gets the data type of the attribute assignment.
        /// </summary>
        public override string DataTypeValue
        {
            set { _dataType = value; }
            get { return _dataType; }
        }

        /// <summary>
        /// The id of the attribute.
        /// </summary>
        public virtual string AttributeId
        {
            set { _attributeId = value; }
            get { return _attributeId; }
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
