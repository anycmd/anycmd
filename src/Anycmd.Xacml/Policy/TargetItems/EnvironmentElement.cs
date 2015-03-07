using System.Xml;

namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Represents a read-only Environment element in the Policy document. This class is a specialization of TargetItem class
	/// which contains the information needed for all the items that can be part of the target.
	/// </summary>
	public class EnvironmentElement : TargetItemBase
	{
		#region Constructor

		/// <summary>
		/// Creates a new instance of Environment using the specified arguments.
		/// </summary>
		/// <param name="match">The target item match collection.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public EnvironmentElement( TargetMatchReadWriteCollection match, XacmlVersion version ) : 
			base( match, version )
		{
		}

		/// <summary>
		/// Creates an instance of the Environment item and calls the base constructor specifying the names of the nodes
		/// that defines this target item.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Environment node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public EnvironmentElement( XmlReader reader, XacmlVersion version ) : 
			base( reader, Consts.Schema2.EnvironmentElement.Environment, Consts.Schema2.EnvironmentElement.EnvironmentMatch, version )
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
			return new EnvironmentMatchElement( reader, SchemaVersion );
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
