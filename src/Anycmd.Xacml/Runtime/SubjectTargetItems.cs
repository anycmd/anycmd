using Anycmd.Xacml.Policy.TargetItems;

namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// Represents the Subjects element of the policy document during evaluation. This instance 
	/// points to te policy "target item".
	/// </summary>
	public class SubjectTargetItems : TargetItems
	{
		#region Constructors

		/// <summary>
		/// Creates a new instance of the Aubjects class using the policy subjects definition.
		/// </summary>
		/// <param name="subjects">The policy subjects definition.</param>
		public SubjectTargetItems( SubjectsElement subjects ) : base( subjects )
		{
		}

		#endregion
	}
}
