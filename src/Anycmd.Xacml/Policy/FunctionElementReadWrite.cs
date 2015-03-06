using System;
using System.Xml;

using cor = Anycmd.Xacml;
using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read/write Function element found in the Policy document that is used as an argument in the Apply 
    /// (or Condition) evaluation.
    /// </summary>
    public class FunctionElementReadWrite : XacmlElement, inf.IExpression
    {
        #region Private members

        /// <summary>
        /// The id of the function that will be used as argument.
        /// </summary>
        private string _functionId;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Function class using the XmlReader specified.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Function node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public FunctionElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader.LocalName == Consts.Schema1.FunctionElement.Function &&
                ValidateSchema(reader, schemaVersion))
            {
                _functionId = reader.GetAttribute(Consts.Schema1.FunctionElement.FunctionId);
            }
            else
            {
                throw new Exception(string.Format(cor.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        /// <summary>
        /// Creates a new instance of the Function class using the provided values.
        /// </summary>
        /// <param name="functionId">The function id</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public FunctionElementReadWrite(string functionId, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _functionId = functionId;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The id of the function used as an argument to the condition or apply evaluation.
        /// </summary>
        public virtual string FunctionId
        {
            get { return _functionId; }
            set { _functionId = value; }
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
        public void WriteDocument(XmlWriter writer)
        {
            writer.WriteStartElement(Consts.Schema1.FunctionElement.Function);
            if (!string.IsNullOrEmpty(this._functionId))
            {
                writer.WriteAttributeString(Consts.Schema1.FunctionElement.FunctionId, this._functionId);
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}
