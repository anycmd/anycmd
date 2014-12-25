
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.Interfaces
{
	/// <summary>
	/// Defines a generic element that has a Target.
	/// </summary>
	interface IHasTarget
	{
		/// <summary>
		/// Gets/Sets the target instance.
		/// </summary>
		pol.TargetElementReadWrite Target{ get; set; }
	}
}
