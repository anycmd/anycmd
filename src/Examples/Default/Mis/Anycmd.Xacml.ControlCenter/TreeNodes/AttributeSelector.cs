
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class AttributeSelector : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.AttributeSelectorElement _attributeSelector;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributeSelector"></param>
		public AttributeSelector( pol.AttributeSelectorElement attributeSelector )
		{
			_attributeSelector = attributeSelector;

			this.Text = "XPath: " + attributeSelector.RequestContextPath;
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.AttributeSelectorElement AttributeSelectorDefinition
		{
			get{ return _attributeSelector; }
		}
	}
}
