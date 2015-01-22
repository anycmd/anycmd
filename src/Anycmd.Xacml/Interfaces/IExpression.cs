
namespace Anycmd.Xacml.Interfaces
{
	/// <summary>
	/// Marker interface used to tag all the elements than can be used as an argument in the Apply
	/// evaluation.
	/// </summary>
	public interface IExpression
	{
		/// <summary>
		/// Writes the XML of the current element
		/// </summary>
		/// <param name="writer">The XmlWriter in which the element will be written</param>
		void WriteDocument(System.Xml.XmlWriter writer);
	}
}
