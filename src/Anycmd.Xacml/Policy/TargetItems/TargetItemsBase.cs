using System;
using System.Xml;

using cor = Anycmd.Xacml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// This abstract base class is used to abstract the loading of the "target item list" found in the policy 
	/// document. Since the Resources, Actions and Subjects read-only nodes have a similar node structure they can be loaded 
	/// with the same code where the node names are changed by the derived class.
	/// </summary>
	public abstract class TargetItemsBase : TargetItemsBaseReadWrite
	{

		#region Constructor

		/// <summary>
		/// Private default constructor to avoid default instantiation.
		/// </summary>
		protected TargetItemsBase(XacmlVersion schemaVersion)
			: base(schemaVersion)
		{
		}

		/// <summary>
		/// Creates a new TargetItem collection with the specified aguments.
		/// </summary>
		/// <param name="anyItem">Whether the target item is defined for any item</param>
		/// <param name="items">The taregt items.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		protected TargetItemsBase(bool anyItem, TargetItemCollection items, XacmlVersion schemaVersion)
			: base(anyItem, items, schemaVersion)
		{
		}

		/// <summary>
		/// Creates a new TargetItem instance using the specified XmlReader instance provided, and the node names of 
		/// the "target item list" nodes which are provided by the derived class during construction. 
		/// </summary>
		/// <param name="reader">The XmlReader instance positioned at the "target item list" node.</param>
		/// <param name="itemsNodeName">The name of the "target item list" node.</param>
		/// <param name="anyItemNodeName">The name of the AnyXxxx node for this "target item list" node.</param>
		/// <param name="itemNodeName">The name of the "target item" node that can be defined within this "target 
		/// item list" node.</param>
		/// <param name="schemaVersion">The version of the schema that was used to validate.</param>
		protected TargetItemsBase(XmlReader reader, string itemsNodeName, string anyItemNodeName, string itemNodeName, XacmlVersion schemaVersion)
			: base(schemaVersion)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (reader.LocalName == itemsNodeName && ValidateSchema(reader, schemaVersion))
			{
				while (reader.Read())
				{
					if (reader.LocalName == anyItemNodeName && ValidateSchema(reader, schemaVersion))
					{
						base.IsAny = true;
					}
					else if (reader.LocalName == itemNodeName && ValidateSchema(reader, schemaVersion))
					{
						base.ItemsList.Add((TargetItemBase)CreateTargetItem(reader));
					}
					else if (reader.LocalName == itemsNodeName &&
						reader.NodeType == XmlNodeType.EndElement)
					{
						break;
					}
				}
			}
			else
			{
				throw new Exception(string.Format(cor.Resource.exc_invalid_node_name, reader.LocalName));
			}
		}


		#endregion

		#region Public properties
		/*
		/// <summary>
		/// The list of "target item"s defined in the "target item list" node.
		/// </summary>
		public override ReadWriteTargetItemCollection ItemsList
		{
			get{ return new TargetItemCollection(base.ItemsList) ;}
		}
*/
		#endregion
	}
}
