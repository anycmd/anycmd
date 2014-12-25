using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only Condition element in the Policy document. This class extends the abstract class ApplyBase.
	/// </summary>
	public class ConditionElement : ConditionElementReadWrite
	{
		#region Constructors

		/// <summary>
		/// Createa an instance of the Condition class using the specified XmlReader. The base class constructor is
		/// also called with the XmlReader and the node name of Condition node.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Condition node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ConditionElement( XmlReader reader, XacmlVersion version ) 
			: base( reader, version )
		{
		}

		/// <summary>
		/// Creates a ConditionElement with the given parameters
		/// </summary>
		/// <param name="functionId"></param>
		/// <param name="arguments"></param>
		/// <param name="schemaVersion"></param>
		public ConditionElement( string functionId, IExpressionReadWriteCollection arguments, XacmlVersion schemaVersion)
			: base( functionId, arguments, schemaVersion )
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
			get{ return base.FunctionId; }
		}

		/// <summary>
		/// The arguments of the condition (or apply)
		/// </summary>
		public override IExpressionReadWriteCollection Arguments
		{
			set{ throw new NotSupportedException(); }
			get{ return new IExpressionCollection( base.Arguments ); }
		}

		#endregion
	}
}
