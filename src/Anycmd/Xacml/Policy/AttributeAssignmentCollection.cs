using System;

using cor = Anycmd.Xacml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Defines a typed collection of read-only AttributeAssignments.
	/// </summary>
	public class AttributeAssignmentCollection : AttributeAssignmentReadWriteCollection 
	{
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
		public override void RemoveAt ( int index )
		{
			throw new NotSupportedException();
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Creates a TargetItemCollection, with the items contained in a ReadWriteTargetItemCollection
		/// </summary>
		/// <param name="items"></param>
		public AttributeAssignmentCollection(AttributeAssignmentReadWriteCollection items)
		{
			if (items == null) throw new ArgumentNullException("items");
			foreach (cor.Policy.AttributeAssignmentElementReadWrite item in items)
			{
				base.Add(new AttributeAssignmentElement(item.AttributeId, item.DataTypeValue, item.Value, item.SchemaVersion));
			}
		}

		/// <summary>
		/// Creates a new blank TargetItemCollection
		/// </summary>
		public AttributeAssignmentCollection()
		{
		}
		#endregion
	}
}
