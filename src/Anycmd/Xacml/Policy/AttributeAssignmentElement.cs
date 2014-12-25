using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read-only AttributeAssignment node found in the Policy document. This element is not usefull in the 
	/// specification because they are only copied to the context if the Obligation is satified.
	/// </summary>
	public class AttributeAssignmentElement : AttributeAssignmentElementReadWrite
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of the AttributeAssignment using the provided XmlReader.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the AttributeAssignament node.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		public AttributeAssignmentElement( XmlReader reader, XacmlVersion schemaVersion )
			: base( reader, schemaVersion )
		{
		}

		/// <summary>
		/// Creates an instance of the AttributeAssignment using the provided values
		/// </summary>
		/// <param name="attributeId"></param>
		/// <param name="dataType"></param>
		/// <param name="contents"></param>
		/// <param name="version"></param>
		public AttributeAssignmentElement( string attributeId, string dataType, string contents, XacmlVersion version ) : 
			base( attributeId, dataType, contents, version )
		{
		}

		#endregion

		#region Public properties

		
		/// <summary>
		/// Gets the value of the assignent.
		/// </summary>
		public override string Value
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// Gets the data type of the attribute assignment.
		/// </summary>
		public override string DataTypeValue
		{
			set{ throw new NotSupportedException(); }
		}

		/// <summary>
		/// The id of the attribute.
		/// </summary>
		public override string AttributeId
		{
			set{ throw new NotSupportedException(); }
		}

		#endregion
	}
}
