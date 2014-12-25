
using con = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.ControlCenter.ContextTreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Action : TreeNodes.NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private con.ActionElementReadWrite _action;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		public Action( con.ActionElementReadWrite action )
		{
			_action = action;

			this.Text = "Action";
			this.SelectedImageIndex = 2;
			this.ImageIndex = 2;

			foreach( con.AttributeElementReadWrite attr in _action.Attributes )
			{
				this.Nodes.Add( new Attribute( attr ) );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public con.ActionElementReadWrite ActionDefinition
		{
			get{ return _action; }
		}
	}
}