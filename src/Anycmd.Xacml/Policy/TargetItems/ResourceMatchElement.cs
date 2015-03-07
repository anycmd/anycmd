using System.Xml;

namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Represents a read-only Match found in an Resource element in the Policy document. This class extends the abstract 
	/// Match class which is used to mantain any Match element found in a target item.
	/// </summary>
	public class ResourceMatchElement : TargetMatchBase
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of the ResourceMatch class using the specified arguments.
		/// </summary>
		/// <param name="matchId">The function id for this match.</param>
		/// <param name="attributeValue">The attribute value to use as the first parameter to the function.</param>
		/// <param name="attributeReference">The attribute reference in the context document.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ResourceMatchElement( string matchId, AttributeValueElementReadWrite attributeValue, AttributeReferenceBase attributeReference, XacmlVersion version ) : 
			base( matchId, attributeValue, attributeReference, version )
		{
		}

		/// <summary>
		/// Creates an instance of the ResourceMatch class and also calls the base class constructor specifying the
		/// XmlReader, and the names of the node defined in this resource item match.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the start of the Match element.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ResourceMatchElement( XmlReader reader, XacmlVersion version ) : 
			base( reader, Consts.Schema1.ResourceElement.ResourceMatch, Consts.Schema1.ResourceElement.ResourceAttributeDesignator, version )
		{
		}
		#endregion

		#region Protected methods

		/// <summary>
		/// Creates an instance of the ResourceAttributeDesignator when the element is found during the Match node
		/// is being processed.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the start of the ResourceAttributeDesignatot node.</param>
		/// <returns>An instance of the ResourceAttributeDesignator class.</returns>
		protected override AttributeDesignatorBase CreateAttributeDesignator( XmlReader reader )
		{
			return new ResourceAttributeDesignatorElement( reader, SchemaVersion );
		}

		#endregion

		#region Public properties
		/// <summary>
		/// Whether the instance is a read only version.
		/// </summary>
		public override bool IsReadOnly
		{
			get{ return true; }
		}
		#endregion
	}
}
