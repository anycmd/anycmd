using System;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Defines a typed collection of read-only Obligations.
	/// </summary>
	public class ObligationCollection : ObligationReadWriteCollection 
	{
		#region CollectionBase members
/*
		/// <summary>
		/// Adds an object to the end of the CollectionBase.
		/// </summary>
		/// <param name="value">The Object to be added to the end of the CollectionBase. </param>
		/// <returns>The CollectionBase index at which the value has been added.</returns>
		public override int Add( ReadWriteObligationElement value )  
		{
			throw new NotSupportedException();
		}
*/
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
		public override void RemoveAt ( int index )
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Creates a ObligationCollection, with the items contained in a ReadWriteObligationCollection
		/// </summary>
		/// <param name="items"></param>
		public ObligationCollection(ObligationReadWriteCollection items)
		{
			if (items == null) throw new ArgumentNullException("items");
			foreach (ObligationElementReadWrite item in items)
			{
				base.Add(new ObligationElement(item.ObligationId, item.FulfillOn, item.AttributeAssignment));
			}
		}

		/// <summary>
		/// Creates a new blank ObligationCollection
		/// </summary>
		public ObligationCollection()
		{
		}
		#endregion
	}
}
