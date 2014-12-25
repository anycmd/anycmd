using System;

using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Defines a typed collection of read-only IExpression.
	/// </summary>
	public class IExpressionCollection : IExpressionReadWriteCollection 
	{

		#region Constructor
		/// <summary>
		/// Creates a IExpressionCollection, with the items contained in a IReadWriteExpressionCollection
		/// </summary>
		/// <param name="items"></param>
		public IExpressionCollection(IExpressionReadWriteCollection items)
		{
			if (items == null) throw new ArgumentNullException("items");
			foreach(inf.IExpression item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Creates a new blank IExpressionCollection
		/// </summary>
		public IExpressionCollection()
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
		public override int Add( inf.IExpression value )  
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
