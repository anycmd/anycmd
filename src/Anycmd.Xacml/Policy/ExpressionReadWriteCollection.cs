
namespace Anycmd.Xacml.Policy
{
    using Interfaces;
    using System.Collections;

    /// <summary>
    /// Defines a typed read/write collection of IExpression.
    /// </summary>
    public class ExpressionReadWriteCollection : CollectionBase
    {
        #region CollectionBase members

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public virtual int Add(IExpression value)
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

        /// <summary>
        /// Gets the index of the given IExpression in the collection
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public int GetIndex(IExpression expression)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.List[i] == expression)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion
    }
}
