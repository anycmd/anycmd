
namespace Anycmd.Xacml.Policy
{
    using Interfaces;
    using System;

    /// <summary>
    /// Defines a typed collection of read-only IExpression.
    /// </summary>
    public class ExpressionCollection : ExpressionReadWriteCollection
    {

        #region Constructor
        /// <summary>
        /// Creates a IExpressionCollection, with the items contained in a IReadWriteExpressionCollection
        /// </summary>
        /// <param name="items"></param>
        public ExpressionCollection(ExpressionReadWriteCollection items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (IExpression item in items)
            {
                List.Add(item);
            }
        }

        /// <summary>
        /// Creates a new blank IExpressionCollection
        /// </summary>
        public ExpressionCollection()
        {
        }
        #endregion

        #region CollectionBase members

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
