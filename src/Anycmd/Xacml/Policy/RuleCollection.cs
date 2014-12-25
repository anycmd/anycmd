using System;

using cor = Anycmd.Xacml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Defines a typed collection of read-only Rules.
	/// </summary>
	public class RuleCollection : RuleReadWriteCollection 
	{

		#region Constructor
		/// <summary>
		/// Creates a TargetItemCollection, with the items contained in a ReadWriteTargetItemCollection
		/// </summary>
		/// <param name="items"></param>
		public RuleCollection(RuleReadWriteCollection items)
		{
			if (items == null) throw new ArgumentNullException("items");
			foreach (cor.Policy.RuleElementReadWrite item in items)
			{
				this.List.Add(new RuleElement(item.Id, item.Description, item.Target, item.Condition, item.Effect, item.SchemaVersion));
			}
		}

		/// <summary>
		/// Creates a new blank TargetItemCollection
		/// </summary>
		public RuleCollection()
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
		public override int Add( ReadWriteRuleElement value )  
		{
			throw new NotSupportedException();
		}
*/
		/// <summary>
		/// Removes the specified element
		/// </summary>
		/// <param name="index">Position of the element</param>
		public override void RemoveAt ( int index )
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}
