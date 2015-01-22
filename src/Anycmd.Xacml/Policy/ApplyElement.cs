using System;
using System.Xml;

using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Represents an Apply node in the Policy document. This class extends the abstract class ApplyBase which is the
	/// common base class for the Condition and the Apply nodes.
	/// </summary>
	public class ApplyElement : ApplyBaseReadWrite, inf.IExpression
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of the Apply class and calls the base class constructor specifying the XmlReader and
		/// the name of the node that defines the Apply.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the start of the Apply node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ApplyElement( XmlReader reader, XacmlVersion version ) 
			: base( reader, Consts.Schema1.ApplyElement.Apply, version )
		{
		}

		/// <summary>
		/// Creates a ConditionElement with the given parameters
		/// </summary>
		/// <param name="functionId"></param>
		/// <param name="arguments"></param>
		/// <param name="schemaVersion"></param>
		public ApplyElement( string functionId, IExpressionReadWriteCollection arguments, XacmlVersion schemaVersion)
			: base( functionId, arguments, schemaVersion)
		{
		}
		#endregion

		#region Public methods

		/// <summary>
		/// Writes the XML of the current element
		/// </summary>
		/// <param name="writer">The XmlWriter in which the element will be written</param>
		public override void WriteDocument(System.Xml.XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");
			writer.WriteStartElement(Consts.Schema1.ApplyElement.Apply);
			if( this.FunctionId != null && this.FunctionId.Length > 0 )
			{
				writer.WriteAttributeString(Consts.Schema1.ApplyElement.FunctionId,this.FunctionId);
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
