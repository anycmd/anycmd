using System;
using System.Xml;


namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Represents a read-only Subjects node defined in the policy document. This class extends the 
	/// abstract base class TargetItems which defines the elements of the Resources, Actions and
	/// Subjets nodes.
	/// </summary>
	public class SubjectsElement : SubjectsElementReadWrite
	{
		#region Constructors

		/// <summary>
		/// Creates a new Subjects with the specified aguments.
		/// </summary>
		/// <param name="anyItem">Whether the target item is defined for any item</param>
		/// <param name="items">The taregt items.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public SubjectsElement( bool anyItem, TargetItemReadWriteCollection items, XacmlVersion version )
			: base( anyItem, items, version )
		{
		}

		/// <summary>
		/// Creates an instance of the Subjects class using the XmlReader instance provided.
		/// </summary>
		/// <remarks>
		/// This constructor is also calling the base class constructor specifying the XmlReader
		/// and the node names that defines this TargetItmes extended class.
		/// </remarks>
		/// <param name="reader">The XmlReader positioned at the Subjects node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public SubjectsElement( XmlReader reader, XacmlVersion version ) : 
			base( reader, version )
		{
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Creates an instance of the containing element of the Subjects class. This method is 
		/// called by the TargetItems base class when an element that identifies a Subject is 
		/// found.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Action node.</param>
		/// <returns>A new instance of the Subject class.</returns>
		protected override TargetItemBaseReadWrite CreateTargetItem(XmlReader reader)
		{
			return new SubjectElement( reader, SchemaVersion );
		}

		#endregion

		#region Public properties
		/// <summary>
		/// The list of "target item"s defined in the "target item list" node.
		/// </summary>
		public override TargetItemReadWriteCollection ItemsList
		{
			set{ throw new NotSupportedException(); }
			get{ return new TargetItemCollection(base.ItemsList) ;}
		}
		#endregion
	}
}
