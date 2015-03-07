using System;
using System.Collections.Generic;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents a Request node in the context document.
    /// </summary>
    public class RequestElementReadWrite : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The list of subjects that were placed in the context document.
        /// </summary>
        private SubjectReadWriteCollection _subjects = new SubjectReadWriteCollection();

        /// <summary>
        /// The resource node defined in the context document.
        /// </summary>
        private ResourceReadWriteCollection _resources = new ResourceReadWriteCollection();

        /// <summary>
        /// The action node defined in the context document.
        /// </summary>
        private ActionElementReadWrite _action;

        /// <summary>
        /// The environment node defined in the context document.
        /// </summary>
        private EnvironmentElementReadWrite _environment;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Request using the parameters specified.
        /// </summary>
        /// <param name="subjects">The subjects list.</param>
        /// <param name="resources">The resource requested</param>
        /// <param name="action">The action requested.</param>
        /// <param name="environment">Any environment attribute part of the request.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public RequestElementReadWrite(SubjectReadWriteCollection subjects, ResourceReadWriteCollection resources, ActionElementReadWrite action, EnvironmentElementReadWrite environment, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            _subjects = subjects;
            _resources = resources;
            _action = action;
            _environment = environment;
        }

        /// <summary>
        /// Creates a new Request using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Request node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public RequestElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.ContextSchema.RequestElement.Request)
            {
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.ContextSchema.RequestElement.Subject:
                            _subjects.Add(new SubjectElementReadWrite(reader, schemaVersion));
                            break;
                        case Consts.ContextSchema.RequestElement.Resource:
                            _resources.Add(new ResourceElementReadWrite(reader, schemaVersion));
                            break;
                        case Consts.ContextSchema.RequestElement.Action:
                            _action = new ActionElementReadWrite(reader, schemaVersion);
                            break;
                        case Consts.ContextSchema.RequestElement.Environment:
                            _environment = new EnvironmentElementReadWrite(reader, schemaVersion);
                            break;
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(Properties.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The list of subjects that were placed in the context document.
        /// </summary>
        public virtual SubjectReadWriteCollection Subjects
        {
            set { _subjects = value; }
            get { return _subjects; }
        }

        /// <summary>
        /// The resources defined in the context document.
        /// </summary>
        public virtual ResourceReadWriteCollection Resources
        {
            set { _resources = value; }
            get { return _resources; }
        }

        /// <summary>
        /// The action node defined in the context document.
        /// </summary>
        public virtual ActionElementReadWrite Action
        {
            set { _action = value; }
            get { return _action; }
        }

        /// <summary>
        /// The environment node defined in the context document.
        /// </summary>
        public virtual EnvironmentElementReadWrite Environment
        {
            set { _environment = value; }
            get { return _environment; }
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
        /// Writes the element in the provided writer
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        /// <param name="namespaces">The xml's namespaces</param>
        public void WriteDocument(XmlWriter writer, IDictionary<string, string> namespaces)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (namespaces == null) throw new ArgumentNullException("namespaces");
            writer.WriteStartDocument();
            writer.WriteStartElement(Consts.ContextSchema.RequestElement.Request, string.Empty);

            foreach (var name in namespaces)
            {
                writer.WriteAttributeString(Consts.Schema1.Namespaces.Xmlns, name.Key.ToString(), null, name.Value.ToString());
            }
            if (this._subjects != null)
            {
                this._subjects.WriteDocument(writer);
            }
            if (this._resources != null)
            {
                this._resources.WriteDocument(writer);
            }
            if (this._action != null)
            {
                this._action.WriteDocument(writer);
            }
            if (this._environment != null)
            {
                this._environment.WriteDocument(writer);
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        #endregion
    }
}
