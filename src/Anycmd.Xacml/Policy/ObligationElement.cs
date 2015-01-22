using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only Obligation node found in the Policy document.
	/// </summary>
	public class ObligationElement : ObligationElementReadWrite
	{
		#region Constructors
		/// <summary>
		/// Creates an ObligationElement with the parameters given
		/// </summary>
		/// <param name="obligationId"></param>
		/// <param name="fulfillOn"></param>
		/// <param name="attributeAssignment"></param>
		public ObligationElement( string obligationId, Effect fulfillOn, AttributeAssignmentReadWriteCollection attributeAssignment ) : 
			base( obligationId, fulfillOn, attributeAssignment)
		{
		}

		/// <summary>
		/// Creates a new instance of the Obligation class using the XmlReader instance provided.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Obligation node.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public ObligationElement( XmlReader reader, XacmlVersion schemaVersion )
			: base( reader, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// Gets all the attribute assignments for this obligation.
		/// </summary>
		public override AttributeAssignmentReadWriteCollection AttributeAssignment
		{
			get{ return new AttributeAssignmentCollection( base.AttributeAssignment ); }
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The effect that will trigger the obligation to be sent to the response.
		/// </summary>
		public override Effect FulfillOn
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The obligation id.
		/// </summary>
		public override string ObligationId
		{
			set{ throw new NotSupportedException(); }
		}

		#endregion
	}
}
