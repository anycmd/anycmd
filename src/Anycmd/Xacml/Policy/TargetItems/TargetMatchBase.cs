using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a generic read-only match found in the target items of the Policy document. 
	/// </summary>
	public abstract class TargetMatchBase : TargetMatchBaseReadWrite
	{
		#region Constructor

		/// <summary>
		/// Creates an instance of a TargetMatchBase with the values specified.
		/// </summary>
		/// <param name="matchId">The match id</param>
		/// <param name="attributeValue">The attribute value instance.</param>
		/// <param name="attributeReference">An attribute reference instance.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		protected TargetMatchBase( string matchId, AttributeValueElementReadWrite attributeValue, AttributeReferenceBase attributeReference, XacmlVersion schemaVersion )
			: base( matchId, attributeValue, attributeReference, schemaVersion )
		{
		}

		/// <summary>
		/// Creates an instance of the TargetMatchBase class using the XmlReader specified, the name of the node that defines
		/// the match (the name of the node changes depending on the target item that defines it) and the attribute
		/// designator node name which also changes depending on the target item that defines the match.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the "matchNodeName" node.</param>
		/// <param name="matchNodeName">The name of the match node for this target item.</param>
		/// <param name="attributeDesignatorNode">The name of the attribute designator node for this target item.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		protected TargetMatchBase( XmlReader reader, string matchNodeName, string attributeDesignatorNode, XacmlVersion schemaVersion )
			: base( reader, matchNodeName, attributeDesignatorNode, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The id of the mathc which is the id of the function used to evaluate the match.
		/// </summary>
		public override string MatchId
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The attribute value used as the first argument of the function.
		/// </summary>
		public override AttributeValueElementReadWrite AttributeValue
		{
			get{ return new AttributeValueElement( base.AttributeValue.DataType, base.AttributeValue.Value, base.AttributeValue.SchemaVersion ); }
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The attribute reference used as a second argument of the function. This reference is resolved by the 
		/// EvaluationEngine before passing the value to the function.
		/// </summary>
		public override AttributeReferenceBase AttributeReference
		{
			set{ throw new NotSupportedException(); }
		}

		#endregion
	}
}
