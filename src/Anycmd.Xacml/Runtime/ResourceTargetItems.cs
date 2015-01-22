
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// Represents the Resources element of the policy document during evaluation. This instance 
	/// points to te policy "target item".
	/// </summary>
	public class ResourceTargetItems : TargetItems
	{
		#region Constructors

		/// <summary>
		/// Creates a new instance of the Aubjects class using the policy resources definition.
		/// </summary>
		/// <param name="resources">The policy resources definition.</param>
		public ResourceTargetItems( pol.ResourcesElement resources ) : base( resources )
		{
		}

		#endregion
	}
}
