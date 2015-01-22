using System.Xml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read/write Match found in an Action element in the Policy document. This class extends the abstract 
	/// Match class which is used to mantain any Match element found in a target item.
	/// </summary>
	public class ActionMatchElementReadWrite : TargetMatchBaseReadWrite
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of the ActionMatch class using the specified arguments.
		/// </summary>
		/// <param name="matchId">The function id for this match.</param>
		/// <param name="attributeValue">The attribute value to use as the first parameter to the function.</param>
		/// <param name="attributeReference">The attribute reference in the context document.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ActionMatchElementReadWrite( string matchId, AttributeValueElementReadWrite attributeValue, AttributeReferenceBase attributeReference, XacmlVersion version ) : 
			base( matchId, attributeValue, attributeReference, version )
		{
		}

		/// <summary>
		/// Creates an instance of the ActionMatch class and also calls the base class constructor specifying the
		/// XmlReader, and the names of the node defined in this action item match.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the start of the Match element.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ActionMatchElementReadWrite( XmlReader reader, XacmlVersion version ) : 
			base( reader, Consts.Schema1.ActionElement.ActionMatch, Consts.Schema1.ActionElement.ActionAttributeDesignator, version )
		{
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Creates an instance of the ActionAttributeDesignator when the element is found during the Match node
		/// is being processed.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the start of the ActionAttributeDesignatot node.</param>
		/// <returns>An instance of the ActionAttributeDesignator class.</returns>
		protected override AttributeDesignatorBase CreateAttributeDesignator( XmlReader reader )
		{
			return new ActionAttributeDesignatorElement( reader, SchemaVersion );
		}

		#endregion

		#region Public properties
		/// <summary>
		/// Whether the instance is a read only version.
		/// </summary>
		public override bool IsReadOnly
		{
			get{ return false; }
		}
		#endregion
	}
}
