using System.Xml;

namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Represents a read-only Resource node defined in the policy document. This class extends TargetItem
	/// since Sibject, Action and Resource share a similar document structure.
	/// </summary>
	public class ResourceElement : TargetItemBase
	{
		#region Constructors

		/// <summary>
		/// Creates a new Resource with the specified aguments.
		/// </summary>
		/// <param name="match">The target item match collection.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ResourceElement( TargetMatchReadWriteCollection match, XacmlVersion version )
			: base( match, version )
		{
		}

		/// <summary>
		/// Creates a new Resource using the XmlReader instance provided.
		/// </summary>
		/// <remarks>This constructor is also calling the base class constructor specifying the
		/// XmlReader instance and the node names for the Resource element and the 
		/// ResourceMatch.</remarks>
		/// <param name="reader">An XmlReader positioned at the Resource node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ResourceElement( XmlReader reader, XacmlVersion version ) 
			: base( reader, Consts.Schema1.ResourceElement.Resource, Consts.Schema1.ResourceElement.ResourceMatch, version )
		{
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// This method is called by the TargetItem class when an ResourceMatch element is found. 
		/// This method creates an instance of the ResourceMatch class using the XmlReader 
		/// provided.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the begining of the ResourceMatch
		/// node.</param>
		/// <returns>A new instance of the ResourceMatch element.</returns>
		protected override TargetMatchBaseReadWrite CreateMatch( XmlReader reader )
		{
			return new ResourceMatchElement( reader, SchemaVersion );
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
