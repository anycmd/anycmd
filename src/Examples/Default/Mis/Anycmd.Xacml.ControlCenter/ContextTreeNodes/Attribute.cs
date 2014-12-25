
using con = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.ControlCenter.ContextTreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Attribute : TreeNodes.NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private con.AttributeElementReadWrite _attribute;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="attribute"></param>
		public Attribute( con.AttributeElementReadWrite attribute )
		{
			_attribute = attribute;

			this.Text = "Attribute";
			this.SelectedImageIndex = 3;
			this.ImageIndex = 3;
		}

		/// <summary>
		/// 
		/// </summary>
		public con.AttributeElementReadWrite AttributeDefinition
		{
			get{ return _attribute; }
		}
	}
}