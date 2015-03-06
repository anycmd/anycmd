using System.Collections;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Defines a typed collection of Attribute.
    /// </summary>
    public class AttributeReadWriteCollection : CollectionBase
    {
        #region CollectionBase members

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <value>The element at the specified index.</value>
        public virtual AttributeElementReadWrite this[int index]
        {
            get
            {
                return ((AttributeElementReadWrite)List[index]);
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
        public virtual int Add(AttributeElementReadWrite value)
        {
            return (List.Add(value));
        }

        /// <summary>
        /// Gets the index of the given AttributeElementReadWrite in the collection
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public int GetIndex(AttributeElementReadWrite attribute)
        {
            for (var i = 0; i < this.Count; i++)
            {
                if (this.List[i] == attribute)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion
    }
}
