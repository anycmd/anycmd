using System.Xml;

namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Represents a read-only Subject node defined in the policy document. This class extends TargetItem
	/// since Subject, Action and Resource share a similar document structure.
	/// </summary>
	public class SubjectElement : TargetItemBase
	{
		#region Constructors

		/// <summary>
		/// Creates a new Subject with the specified aguments.
		/// </summary>
		/// <param name="match">The target item match collection.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public SubjectElement( TargetMatchReadWriteCollection match, XacmlVersion version )
			: base( match, version )
		{
		}

		/// <summary>
		/// Creates a new Subject using the XmlReader instance provided.
		/// </summary>
		/// <remarks>This constructor is also calling the base class constructor specifying the
		/// XmlReader instance and the node names for the Subject element and the 
		/// ResourceMatch.</remarks>
		/// <param name="reader">An XmlReader positioned at the Subject node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public SubjectElement( XmlReader reader, XacmlVersion version ) : 
			base( reader, Consts.Schema1.SubjectElement.Subject, Consts.Schema1.SubjectElement.SubjectMatch, version )
		{
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// This method is called by the TargetItem class when a SubjectMatch element is found. 
		/// This method creates an instance of the SubjectMatch class using the XmlReader 
		/// provided.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the begining of the SubjectMatch
		/// node.</param>
		/// <returns>A new instance of the SubjectMatch element.</returns>
		protected override TargetMatchBaseReadWrite CreateMatch(XmlReader reader)
		{
			return new SubjectMatchElement( reader, SchemaVersion );
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
