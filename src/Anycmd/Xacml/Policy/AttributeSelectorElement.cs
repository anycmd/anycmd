using System;
using System.Xml;

using cor = Anycmd.Xacml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents an AttributeSelector node found in the Policy document. This element defines an XPath query that
    /// is executed over the Context document and the results are used as values during the evaluation.
    /// </summary>
    public class AttributeSelectorElement : AttributeReferenceBase
    {
        #region Private members

        /// <summary>
        /// Mantains the XPath query used to search for the value.
        /// </summary>
        private string _requestContextPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the AttributeSelector.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the AttributeSelector node.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public AttributeSelectorElement(XmlReader reader, XacmlVersion version)
            : base(reader, version)
        {
            if (reader.LocalName == Consts.Schema1.AttributeSelectorElement.AttributeSelector &&
                ValidateSchema(reader, version))
            {
                _requestContextPath = reader.GetAttribute(Consts.Schema1.AttributeSelectorElement.RequestContextPath);
            }
            else
            {
                throw new Exception(string.Format(cor.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        /// <summary>
        /// Creates an instance of the AttibuteSelector with the provided values
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="mustBePresent"></param>
        /// <param name="requestContextPath"></param>
        /// <param name="schemaVersion"></param>
        public AttributeSelectorElement(string dataType, bool mustBePresent, string requestContextPath, XacmlVersion schemaVersion)
            : base(dataType, mustBePresent, schemaVersion)
        {
            _requestContextPath = requestContextPath;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the XPath query used to search for the Context node values.
        /// </summary>
        public string RequestContextPath
        {
            get { return _requestContextPath; }
            set { _requestContextPath = value; }
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
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public override void WriteDocument(XmlWriter writer)
        {
            writer.WriteStartElement(Consts.Schema1.AttributeSelectorElement.AttributeSelector);
            writer.WriteAttributeString(Consts.Schema1.AttributeSelectorElement.RequestContextPath, this._requestContextPath);
            writer.WriteAttributeString(Consts.Schema1.AttributeSelectorElement.DataType, this.DataType);
            writer.WriteEndElement();
        }

        #endregion
    }
}
