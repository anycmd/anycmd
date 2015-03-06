using System;
using System.Xml;

using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a generic attribute referencing element found in the Policy document.  The only elements that are
    /// referencing other elements are: AttributeDesignator and AttributeSelector.
    /// </summary>
    public abstract class AttributeReferenceBase : XacmlElement, inf.IExpression
    {
        #region Private members

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the AttributeReference using the data type and the flag specified.
        /// </summary>
        /// <param name="dataType">The data type id.</param>
        /// <param name="mustBePresent">Whether the attribute must be present.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        protected AttributeReferenceBase(string dataType, bool mustBePresent, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            DataType = dataType;
            MustBePresent = mustBePresent;
        }

        /// <summary>
        /// Creates an instance of the AttributeReference using the XmlNode specified.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the attribute reference node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        protected AttributeReferenceBase(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            DataType = reader.GetAttribute(Consts.Schema1.AttributeSelectorElement.DataType);
            string mustBePresent = reader.GetAttribute(Consts.Schema1.AttributeSelectorElement.MustBePresent);
            if (string.IsNullOrEmpty(mustBePresent))
            {
                MustBePresent = false;
            }
            else
            {
                MustBePresent = bool.Parse(mustBePresent);
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the data type of the referenced node
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Whether the referenced value must be present or not.
        /// </summary>
        public bool MustBePresent { get; set; }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public abstract void WriteDocument(XmlWriter writer);

        #endregion
    }
}