
using con = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.ControlCenter.ContextTreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Context : TreeNodes.NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private con.ContextDocumentReadWrite _context;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public Context( con.ContextDocumentReadWrite context )
		{
			_context = context;

			this.Text = "Context Document";
			this.SelectedImageIndex = 0;
			this.ImageIndex = 0;

			if( _context.Request != null )
			{
				this.Nodes.Add( new Request( _context.Request ) );
			}
			this.ExpandAll();
		}

		/// <summary>
		/// 
		/// </summary>
		public con.ContextDocumentReadWrite ContextDefinition
		{
			get{ return _context; }
		}
	}
}