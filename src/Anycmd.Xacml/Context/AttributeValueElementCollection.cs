using System;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Defines a typed collection of AttributeValueElement.
    /// </summary>
    public class AttributeValueElementCollection : AttributeValueElementReadWriteCollection
    {
        #region Constructor
        /// <summary>
        /// Creates a AttributeValueElementCollection, with the items contained in a AttributeValueElementReadWriteCollection
        /// </summary>
        /// <param name="items"></param>
        public AttributeValueElementCollection(AttributeValueElementReadWriteCollection items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (AttributeValueElementReadWrite item in items)
            {
                this.List.Add(new AttributeValueElement(item.Contents, item.SchemaVersion));
            }
        }
        /// <summary>
        /// Creates a new blank AttributeValueElementCollection
        /// </summary>
        public AttributeValueElementCollection()
        {
        }
        #endregion

        #region CollectionBase members

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <value>The element at the specified index.</value>
        public override AttributeValueElementReadWrite this[int index]
        {
            get
            {
                return ((AttributeValueElement)List[index]);
            }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public override void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public override int Add(AttributeValueElementReadWrite value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return (List.Add(new AttributeValueElement(value.Value, value.SchemaVersion)));
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
