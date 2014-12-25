using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only Function element found in the Policy document that is used as an argument in the Apply 
	/// (or Condition) evaluation.
	/// </summary>
	public class FunctionElement : FunctionElementReadWrite
	{
		#region Constructors

		/// <summary>
		/// Creates a new instance of the Function class using the XmlReader specified.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Function node.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public FunctionElement( XmlReader reader, XacmlVersion schemaVersion )
			: base( reader, schemaVersion )
		{
		}

		/// <summary>
		/// Creates a new instance of the Function class using the provided values.
		/// </summary>
		/// <param name="functionId">The function id</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public FunctionElement( string functionId, XacmlVersion schemaVersion ) : base( functionId, schemaVersion )
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The id of the function used as an argument to the condition or apply evaluation.
		/// </summary>
		public override string FunctionId
		{
			set{ throw new NotSupportedException(); }
		}

		#endregion

	}
}
