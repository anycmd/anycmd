using System;
using System.Collections;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
	/// <summary>
	/// Defines a typed collection of read/write Rules.
	/// </summary>
	public class RuleReadWriteCollection : CollectionBase 
	{
		#region CollectionBase members

		/// <summary>
		/// Adds an object to the end of the CollectionBase.
		/// </summary>
		/// <param name="value">The Object to be added to the end of the CollectionBase. </param>
		/// <returns>The CollectionBase index at which the value has been added.</returns>
		public virtual int Add( RuleElementReadWrite value )  
		{
			return( List.Add( value ) );
		}

		/// <summary>
		/// Clears the collection
		/// </summary>
		public virtual new void Clear()
		{
			base.Clear();
		}
		/// <summary>
		/// Removes the specified element
		/// </summary>
		/// <param name="index">Position of the element</param>
		public virtual new void RemoveAt ( int index )
		{
			base.RemoveAt(index);
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Gets the index of the given RuleElementReadWrite in the collection
		/// </summary>
		/// <param name="rule"></param>
		/// <returns></returns>
		public int GetIndex( RuleElementReadWrite rule )
		{
			for( int i=0; i<this.Count; i++ )
			{
				if( this.List[i] == rule )
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Writes the XML of the current element
		/// </summary>
		/// <param name="writer">The XmlWriter in which the element will be written</param>
		public void WriteDocument(XmlWriter writer)
		{
			if (writer == null) throw new ArgumentNullException("writer");
			foreach(RuleElementReadWrite oRule in this)
			{
				writer.WriteStartElement(Consts.Schema1.RuleElement.Rule);
				writer.WriteAttributeString(Consts.Schema1.RuleElement.RuleId, oRule.Id);
				writer.WriteAttributeString(Consts.Schema1.RuleElement.Effect, oRule.Effect.ToString());
				if( oRule.Description.Length != 0 )
				{
					writer.WriteElementString(Consts.Schema1.RuleElement.Description, oRule.Description);
				}
				if(oRule.Target != null)
				{
					oRule.Target.WriteDocument(writer);
				}
				if(oRule.Condition != null)
				{
					oRule.Condition.WriteDocument(writer);
				}
				writer.WriteEndElement();
			}
		}

		#endregion
	}
}