using System.Collections;


namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// Defines a typed collection of "IMatchEvaluable"s.
	/// </summary>
	public class IMatchEvaluableCollection : CollectionBase 
	{
		#region CollectionBase members

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <value>The element at the specified index.</value>
		public IMatchEvaluable this[ int index ]  
		{
			get  
			{
				return( (IMatchEvaluable) List[index] );
			}
		}

		/// <summary>
		/// Adds an object to the end of the CollectionBase.
		/// </summary>
		/// <param name="value">The Object to be added to the end of the CollectionBase. </param>
		/// <returns>The CollectionBase index at which the value has been added.</returns>
		public int Add( IMatchEvaluable value )  
		{
			return( List.Add( value ) );
		}

		#endregion
	}
}
