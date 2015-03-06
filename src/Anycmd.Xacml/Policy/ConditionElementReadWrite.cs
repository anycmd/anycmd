using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents a read/write Condition element in the Policy document. This class extends the abstract class ApplyBase.
	/// </summary>
	public class ConditionElementReadWrite : ApplyBaseReadWrite
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of the Condition class using the specified XmlReader. The base class constructor is
		/// also called with the XmlReader and the node name of Condition node.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Condition node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ConditionElementReadWrite( XmlReader reader, XacmlVersion version )
			: base(reader, Consts.Schema1.RuleElement.Condition, version)
		{
		}
		/// <summary>
		/// Creates a ConditionElement with the given parameters
		/// </summary>
		/// <param name="functionId"></param>
		/// <param name="arguments"></param>
		/// <param name="schemaVersion"></param>
		public ConditionElementReadWrite(string functionId, ExpressionReadWriteCollection arguments, XacmlVersion schemaVersion)
			: base( functionId, arguments, schemaVersion )
		{
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Writes the XML of the current element
		/// </summary>
		/// <param name="writer">The XmlWriter in which the element will be written</param>
		public override void WriteDocument(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");
			writer.WriteStartElement(Consts.Schema1.ConditionElement.Condition);
			if( !string.IsNullOrEmpty(this.FunctionId) )
			{
				writer.WriteAttributeString(Consts.Schema1.ConditionElement.FunctionId,this.FunctionId);
			}
			
			foreach(Anycmd.Xacml.Interfaces.IExpression oExpression in this.Arguments)
			{
				oExpression.WriteDocument(writer);
			}

			writer.WriteEndElement();
		}

		#endregion

		#region Public properties
		/// <summary>
		/// Whether the instance is a read only version.
		/// </summary>
		public override bool IsReadOnly
		{
			get{ return false; }
		}
		#endregion
	}
}
