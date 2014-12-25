using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents an Resource node found in the context document. This class extends the abstract base class 
    /// TargetItem which loads the "target item" definition.
    /// </summary>
    public class ResourceElementReadWrite : TargetItemBase
    {
        #region Private members

        /// <summary>
        /// The contents of the ResourceContent node.
        /// </summary>
        private ResourceContentElementReadWrite _resourceContent;

        /// <summary>
        /// The scope of the resource if the request IsHierarchical
        /// </summary>
        private ResourceScope _resourceScope;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a Resource using the specified arguments.
        /// </summary>
        /// <param name="resourceScope">The resource scope for this target item.</param>
        /// <param name="resourceContent">The resource content in the context document.</param>
        /// <param name="attributes">The attribute list.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public ResourceElementReadWrite(ResourceContentElementReadWrite resourceContent, ResourceScope resourceScope, AttributeReadWriteCollection attributes, XacmlVersion schemaVersion)
            : base(attributes, schemaVersion)
        {
            _resourceContent = resourceContent;
            _resourceScope = resourceScope;
        }

        /// <summary>
        /// Creates an instance of the Resource class using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Subject node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public ResourceElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, Consts.ContextSchema.RequestElement.Resource, schemaVersion)
        {
            // Search for a hierarchical node mark
            foreach (AttributeElementReadWrite attribute in Attributes)
            {
                if (attribute.AttributeId == Consts.ContextSchema.ResourceScope.ResourceScopeAttributeId)
                {
                    foreach (AttributeValueElementReadWrite element in attribute.AttributeValues)
                    {
                        if (element.Contents == Consts.ContextSchema.ResourceScope.Immediate) continue;
                        _resourceScope = (ResourceScope)Enum.Parse(typeof(ResourceScope), element.Contents, false);
                        return;
                    }
                }
            }
        }

        #endregion

        #region Protected members

        /// <summary>
        /// Overrided to process custom attributes an attribute is found in the base class constructor.
        /// </summary>
        /// <param name="namespaceName">The namespace for the attribute.</param>
        /// <param name="attributeName">The attribute name found.</param>
        /// <param name="attributeValue">The attribute value found.</param>
        protected override void AttributeFound(string namespaceName, string attributeName, string attributeValue)
        {
        }

        /// <summary>
        /// Overrided to process custom nodes a node is found in the base class constructor.
        /// </summary>
        /// <remarks>This is used to avoid extra coding for all the target nodes</remarks>
        /// <param name="reader">The XmlReader pointing to the element found</param>
        protected override void NodeFound(XmlReader reader)
        {
            if (reader.LocalName == Consts.ContextSchema.ResourceElement.ResourceContent)
            {
                _resourceContent = new ResourceContentElementReadWrite(reader, SchemaVersion);
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The contents of the ResourceContent node.
        /// </summary>
        public virtual ResourceContentElementReadWrite ResourceContent
        {
            get { return _resourceContent; }
            set { _resourceContent = value; }
        }

        /// <summary>
        /// Whether the request is a hierarchical request
        /// </summary>
        public bool IsHierarchical
        {
            get { return (_resourceScope != ResourceScope.Immediate); }
        }

        /// <summary>
        /// The scope of the resource if the request IsHierarchical
        /// </summary>
        public virtual ResourceScope ResourceScopeValue
        {
            get { return _resourceScope; }
            set { _resourceScope = value; }
        }
        #endregion
    }
}
