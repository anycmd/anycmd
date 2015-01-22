using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents a Subject node found in the context document. This class extends the abstract base class 
    /// TargetItem which loads the "target item" definition.
    /// </summary>
    public class SubjectElementReadWrite : TargetItemBase
    {
        #region Private members

        /// <summary>
        /// The subject category defined in the context document.
        /// </summary>
        private string _subjectCategory;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a Subject using the specified arguments.
        /// </summary>
        /// <param name="subjectCategory">The subject category.</param>
        /// <param name="attributes">The attribute list.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public SubjectElementReadWrite(string subjectCategory, AttributeReadWriteCollection attributes, XacmlVersion schemaVersion)
            : base(attributes, schemaVersion)
        {
            _subjectCategory = subjectCategory;
        }

        /// <summary>
        /// Creates an instance of the Subject class using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Subject node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public SubjectElementReadWrite(XmlReader reader, XacmlVersion schemaVersion) :
            base(reader, Consts.ContextSchema.RequestElement.Subject, schemaVersion)
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// This method is called by the TargetItem class when an attribute is found. 
        /// </summary>
        /// <param name="namespaceName">The namespace for the attribute.</param>
        /// <param name="attributeName">The attribute name found.</param>
        /// <param name="attributeValue">The attribute value found.</param>
        protected override void AttributeFound(string namespaceName, string attributeName, string attributeValue)
        {
            if (attributeName == Consts.ContextSchema.SubjectElement.SubjectCategory)
            {
                _subjectCategory = attributeValue;
            }
        }

        /// <summary>
        /// This method is called by the TargetItem class when an element is found. This class ignores this method.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the element.</param>
        protected override void NodeFound(XmlReader reader)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// The subject category defined in the context document.
        /// </summary>
        public virtual string SubjectCategory
        {
            set { _subjectCategory = value; }
            get { return _subjectCategory; }
        }
        #endregion
    }
}