using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents a Request node in the context document.
    /// </summary>
    public class RequestElement : RequestElementReadWrite
    {
        #region Constructors

        /// <summary>
        /// Creates a new Request using the parameters specified.
        /// </summary>
        /// <param name="subjects">The subjects list.</param>
        /// <param name="resources">The resource requested</param>
        /// <param name="action">The action requested.</param>
        /// <param name="environment">Any environment attribute part of the request.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public RequestElement(SubjectCollection subjects, ResourceCollection resources, ActionElement action, EnvironmentElement environment, XacmlVersion schemaVersion)
            : base(subjects, resources, action, environment, schemaVersion)
        {
        }

        /// <summary>
        /// Creates a new Request using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Request node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public RequestElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The list of subjects that were placed in the context document.
        /// </summary>
        public override SubjectReadWriteCollection Subjects
        {
            get
            {
                return new SubjectCollection(base.Subjects);
            }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The resources defined in the context document.
        /// </summary>
        public override ResourceReadWriteCollection Resources
        {
            get
            {
                return new ResourceCollection(base.Resources);
            }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The action node defined in the context document.
        /// </summary>
        public override ActionElementReadWrite Action
        {
            set { throw new NotSupportedException(); }
            get { return new ActionElement(new AttributeCollection(base.Action.Attributes), base.Action.SchemaVersion); }
        }

        /// <summary>
        /// The environment node defined in the context document.
        /// </summary>
        public override EnvironmentElementReadWrite Environment
        {
            set { throw new NotSupportedException(); }
            get
            {
                return base.Environment != null ? new EnvironmentElement(new AttributeCollection(base.Environment.Attributes), base.Environment.SchemaVersion) : null;
            }
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
