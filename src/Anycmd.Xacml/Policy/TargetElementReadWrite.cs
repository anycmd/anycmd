using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read/write Target node defined in the policy document.
    /// </summary>
    public class TargetElementReadWrite : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The Resources defined in this target.
        /// </summary>
        private ResourcesElementReadWrite _resources;

        /// <summary>
        /// The Subjects defined in this target.
        /// </summary>
        private SubjectsElementReadWrite _subjects;

        /// <summary>
        /// The Actions defined in this target.
        /// </summary>
        private ActionsElementReadWrite _actions;

        /// <summary>
        /// The Environments defined in this target.
        /// </summary>
        private EnvironmentsElementReadWrite _environments;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Target with the specified agumetns.
        /// </summary>
        /// <param name="resources">The resources for this target.</param>
        /// <param name="subjects">The subjects for this target.</param>
        /// <param name="actions">The actions for this target.</param>
        /// <param name="environments">The environments for this target.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public TargetElementReadWrite(ResourcesElementReadWrite resources, SubjectsElementReadWrite subjects, ActionsElementReadWrite actions, EnvironmentsElementReadWrite environments, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _resources = resources;
            _subjects = subjects;
            _actions = actions;
            _environments = environments;
        }

        /// <summary>
        /// Creates a new Target using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Target node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public TargetElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.Schema1.TargetElement.Target &&
                ValidateSchema(reader, schemaVersion))
            {
                if (!reader.IsEmptyElement)
                {
                    while (reader.Read())
                    {
                        switch (reader.LocalName)
                        {
                            case Consts.Schema1.TargetElement.Subjects:
                                _subjects = new SubjectsElementReadWrite(reader, schemaVersion);
                                break;
                            case Consts.Schema1.TargetElement.Resources:
                                _resources = new ResourcesElementReadWrite(reader, schemaVersion);
                                break;
                            case Consts.Schema1.TargetElement.Actions:
                                _actions = new ActionsElementReadWrite(reader, schemaVersion);
                                break;
                            case Consts.Schema2.TargetElement.Environments:
                                _environments = new EnvironmentsElementReadWrite(reader, schemaVersion);
                                break;
                        }
                        if (reader.LocalName == Consts.Schema1.TargetElement.Target &&
                            reader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }
                    }
                }

                // V2 does not have the All(TargetItem) element, and a missing (TargetItem)s element is considered as
                // Any(TargetItem). In order to mantain the code for both versions 
                if (schemaVersion == XacmlVersion.Version20)
                {
                    if (_subjects == null)
                    {
                        _subjects = new SubjectsElementReadWrite(true, new TargetItemReadWriteCollection(), schemaVersion);
                    }
                    if (_resources == null)
                    {
                        _resources = new ResourcesElementReadWrite(true, new TargetItemReadWriteCollection(), schemaVersion);
                    }
                    if (_actions == null)
                    {
                        _actions = new ActionsElementReadWrite(true, new TargetItemReadWriteCollection(), schemaVersion);
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The Resources defined in this target.
        /// </summary>
        public virtual ResourcesElementReadWrite Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        /// <summary>
        /// The Subjects defined in this target.
        /// </summary>
        public virtual SubjectsElementReadWrite Subjects
        {
            get { return _subjects; }
            set { _subjects = value; }
        }

        /// <summary>
        /// The Actions defined in this target.
        /// </summary>
        public virtual ActionsElementReadWrite Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }

        /// <summary>
        /// The Environments defined in this target.
        /// </summary>
        public virtual EnvironmentsElementReadWrite Environments
        {
            get { return _environments; }
            set { _environments = value; }
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
        /// Writes the XML of the Target element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            //The Target element starts
            writer.WriteStartElement(Consts.Schema1.TargetElement.Target);
            switch (this.SchemaVersion)
            {
                case XacmlVersion.Version10:
                case XacmlVersion.Version11:
                    {
                        if (this._subjects != null)
                        {
                            this._subjects.WriteDocument(writer);
                        }
                        if (this._resources != null)
                        {
                            this._resources.WriteDocument(writer);
                        }
                        if (this._actions != null)
                        {
                            this._actions.WriteDocument(writer);
                        }
                        if (this._environments != null)
                        {
                            this._environments.WriteDocument(writer);
                        }
                        break;
                    }
                case XacmlVersion.Version20:
                    {
                        if (this._subjects != null)
                        {
                            if (!this._subjects.IsAny)
                            {
                                this._subjects.WriteDocument(writer);
                            }
                        }
                        if (this._resources != null)
                        {
                            if (!this._resources.IsAny)
                            {
                                this._resources.WriteDocument(writer);
                            }
                        }
                        if (this._actions != null)
                        {
                            if (!this._actions.IsAny)
                            {
                                this._actions.WriteDocument(writer);
                            }
                        }
                        if (this._environments != null)
                        {
                            if (!this._environments.IsAny)
                            {
                                this._environments.WriteDocument(writer);
                            }
                        }
                        break;
                    }
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}