using System.Drawing;
using System.Windows.Forms;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class NoBoldNode : TreeNode
	{
		/// <summary>
		/// 
		/// </summary>
		public NoBoldNode()
		{
			this.NodeFont = new Font( MainForm.DEFAULT_FONT, FontStyle.Regular );
		}
	}
}
