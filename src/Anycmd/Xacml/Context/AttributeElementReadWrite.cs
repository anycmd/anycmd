
namespace Anycmd.Xacml.Context
{
    using Policy;
    using System;
    using System.Xml;

    /// <summary>
    /// Represents an attribute found in the context document. This class extends the abstract base class 
    /// StringValue which represents a value found in the context document as a string which must be converted to 
    /// a supported data type.
    /// </summary>
    public class AttributeElementReadWrite : StringValueBase
    {
        #region Private members

        /// <summary>
        /// The attribute value node representation.
        /// </summary>
        private AttributeValueElementReadWriteCollection _attributeValues = new AttributeValueElementReadWriteCollection();

        /// <summary>
        /// The attribute id
        /// </summary>
        private string _attributeId;

        /// <summary>
        /// The data type for this attribute.
        /// </summary>
        private string _dataType;

        /// <summary>
        /// The issuer of the attribute.
        /// </summary>
        private string _issuer;

        /// <summary>
        /// The time when the attribute was issued.
        /// </summary>
        private string _issueInstant;

        #endregion

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
        public AttributeElementReadWrite(string attributeId, string dataType, string issuer, string issueInstant, string value, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            _attributeId = attributeId;
            _dataType = dataType;
            _issuer = issuer;
            _issueInstant = issueInstant;
            _attributeValues.Add(new AttributeValueElementReadWrite(value, schemaVersion));
        }

        /// <summary>
        /// Clones an Attribute from another attribute.
        /// </summary>
        /// <param name="attributeElement">The attribute id.</param>
        public AttributeElementReadWrite(AttributeElementReadWrite attributeElement)
            : base(XacmlSchema.Context, attributeElement.SchemaVersion)
        {
            if (attributeElement == null) throw new ArgumentNullException("attributeElement");
            _attributeId = attributeElement._attributeId;
            _dataType = attributeElement._dataType;
            _issuer = attributeElement._issuer;
            _issueInstant = attributeElement._issueInstant;
            foreach (AttributeValueElementReadWrite avalue in attributeElement._attributeValues)
            {
                _attributeValues.Add(new AttributeValueElementReadWrite(avalue));
            }
        }

        /// <summary>
        /// Creates an Attribute instance using the XmlReader provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the Attribute node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public AttributeElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.ContextSchema.AttributeElement.Attribute &&
                ValidateSchema(reader, schemaVersion))
            {
                // Read the attributes
                _attributeId = reader.GetAttribute(Consts.ContextSchema.AttributeElement.AttributeId);
                _dataType = reader.GetAttribute(Consts.ContextSchema.AttributeElement.DataType);
                _issuer = reader.GetAttribute(Consts.ContextSchema.AttributeElement.Issuer);
                if (schemaVersion == XacmlVersion.Version10 || schemaVersion == XacmlVersion.Version11)
                {
                    _issueInstant = reader.GetAttribute(Consts.ContextSchema.AttributeElement.IssueInstant);
                }

                // Read the contents
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.ContextSchema.AttributeElement.AttributeValue:
                            _attributeValues.Add(new AttributeValueElementReadWrite(reader, schemaVersion));
                            break;
                    }
                    if (reader.LocalName == Consts.ContextSchema.AttributeElement.Attribute &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The attribute id.
        /// </summary>
        public virtual string AttributeId
        {
            set { _attributeId = value; }
            get { return _attributeId; }
        }

        /// <summary>
        /// The data type of the attribute.
        /// </summary>
        public virtual string DataType
        {
            set { _dataType = value; }
            get { return _dataType; }
        }

        /// <summary>
        /// The issuer of the attribute.
        /// </summary>
        public virtual string Issuer
        {
            set { _issuer = value; }
            get { return _issuer; }
        }

        /// <summary>
        /// The time when the attribute was issued.
        /// </summary>
        public virtual string IssueInstant
        {
            set { _issueInstant = value; }
            get { return _issueInstant; }
        }

        /// <summary>
        /// The attribute values.
        /// </summary>
        public virtual AttributeValueElementReadWriteCollection AttributeValues
        {
            set { _attributeValues = value; }
            get { return _attributeValues; }
        }

        /// <summary>
        /// The value of the attribute.
        /// </summary>
        public override string Value
        {
            get
            {
                //TODO: in V2 there are more than one value for the attribute element, so I think it must be changed
                if (SchemaVersion == XacmlVersion.Version10 || SchemaVersion == XacmlVersion.Version11)
                {
                    return _attributeValues[0].Contents;
                }
                else
                {
                    if (_attributeValues.Count > 1)
                    {
                        throw new EvaluationException("there is more than one value."); //TODO: resources
                    }
                    return _attributeValues[0].Contents;
                }
            }
            set { _attributeValues[0].Contents = value; }
        }

        /// <summary>
        /// The data type for the attribute value.
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
        /// Matches the designator within the values of this attribute element.
        /// </summary>
        /// <param name="attributeDesignator">The attribute designator instance.</param>
        /// <returns><c>true</c> if the designator matches with this attribute.</returns>
        public bool Match(AttributeDesignatorBase attributeDesignator)
        {
            if (attributeDesignator == null) throw new ArgumentNullException("attributeDesignator");
            if (AttributeId != attributeDesignator.AttributeId || DataType != attributeDesignator.DataType)
                return false;
            if (string.IsNullOrEmpty(attributeDesignator.Issuer)) return true;
            return Issuer == attributeDesignator.Issuer;
        }

        /// <summary>
        /// Returns a string representation of this attribute element.
        /// </summary>
        /// <returns>The string representation of this attribute element.</returns>
        public override string ToString()
        {
            //TODO: in V2 there are more than one value for the attribute element, so I think it must be changed
            if (SchemaVersion == XacmlVersion.Version10 || SchemaVersion == XacmlVersion.Version11)
            {
                return "[" + this._attributeId + "[" + _dataType + "]:" + _attributeValues[0].Contents + "]";
            }
            else
            {
                //TODO: there must be multiple values
                return "[" + this._attributeId + "[" + _dataType + "]:" + _attributeValues[0].Contents + "]";
            }
        }

        #endregion
    }
}
