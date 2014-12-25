using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Mantains a context document which can be a Request or a response document.
    /// </summary>
    public class ContextDocument : ContextDocumentReadWrite
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public ContextDocument()
        {
        }

        /// <summary>
        /// Creates a new ContextDocument using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the begining of the document.</param>
        /// <param name="schemaVersion">The schema used to validate this context document.</param>
        public ContextDocument(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }

        #endregion

        public static ContextDocumentReadWrite Create(XmlReader reader, XacmlVersion schemaVersion, DocumentAccess access)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            switch (access)
            {
                case DocumentAccess.ReadOnly:
                    return new ContextDocument(reader, schemaVersion);
                case DocumentAccess.ReadWrite:
                    return new ContextDocumentReadWrite(reader, schemaVersion);
            }
            return null;
        }

        #region Public properties
        /// <summary>
        /// The Request in the context document.
        /// </summary>
        public override RequestElementReadWrite Request
        {
            get
            {
                SubjectCollection subjects = null;
                ActionElement action = null;
                EnvironmentElement environment = null;
                ResourceCollection resources = null;
                if (base.Request.Subjects != null)
                {
                    subjects = new SubjectCollection(base.Request.Subjects);
                }
                if (base.Request.Action != null)
                {
                    action = new ActionElement(new AttributeCollection(base.Request.Action.Attributes), base.Request.Action.SchemaVersion);
                }
                if (base.Request.Environment != null)
                {
                    environment = new EnvironmentElement(new AttributeCollection(base.Request.Environment.Attributes), base.Request.Environment.SchemaVersion);
                }
                if (base.Request.Resources != null)
                {
                    resources = new ResourceCollection(base.Request.Resources);
                }
                return new RequestElement(subjects, resources, action, environment, base.Request.SchemaVersion);
            }
            set { throw new NotSupportedException(); }
        }

        #endregion
    }
}
