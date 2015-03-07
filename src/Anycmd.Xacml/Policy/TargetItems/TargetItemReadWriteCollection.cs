using System.Collections;


namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Defines a typed collection of read/write TargetItems.
	/// </summary>
	public class TargetItemReadWriteCollection : CollectionBase 
	{
		#region CollectionBase members

		/// <summary>
		/// Adds an object to the end of the CollectionBase.
		/// </summary>
		/// <param name="value">The Object to be added to the end of the CollectionBase. </param>
		/// <returns>The CollectionBase index at which the value has been added.</returns>
		public virtual int Add( TargetItemBaseReadWrite value )  
		{
			return( List.Add( value ) );
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
		public virtual new void RemoveAt ( int index )
		{
			base.RemoveAt(index);
		}

		#endregion
	}
}
