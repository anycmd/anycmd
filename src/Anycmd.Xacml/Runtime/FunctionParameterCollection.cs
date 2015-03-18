using System.Collections;

using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// Defines a typed collection of IFunctionParameters.
	/// </summary>
	public class FunctionParameterCollection : CollectionBase 
	{
		#region CollectionBase members

		/// <summary>
		/// Adds an object to the end of the CollectionBase.
		/// </summary>
		/// <param name="value">The Object to be added to the end of the CollectionBase. </param>
		/// <returns>The CollectionBase index at which the value has been added.</returns>
		public int Add( inf.IFunctionParameter value )  
		{
			return( List.Add( value ) );
		}

		/// <summary>
		/// Returns a IFunctionParameter[] of the Collection contents.
		/// </summary>
		/// <returns></returns>
		public inf.IFunctionParameter[] ToArray()
		{
			inf.IFunctionParameter[] retArr = new inf.IFunctionParameter[ Count ];
			List.CopyTo( retArr, 0 );
			return retArr;
		}

		#endregion
	}
}
