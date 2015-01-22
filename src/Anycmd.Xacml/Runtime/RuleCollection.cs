using System.Collections;


namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// Defines a typed collection of Rules.
	/// </summary>
	public class RuleCollection : CollectionBase 
	{
		#region CollectionBase members

		/// <summary>
		/// Adds an object to the end of the CollectionBase.
		/// </summary>
		/// <param name="value">The Object to be added to the end of the CollectionBase. </param>
		/// <returns>The CollectionBase index at which the value has been added.</returns>
		public int Add( Rule value )  
		{
			return( List.Add( value ) );
		}

		#endregion
	}
}
