using System.Xml;

namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Represents a read-only Action element in the Policy document. This class is a specialization of TargetItem class
	/// which contains the information needed for all the items that can be part of the target.
	/// </summary>
	public class ActionElement : TargetItemBase
	{
		#region Constructor

		/// <summary>
		/// Creates a new instance of Action using the specified arguments.
		/// </summary>
		/// <param name="match">The target item match collection.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ActionElement( TargetMatchReadWriteCollection match, XacmlVersion version ) : 
			base( match, version )
		{
		}

		/// <summary>
		/// Creates an instance of the Action item and calls the base constructor specifying the names of the nodes
		/// that defines this target item.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Action node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ActionElement( XmlReader reader, XacmlVersion version ) : 
			base( reader, Consts.Schema1.ActionElement.Action, Consts.Schema1.ActionElement.ActionMatch, version )
		{
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Overrided method that is called when the xxxMatch element is found in the target item definition.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the start of the Match element found.</param>
		/// <returns>The instance of the ActionMatch which is a class extending the abstract Match class</returns>
		protected override TargetMatchBaseReadWrite CreateMatch(XmlReader reader)
		{
			return new ActionMatchElement( reader, SchemaVersion );
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
