using System;

namespace Anycmd.Xacml.Policy.TargetItems
{
    /// <summary>
    /// Defines a typed collection of read-only Matchs.
    /// </summary>
    public class TargetMatchCollection : TargetMatchReadWriteCollection
    {
        #region Constructor
        /// <summary>
        /// Creates a TargetMatchCollection, with the items contained in a ReadWriteTargetMatchCollection
        /// </summary>
        /// <param name="items"></param>
        public TargetMatchCollection(TargetMatchReadWriteCollection items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (TargetMatchBaseReadWrite item in items)
            {
                var sItem = item as SubjectMatchElementReadWrite;
                var aItem = item as ActionMatchElementReadWrite;
                var rItem = item as ResourceMatchElementReadWrite;
                var eItem = item as EnvironmentMatchElementReadWrite;
                if (sItem != null)
                {
                    this.List.Add(new SubjectMatchElement(sItem.MatchId, sItem.AttributeValue, sItem.AttributeReference, sItem.SchemaVersion));
                }
                else if (aItem != null)
                {
                    this.List.Add(new ActionMatchElement(aItem.MatchId, aItem.AttributeValue, aItem.AttributeReference, aItem.SchemaVersion));
                }
                else if (rItem != null)
                {
                    this.List.Add(new ResourceMatchElement(rItem.MatchId, rItem.AttributeValue, rItem.AttributeReference, rItem.SchemaVersion));
                }
                else if (eItem != null)
                {
                    this.List.Add(new EnvironmentMatchElement(eItem.MatchId, eItem.AttributeValue, eItem.AttributeReference, eItem.SchemaVersion));
                }
            }
        }
        /// <summary>
        /// Creates a new blank TargetMatchCollection
        /// </summary>
        public TargetMatchCollection()
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
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public override int Add(TargetMatchBaseReadWrite value)
        {
            return (List.Add((TargetMatchBase)value));
        }

        /// <summary>
        /// Removes the specified element
        /// </summary>
        /// <param name="index">Position of the element</param>
        public override void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Get/Set a ReadWriteTargetMatchBase of the ReadWriteTargetMatchCollection
        /// </summary>
        public override TargetMatchBaseReadWrite this[int index]
        {
            get { return (TargetMatchBase)this.List[index]; }
            set { throw new NotSupportedException(); }
        }
        #endregion
    }
}
