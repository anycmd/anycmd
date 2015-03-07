using System.Xml;


namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Represents a read/write Environments node defined in the policy document. This class extends the 
	/// abstract base class TargetItems which defines the elements of the Resources, Actions,
	/// Subjets and Environments nodes.
	/// </summary>
	public class EnvironmentsElementReadWrite : TargetItemsBaseReadWrite
	{
		#region Constructors

		/// <summary>
		/// Creates a new Environments with the specified aguments.
		/// </summary>
		/// <param name="anyItem">Whether the target item is defined for any item.</param>
		/// <param name="items">The taregt items.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public EnvironmentsElementReadWrite( bool anyItem, TargetItemReadWriteCollection items, XacmlVersion version )
			: base( anyItem, items, version )
		{
		}

		/// <summary>
		/// Creates an instance of the Environments class using the XmlReader instance provided.
		/// </summary>
		/// <remarks>
		/// This constructor is also calling the base class constructor specifying the XmlReader
		/// and the node names that defines this TargetItmes extended class.
		/// </remarks>
		/// <param name="reader">The XmlReader positioned at the Environments node.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public EnvironmentsElementReadWrite( XmlReader reader, XacmlVersion version ) 
			: base( reader, Consts.Schema2.TargetElement.Environments, Consts.Schema2.EnvironmentElement.AnyEnvironment, Consts.Schema2.EnvironmentElement.Environment, version )
		{
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Creates an instance of the containing element of the Environment class. This method is 
		/// called by the TargetItems base class when an element that identifies a Environment is 
		/// found.
		/// </summary>
		/// <param name="reader">The XmlReader positioned at the Environment node.</param>
		/// <returns>A new instance of the Environment class.</returns>
		protected override TargetItemBaseReadWrite CreateTargetItem( XmlReader reader )
		{
			return new EnvironmentElementReadWrite( reader, SchemaVersion );
		}

		#endregion

		#region Public methods
		/// <summary>
		/// Writes the XML of the current element
		/// </summary>
		/// <param name="writer">The XmlWriter in which the element will be written</param>
		public void WriteDocument( XmlWriter writer )
		{
			writer.WriteStartElement(Consts.Schema2.TargetElement.Environments);
			if(this.IsAny)
			{
				writer.WriteElementString(Consts.Schema2.EnvironmentElement.AnyEnvironment, string.Empty);
			}
			else
			{
				foreach(EnvironmentElementReadWrite oEnvironment in this.ItemsList)
				{
					writer.WriteStartElement(Consts.Schema2.EnvironmentElement.Environment);
					foreach(EnvironmentMatchElementReadWrite oItem in oEnvironment.Match)
					{
						writer.WriteStartElement(Consts.Schema2.EnvironmentElement.EnvironmentMatch);
						writer.WriteAttributeString(Consts.Schema1.MatchElement.MatchId, oItem.MatchId);
				
						writer.WriteStartElement(Consts.Schema1.AttributeValueElement.AttributeValue);
						writer.WriteAttributeString(Consts.Schema1.AttributeValueElement.DataType, oItem.AttributeValue.DataType);
						writer.WriteString(oItem.AttributeValue.Value);
						writer.WriteEndElement();
						if( oItem.AttributeReference is EnvironmentAttributeDesignatorElement )
						{
							((EnvironmentAttributeDesignatorElement)oItem.AttributeReference).WriteDocument(writer);
						}
						else if( oItem.AttributeReference is AttributeSelectorElement )
						{
							((AttributeSelectorElement)oItem.AttributeReference).WriteDocument(writer);
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
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
