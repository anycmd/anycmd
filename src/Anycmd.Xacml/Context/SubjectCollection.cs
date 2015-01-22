using System;


namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Defines a typed collection of Subjects.
    /// </summary>
    public class SubjectCollection : SubjectReadWriteCollection
    {
        #region Constructors
        /// <summary>
        /// Creates a new blank SubjectCollection
        /// </summary>
        public SubjectCollection()
        {
        }

        /// <summary>
        /// Creates a SubjectCollection with the provided SubjectReadWriteCollection
        /// </summary>
        /// <param name="items"></param>
        public SubjectCollection(SubjectReadWriteCollection items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (SubjectElementReadWrite item in items)
            {
                this.Add(item);
            }
        }

        #endregion

        #region CollectionBase members

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public override sealed int Add(SubjectElementReadWrite value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return (List.Add(new SubjectElement(value.SubjectCategory, new AttributeCollection(value.Attributes), value.SchemaVersion)));
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