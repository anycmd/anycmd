
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.Runtime
{
	/// <summary>
	/// Internal interface used to define a generic Obligations container for the runtime model.
	/// </summary>
	public interface IObligationsContainer
	{
		/// <summary>
		/// All the obligations that must be placed in the Response.
		/// </summary>
		pol.ObligationCollection Obligations{ get; }
	}
}
