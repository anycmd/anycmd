
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class AttributeValue : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.AttributeValueElementReadWrite _attributeValue;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributeValue"></param>
		public AttributeValue( pol.AttributeValueElementReadWrite attributeValue )
		{
			_attributeValue = attributeValue;

			this.Text = "[" + attributeValue.DataType + "] " + attributeValue.Contents;
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.AttributeValueElementReadWrite AttributeValueDefinition
		{
			get{ return _attributeValue; }
		}
	}
}
