using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
	/// <summary>
	/// Represents an Resource node found in the context document. This class extends the abstract base class 
	/// TargetItem which loads the "target item" definition.
	/// </summary>
	public class ResourceElement : ResourceElementReadWrite
	{
		#region Constructor

		/// <summary>
		/// Creates a Resource using the specified arguments.
		/// </summary>
		/// <param name="resourceScope">The resource scope for this target item.</param>
		/// <param name="resourceContent">The resource content in the context document.</param>
		/// <param name="attributes">The attribute list.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public ResourceElement( ResourceContentElement resourceContent, ResourceScope resourceScope, AttributeCollection attributes, XacmlVersion schemaVersion ) 
			: base( resourceContent, resourceScope, attributes, schemaVersion )
		{
		}

		/// <summary>
		/// Creates an instance of the Resource class using the XmlReader instance provided.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Subject node.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public ResourceElement( XmlReader reader, XacmlVersion schemaVersion ) 
			: base( reader, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The contents of the ResourceContent node.
		/// </summary>
		public override ResourceContentElementReadWrite ResourceContent
		{
			get
			{
			    return base.ResourceContent != null ? new ResourceContentElement( base.ResourceContent.XmlDocument, base.ResourceContent.SchemaVersion ) : null;
			}
		    set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The scope of the resource if the request IsHierarchical
		/// </summary>
		public override ResourceScope ResourceScopeValue
		{
			set{ throw new NotSupportedException(); }
		}
		/// <summary>
		/// 
		/// </summary>
		public override AttributeReadWriteCollection Attributes
		{
			get
			{
				return new AttributeCollection( base.Attributes );
			}
			set
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		#endregion
	}
}
