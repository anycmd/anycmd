
namespace Anycmd.Xacml.Context
{
    using System;

    /// <summary>
    /// Defines a typed collection of Attribute.
    /// </summary>
    public class AttributeCollection : AttributeReadWriteCollection
    {
        #region Constructor
        /// <summary>
        /// Creates a AttributeCollection, with the items contained in a AttributeReadWriteCollection
        /// </summary>
        /// <param name="items"></param>
        public AttributeCollection(AttributeReadWriteCollection items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (AttributeElementReadWrite item in items)
            {
                this.List.Add(new AttributeElement(item.AttributeId, item.DataType, item.Issuer, item.IssueInstant,
                    item.Value, item.SchemaVersion));
            }
        }
        /// <summary>
        /// Creates a new blank AttributeCollection
        /// </summary>
        public AttributeCollection()
        {
        }
        #endregion

        #region CollectionBase members

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <value>The element at the specified index.</value>
        public override AttributeElementReadWrite this[int index]
        {
            get
            {
                return ((AttributeElement)List[index]);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public override int Add(AttributeElementReadWrite value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return (List.Add(new AttributeElement(value.AttributeId, value.DataType, value.Issuer, value.IssueInstant, value.Value,
                value.SchemaVersion)));
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
