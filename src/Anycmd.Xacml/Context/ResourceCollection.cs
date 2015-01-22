using System;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Defines a typed collection of Resources.
    /// </summary>
    public class ResourceCollection : ResourceReadWriteCollection
    {
        #region Constructors
        /// <summary>
        /// Creates a new blank ResourceCollection
        /// </summary>
        public ResourceCollection()
        {
        }

        /// <summary>
        /// Creates a ResourceCollection with the provided ResourceReadWriteCollection
        /// </summary>
        /// <param name="items"></param>
        public ResourceCollection(ResourceReadWriteCollection items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (ResourceElementReadWrite item in items)
            {
                this.Add(item);
            }
        }

        #endregion

        #region CollectionBase members

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <value>The element at the specified index.</value>
        public override ResourceElementReadWrite this[int index]
        {
            get
            {
                return ((ResourceElement)List[index]);
            }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public override sealed int Add(ResourceElementReadWrite value)
        {
            if (value == null) throw new ArgumentNullException("value");
            ResourceContentElement resourceContent = null;
            if (value.ResourceContent != null)
            {
                resourceContent = new ResourceContentElement(value.ResourceContent.XmlDocument, value.ResourceContent.SchemaVersion);
            }
            return (List.Add(new ResourceElement(resourceContent, value.ResourceScopeValue, new AttributeCollection(value.Attributes), value.SchemaVersion)));
        }
        /// <summary>
        /// Clears the collection
        /// </summary>
        public override void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the specified element
        /// </summary>
        /// <param name="index">Position of the element</param>
        public override void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}