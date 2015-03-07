using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// This abstract base class is used to read the Action, Resource, Subject and Environment nodes, since all of 
    /// them have a similar xml structure. The differences in the node names are specified in the constructor.
    /// </summary>
    public abstract class TargetItemBase : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The attributes defined in the "target item".
        /// </summary>
        private AttributeReadWriteCollection _attributes = new AttributeReadWriteCollection();

        #endregion

        #region Constructors

        /// <summary>
        /// Default private constructor.
        /// </summary>
        private TargetItemBase(XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
        }

        /// <summary>
        /// Creates a new target item using the specified attribute list.
        /// </summary>
        /// <param name="attributes">The attribute list.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        protected TargetItemBase(AttributeReadWriteCollection attributes, XacmlVersion schemaVersion)
            : this(schemaVersion)
        {
            _attributes = attributes;
        }

        /// <summary>
        /// Creates a new "target item" using the XmlReader instance provided and the node name for the derived 
        /// "target item".
        /// </summary>
        /// <param name="reader">The XmlReader positioned in the "target item" node.</param>
        /// <param name="itemNodeName">The node if the "target item".</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        protected TargetItemBase(XmlReader reader, string itemNodeName, XacmlVersion schemaVersion)
            : this(schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == itemNodeName)
            {
                // Find all the attributes in the node and notify the derived class about all of them.
                if (reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        // TODO:构造函数中调用了虚方法
                        AttributeFound(reader.NamespaceURI, reader.LocalName, reader.Value);
                    }
                    reader.MoveToElement();
                }

                // Read all the inner attributes and notify the derived class about any node found 
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.ContextSchema.AttributeElement.Attribute:
                            _attributes.Add(new AttributeElementReadWrite(reader, schemaVersion));
                            break;
                        default:
                            // TODO:构造函数中调用了虚方法
                            NodeFound(reader);
                            break;
                    }

                    if (reader.LocalName == itemNodeName &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
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

        #region Abstract methods

        /// <summary>
        /// This method is called when an attribute is found, in order to notify the derived class of the existence
        /// of the attribute.
        /// </summary>
        /// <param name="namespaceName">The namespace for the attribute.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="attributeValue">The value of the attribute.</param>
        protected abstract void AttributeFound(string namespaceName, string attributeName, string attributeValue);

        /// <summary>
        /// Method called when a node is found during the processing of the "target item".
        /// </summary>
        /// <param name="reader"></param>
        protected abstract void NodeFound(XmlReader reader);

        #endregion

        #region Public properties

        /// <summary>
        /// All the attributes found in the "target item".
        /// </summary>
        public virtual AttributeReadWriteCollection Attributes
        {
            set { _attributes = value; }
            get { return _attributes; }
        }
        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }
        #endregion
    }
}
