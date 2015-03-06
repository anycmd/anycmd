using System;
using System.Xml;


namespace Anycmd.Xacml.Policy 
{
	/// <summary>
	/// The ApplyBase class is used to define the common data used in the Apply and Condition read-only nodes.
	/// </summary>
	public abstract class ApplyBase : ApplyBaseReadWrite
	{
		#region Constructor

		/// <summary>
		/// Creates an instance of the ApplyBase using the XmlReader positioned in the node and the node name
		/// specifyed by the derived class in the constructor.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the "nodeName" node.</param>
		/// <param name="nodeName">The name of the node specifies by the derived class.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		protected ApplyBase( XmlReader reader, string nodeName, XacmlVersion schemaVersion )
			: base( reader, nodeName, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The id of the function used in this element.
		/// </summary>
		public override string FunctionId
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The arguments of the condition (or apply)
		/// </summary>
		public override ExpressionReadWriteCollection Arguments
		{
			get{ return new ExpressionCollection( base.Arguments ); }
			set{ throw new NotSupportedException(); }
		}

		#endregion

	}
}
