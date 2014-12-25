using System.Collections;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Defines a typed collection of AttributeValueElement with a read-write access.
    /// </summary>
    public class AttributeValueElementReadWriteCollection : CollectionBase
    {
        #region CollectionBase members

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <value>The element at the specified index.</value>
        public virtual AttributeValueElementReadWrite this[int index]
        {
            get
            {
                return ((AttributeValueElementReadWrite)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public virtual int Add(AttributeValueElementReadWrite value)
        {
            return (List.Add(value));
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public virtual new void Clear()
        {
            base.Clear();
        }
        /// <summary>
        /// Removes the specified element
        /// </summary>
        /// <param name="index">Position of the element</param>
        public virtual new void RemoveAt(int index)
        {
            base.RemoveAt(index);
        }
        #endregion
    }
}
