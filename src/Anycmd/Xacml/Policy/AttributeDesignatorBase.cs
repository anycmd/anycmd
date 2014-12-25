using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a generic attribute designator found in the Policy document. The AttributeDesignator is a node
    /// that must be replaced with the value of the designated node in the context document. The algorithm that 
    /// resolves the designation is in the EvaluationEngine class.
    /// </summary>
    /// <remarks>The algorith used to match the attrbute designator with the Context document is defined in the 
    /// section 7.9 of the Xacml specification.</remarks>
    public abstract class AttributeDesignatorBase : AttributeReferenceBase
    {
        #region Private members

        /// <summary>
        /// The Id of the designated attribute.
        /// </summary>
        private string _attributeId = String.Empty;

        /// <summary>
        /// The issuer of the designated attribute.
        /// </summary>
        private string _issuer = String.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the AttributeDesignator using the arguments specified.
        /// </summary>
        /// <param name="dataType">The data type id.</param>
        /// <param name="mustBePresent">Whether the attribute must be present.</param>
        /// <param name="attributeId">The attribute id.</param>
        /// <param name="issuer">The issuer id.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        protected AttributeDesignatorBase(string dataType, bool mustBePresent, string attributeId, string issuer, XacmlVersion version)
            : base(dataType, mustBePresent, version)
        {
            _attributeId = attributeId;
            _issuer = issuer;
        }

        /// <summary>
        /// Creates an instance of the AttributeDesignator class with the XmlReader instance. The base class 
        /// constructor is also called using the XmlReader.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the begining of the AttributeDesignator node.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        protected AttributeDesignatorBase(XmlReader reader, XacmlVersion version) :
            base(reader, version)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            _attributeId = reader.GetAttribute(Consts.Schema1.AttributeDesignatorElement.AttributeId);
            _issuer = reader.GetAttribute(Consts.Schema1.AttributeDesignatorElement.Issuer);
            if (_issuer == null)
            {
                _issuer = String.Empty;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The Id of the designated attribute.
        /// </summary>
        public string AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        /// <summary>
        /// The issuer of the designated attribute.
        /// </summary>
        public string Issuer
        {
            get { return _issuer; }
            set { _issuer = value; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string representation of the attribute designator placing the attribute id and the data type.
        /// </summary>
        /// <returns>The string representation of the instance.</returns>
        public override string ToString()
        {
            return "[" + this._attributeId + "[" + DataType + "]" + "]";
        }

        #endregion
    }
}
