using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents a Subject node found in the context document. This class extends the abstract base class 
    /// TargetItem which loads the "target item" definition.
    /// </summary>
    public class SubjectElement : SubjectElementReadWrite
    {
        #region Constructors

        /// <summary>
        /// Creates a Subject using the specified arguments.
        /// </summary>
        /// <param name="subjectCategory">The subject category.</param>
        /// <param name="attributes">The attribute list.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public SubjectElement(string subjectCategory, AttributeCollection attributes, XacmlVersion schemaVersion)
            : base(subjectCategory, attributes, schemaVersion)
        {
        }

        /// <summary>
        /// Creates an instance of the Subject class using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Subject node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public SubjectElement(XmlReader reader, XacmlVersion schemaVersion) :
            base(reader, schemaVersion)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// The subject category defined in the context document.
        /// </summary>
        public override string SubjectCategory
        {
            set { throw new NotSupportedException(); }
        }
        /// <summary>
        /// 
        /// </summary>
        public override AttributeReadWriteCollection Attributes
        {
            get
            {
                return new AttributeCollection(base.Attributes);
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
        #endregion
    }
}
