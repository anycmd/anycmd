
namespace Anycmd.Xacml.Context
{
	/// <summary>
	/// Defines the ResourceScope defined in the respurce node of the context document. As defined in the Section 
	/// 7.8 of the specification.
	/// </summary>
	public enum ResourceScope
	{
		/// <summary>
		/// No hierarchy
		/// </summary>
		Immediate, 
		
		/// <summary>
		/// Evaluate the resource level and its direct childrens
		/// </summary>
		Children, 
		
		/// <summary>
		/// Evaluate the resource level, its direct and indirect childrens
		/// </summary>
		Descendants
	}
}
