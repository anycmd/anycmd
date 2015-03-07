using System;
using System.Xml;


namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// This abstract base class is used to unify the processing of the "target item", Resource, Action and 
	/// Subject nodes found in the policy document with a read-only access. As all those nodes have a similar representation in the 
	/// policy document they can be loaded by the same code. 
	/// </summary>
	public abstract class TargetItemBase : TargetItemBaseReadWrite
	{
		#region Constructors

		/// <summary>
		/// Private default constructor to avoid default instantiation.
		/// </summary>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		protected TargetItemBase( XacmlVersion schemaVersion )
			: base( schemaVersion )
		{
		}

		/// <summary>
		/// Creates a new instance of TargetItem using the specified arguments.
		/// </summary>
		/// <param name="match">The collection of target item matchs.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		protected TargetItemBase( TargetMatchReadWriteCollection match, XacmlVersion schemaVersion )
			: base( match, schemaVersion )
		{
		}

		/// <summary>
		/// Creates an instance of the TargetItem using the XmlReader instance provided and the node names provided
		/// by the extended class which allows this code being independent of the node names.
		/// </summary>
		/// <param name="reader">The XmlReader instance positioned at the "target item" node (Resource, Action or 
		/// Subject).</param>
		/// <param name="targetItemNodeName">The name of the node that represents the "target item".</param>
		/// <param name="matchNodeName">The name of the node that represents the Match element for the "target item" 
		/// being loaded.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		protected TargetItemBase( XmlReader reader, string targetItemNodeName, string matchNodeName, XacmlVersion schemaVersion )
			: base( reader, targetItemNodeName, matchNodeName, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The list of Match elements found in the target item node.
		/// </summary>
		public override TargetMatchReadWriteCollection Match
		{
			set{ throw new NotSupportedException(); }
			get{ return new TargetMatchCollection( base.Match ); }
		}

		#endregion
	}
}
