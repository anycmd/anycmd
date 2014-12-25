using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    using Interfaces;

    /// <summary>
    /// Represents a VariableReference element found in the Policy document.
    /// </summary>
    public class VariableReferenceElement : XacmlElement, IExpression
    {
        #region Private members

        /// <summary>
        /// The id of the variable.
        /// </summary>
        private string _variableId;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the VariableReference class using the XmlReader specified.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the VariableReference node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public VariableReferenceElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.Schema2.VariableReferenceElement.VariableReference &&
                ValidateSchema(reader, schemaVersion))
            {
                _variableId = reader.GetAttribute(Consts.Schema2.VariableReferenceElement.VariableId);
            }
            else
            {
                throw new Exception(string.Format(Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The id of the variable.
        /// </summary>
        public string VariableId
        {
            get { return _variableId; }
        }
        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return true; }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.WriteStartElement(Consts.Schema1.VariableReferenceElement.VariableReference);
            writer.WriteAttributeString(Consts.Schema1.VariableReferenceElement.VariableId, this._variableId);
            writer.WriteEndElement();
        }

        #endregion
    }
}
