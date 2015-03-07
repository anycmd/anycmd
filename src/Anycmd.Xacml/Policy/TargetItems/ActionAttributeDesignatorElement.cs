using System;
using System.Xml;

namespace Anycmd.Xacml.Policy.TargetItems
{
	/// <summary>
	/// Represents an ActionAttributeDesignator in the Policy document.
	/// </summary>
	public class ActionAttributeDesignatorElement : AttributeDesignatorBase
	{
		#region Constructor

		/// <summary>
		/// Creates an instance of the ActionAttributeDesignator using the specified arguments.
		/// </summary>
		/// <param name="dataType">The id of data type of the referenced attribute in the context docuemnt.</param>
		/// <param name="mustBePresent">Whether the attribute must be present or not.</param>
		/// <param name="attributeId">The id of the attribute in the context document.</param>
		/// <param name="issuer">The issuer of the attribute.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ActionAttributeDesignatorElement( string dataType, bool mustBePresent, string attributeId, string issuer, XacmlVersion version )
			: base( dataType, mustBePresent, attributeId, issuer, version )
		{
		}

		/// <summary>
		/// Creates an instance of the ActionAttributeDesignator which is a derived class from the 
		/// AttributeDesignator abstract class. This constructor is also calling the base class constructor 
		/// specifying the XmlReader instance.
		/// </summary>
		/// <param name="reader">An XmlReader positioned at the start of the ActionAttributeDesignator element.</param>
		/// <param name="version">The version of the schema that was used to validate.</param>
		public ActionAttributeDesignatorElement( XmlReader reader, XacmlVersion version ) : 
			base( reader, version )
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
			writer.WriteStartElement(Consts.Schema1.ActionAttributeDesignatorElement.ActionAttributeDesignator);
			writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.AttributeId,this.AttributeId);
			writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.DataType,this.DataType);
			if( this.Issuer.Length != 0 )
			{
				writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.Issuer,this.Issuer);
			}
			if(this.MustBePresent)
			{
				writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.MustBePresent,"true");
			}
			else
			{
				writer.WriteAttributeString(Consts.Schema1.AttributeDesignatorElement.MustBePresent,"false");
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
