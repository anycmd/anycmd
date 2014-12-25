using System.Xml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only Match found in an Subject element in the Policy document. This class extends the abstract 
	/// Match class which is used to mantain any Match element found in a target item.
	/// </summary>
	public class SubjectMatchElement : TargetMatchBase
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of a SubjectMatch with the values specified.
		/// </summary>
		/// <param name="matchId">The match id</param>
		/// <param name="attributeValue">The attribute value instance.</param>
		/// <param name="attributeReference">An attribute reference instance.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public SubjectMatchElement( string matchId, AttributeValueElementReadWrite attributeValue, AttributeReferenceBase attributeReference, XacmlVersion version )
			: base( matchId, attributeValue, attributeReference, version )
		{
		}

		/// <summary>
		/// Creates an instance of the SubjectMatch class and also calls the base class constructor specifying the
		/// XmlReader, and the names of the node defined in this subject item match.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Match element.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public SubjectMatchElement( XmlReader reader, XacmlVersion version ) : 
			base( reader, Consts.Schema1.SubjectElement.SubjectMatch, Consts.Schema1.SubjectElement.SubjectAttributeDesignator, version )
		{
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Creates an instance of the SubjectAttributeDesignator when the element is found during the Match node
		/// is being processed.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the start of the SubjectAttributeDesignatot node.</param>
		/// <returns>An instance of the SubjectAttributeDesignator class.</returns>
		protected override AttributeDesignatorBase CreateAttributeDesignator( XmlReader reader )
		{
			return new SubjectAttributeDesignatorElement( reader, SchemaVersion );
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
