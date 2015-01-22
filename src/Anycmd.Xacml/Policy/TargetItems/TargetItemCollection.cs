using System;

using cor = Anycmd.Xacml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Defines a typed collection of read-only TargetItems.
    /// </summary>
    public class TargetItemCollection : TargetItemReadWriteCollection
    {
        #region Constructor

        /// <summary>
        /// Creates a TargetItemCollection, with the items contained in a ReadWriteTargetItemCollection
        /// </summary>
        /// <param name="items"></param>
        public TargetItemCollection(TargetItemReadWriteCollection items)
        {
            if (items == null) throw new ArgumentNullException("items");
            foreach (cor.Policy.TargetItemBaseReadWrite item in items)
            {
                SubjectElementReadWrite sItem = item as SubjectElementReadWrite;
                ActionElementReadWrite aItem = item as ActionElementReadWrite;
                ResourceElementReadWrite rItem = item as ResourceElementReadWrite;
                EnvironmentElementReadWrite eItem = item as EnvironmentElementReadWrite;
                if (sItem != null)
                {
                    this.List.Add(new SubjectElement(sItem.Match, sItem.SchemaVersion));
                }
                else if (aItem != null)
                {
                    this.List.Add(new ActionElement(aItem.Match, aItem.SchemaVersion));
                }
                else if (rItem != null)
                {
                    this.List.Add(new ResourceElement(rItem.Match, rItem.SchemaVersion));
                }
                else if (eItem != null)
                {
                    this.List.Add(new EnvironmentElement(eItem.Match, eItem.SchemaVersion));
                }
            }
        }

        /// <summary>
        /// Creates a new blank TargetItemCollection
        /// </summary>
        public TargetItemCollection()
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
        /*
                /// <summary>
                /// Adds an object to the end of the CollectionBase.
                /// </summary>
                /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
                /// <returns>The CollectionBase index at which the value has been added.</returns>
                public override int Add( TargetItemBase value )  
                {
                    throw new NotSupportedException();
                }
        */
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
