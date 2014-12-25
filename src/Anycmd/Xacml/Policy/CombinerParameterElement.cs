using System;
using System.Xml;

using cor = Anycmd.Xacml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a CombinerParameter defined in the policy document.
    /// </summary>
    public class CombinerParameterElement : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The parameter name.
        /// </summary>
        private string _parameterName;

        /// <summary>
        /// The attribute value.
        /// </summary>
        private AttributeValueElement _attributeValue;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new CombinerParameter using the provided argument values.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public CombinerParameterElement(string parameterName, AttributeValueElement attributeValue, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _parameterName = parameterName;
            _attributeValue = attributeValue;
        }

        /// <summary>
        /// Creates a new CombinerParameterElement using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the CombinerParameterElement node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public CombinerParameterElement(XmlReader reader, XacmlVersion schemaVersion) :
            this(reader, Consts.Schema2.CombinerParameterElement.CombinerParameter, schemaVersion)
        {
        }

        /// <summary>
        /// Creates a new CombinerParameterElement using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the CombinerParameterElement node.</param>
        /// <param name="nodeName">The name of the node for this combiner parameter item.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        protected CombinerParameterElement(XmlReader reader, string nodeName, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == nodeName &&
                ValidateSchema(reader, schemaVersion))
            {
                // Read the attributes
                if (reader.HasAttributes)
                {
                    // Load all the attributes
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.LocalName == Consts.Schema2.CombinerParameterElement.ParameterName)
                        {
                            _parameterName = reader.GetAttribute(Consts.Schema2.CombinerParameterElement.ParameterName);
                        }
                        else
                        {
                            AttributeFound(reader);
                        }
                    }
                    reader.MoveToElement();
                }

                // Read the rule contents.
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.Schema2.CombinerParameterElement.AttributeValue:
                            _attributeValue = new AttributeValueElement(reader, schemaVersion);
                            break;
                    }
                    if (reader.LocalName == Consts.Schema2.CombinerParameterElement.CombinerParameter &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(cor.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Method called when unknown attributes are found during parsing of this element. Derived classes will use this
        /// method to being notified about attributes.
        /// </summary>
        /// <param name="reader">The reader positioned at the attribute.</param>
        protected virtual void AttributeFound(XmlReader reader)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The parameter name
        /// </summary>
        public string ParameterName
        {
            get { return _parameterName; }
        }

        /// <summary>
        /// The effect of the rule.
        /// </summary>
        public AttributeValueElement AttributeValue
        {
            get { return _attributeValue; }
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
