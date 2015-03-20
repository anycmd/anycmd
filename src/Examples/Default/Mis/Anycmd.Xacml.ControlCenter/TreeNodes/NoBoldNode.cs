using System;
using System.Drawing;
using System.Windows.Forms;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
    /// <summary>
    /// 
    /// </summary>
    public class NoBoldNode : TreeNode
    {
        private static readonly Font DefaultFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((Byte)(0)));

        /// <summary>
        /// 
        /// </summary>
        public NoBoldNode()
        {
            this.NodeFont = new Font(DefaultFont, FontStyle.Regular);
        }
    }
}
