
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class AttributeDesignator : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.AttributeDesignatorBase _attributeDesignator;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attributeDesignator"></param>
		public AttributeDesignator( pol.AttributeDesignatorBase attributeDesignator )
		{
			_attributeDesignator = attributeDesignator;

			this.Text = "[" + attributeDesignator.DataType + "]:" + attributeDesignator.AttributeId;
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.AttributeDesignatorBase AttributeDesignatorDefinition
		{
			get{ return _attributeDesignator; }
		}
	}
}
