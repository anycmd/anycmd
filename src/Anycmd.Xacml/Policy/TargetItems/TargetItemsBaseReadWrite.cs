using System;
using System.Xml;

using cor = Anycmd.Xacml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// This abstract base class is used to abstract the loading of the "target item list" found in the policy 
    /// document. Since the Resources, Actions and Subjects read/write nodes have a similar node structure they can be loaded 
    /// with the same code where the node names are changed by the derived class.
    /// </summary>
    public abstract class TargetItemsBaseReadWrite : XacmlElement
    {
        #region Private members

        /// <summary>
        /// Whether the "target item list" node defines the node Any[Subject|Action|Resource].
        /// </summary>
        private bool _anyItem;

        /// <summary>
        /// The list of "target items" defined in the node.
        /// </summary>
        private TargetItemReadWriteCollection _items = new TargetItemReadWriteCollection();

        #endregion

        #region Constructor

        /// <summary>
        /// Private default constructor to avoid default instantiation.
        /// </summary>
        protected TargetItemsBaseReadWrite(XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
        }

        /// <summary>
        /// Creates a new TargetItem collection with the specified aguments.
        /// </summary>
        /// <param name="anyItem">Whether the target item is defined for any item</param>
        /// <param name="items">The taregt items.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        protected TargetItemsBaseReadWrite(bool anyItem, TargetItemReadWriteCollection items, XacmlVersion schemaVersion)
            : this(schemaVersion)
        {
            _anyItem = anyItem;
            _items = items;
        }

        /// <summary>
        /// Creates a new TargetItem instance using the specified XmlReader instance provided, and the node names of 
        /// the "target item list" nodes which are provided by the derived class during construction. 
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the "target item list" node.</param>
        /// <param name="itemsNodeName">The name of the "target item list" node.</param>
        /// <param name="anyItemNodeName">The name of the AnyXxxx node for this "target item list" node.</param>
        /// <param name="itemNodeName">The name of the "target item" node that can be defined within this "target 
        /// item list" node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        protected TargetItemsBaseReadWrite(XmlReader reader, string itemsNodeName, string anyItemNodeName, string itemNodeName, XacmlVersion schemaVersion)
            : this(schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == itemsNodeName && ValidateSchema(reader, schemaVersion))
            {
                while (reader.Read())
                {
                    if (reader.LocalName == anyItemNodeName && ValidateSchema(reader, schemaVersion))
                    {
                        _anyItem = true;
                    }
                    else if (reader.LocalName == itemNodeName && ValidateSchema(reader, schemaVersion))
                    {
                        _items.Add((TargetItemBaseReadWrite)CreateTargetItem(reader));
                    }
                    else if (reader.LocalName == itemsNodeName &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(cor.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Abstract method called when a "target item" is found during the loading of the "target item list" node.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the "target item" node.</param>
        /// <returns>A new instance of the TargetItem derived class.</returns>
        protected abstract TargetItemBaseReadWrite CreateTargetItem(XmlReader reader);

        #endregion

        #region Public properties

        /// <summary>
        /// Whether the "target item list" defines the node Any[Subject|Action|Resource].
        /// </summary>
        public bool IsAny
        {
            get { return _anyItem; }
            set { _anyItem = value; }
        }

        /// <summary>
        /// The list of "target item"s defined in the "target item list" node.
        /// </summary>
        public virtual TargetItemReadWriteCollection ItemsList
        {
            set { _items = value; }
            get { return _items; }
        }

        #endregion
    }
}
