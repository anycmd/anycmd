using System;
using System.Xml;

namespace Anycmd.Xacml.Policy.TargetItems
{
    /// <summary>
    /// Represents an SubjectAttributeDesignator node found in the Policy document.
    /// </summary>
    public class SubjectAttributeDesignatorElement : AttributeDesignatorBase
    {
        #region Private members

        /// <summary>
        /// The subject category placed in the designator.
        /// </summary>
        private string _subjectCategory;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the SubjectAttributeDesignator using the arguments specified.
        /// </summary>
        /// <param name="dataType">The data type id.</param>
        /// <param name="mustBePresent">Whether the attribute must be present.</param>
        /// <param name="attributeId">The attribute id.</param>
        /// <param name="issuer">The issuer id.</param>
        /// <param name="subjectCategory">The subject category.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public SubjectAttributeDesignatorElement(string dataType, bool mustBePresent, string attributeId, string issuer, string subjectCategory, XacmlVersion version)
            : base(dataType, mustBePresent, attributeId, issuer, version)
        {
            _subjectCategory = subjectCategory;
        }

        /// <summary>
        /// Creates an instance of the SubjectAttributeDesignator using the provided XmlReader. It also calls the
        /// base class constructor specifying the XmlReader.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the SubjectAttributeDesignator node.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public SubjectAttributeDesignatorElement(XmlReader reader, XacmlVersion version)
            :
            base(reader, version)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            _subjectCategory = reader.GetAttribute(Consts.Schema1.SubjectAttributeDesignatorElement.SubjectCategory);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the subject category placed in the designator.
        /// </summary>
        public string SubjectCategory
        {
            get { return _subjectCategory; }
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
        public override void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.WriteStartElement(Consts.Schema1.SubjectAttributeDesignatorElement.SubjectAttributeDesignator);
            writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.AttributeId, this.AttributeId);
            writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.DataType, this.DataType);
            if (this._subjectCategory != null && this._subjectCategory.Length != 0)
            {
                writer.WriteAttributeString(Consts.Schema1.SubjectAttributeDesignatorElement.SubjectCategory, this._subjectCategory);
            }
            if (this.Issuer != null && this.Issuer.Length != 0)
            {
                writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.Issuer, this.Issuer);
            }
            if (this.MustBePresent)
            {
                writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.MustBePresent, "true");
            }
            else
            {
                writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.MustBePresent, "false");
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}
