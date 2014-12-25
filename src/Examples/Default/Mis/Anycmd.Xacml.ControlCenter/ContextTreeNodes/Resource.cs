
using con = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.ControlCenter.ContextTreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Resource : TreeNodes.NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private con.ResourceElementReadWrite _resource;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resource"></param>
		public Resource( con.ResourceElementReadWrite resource )
		{
			_resource = resource;

			this.Text = "Resource";
			this.SelectedImageIndex = 2;
			this.ImageIndex = 2;

			foreach( con.AttributeElementReadWrite attr in _resource.Attributes )
			{
				this.Nodes.Add( new Attribute( attr ) );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public con.ResourceElementReadWrite ResourceDefinition
		{
			get{ return _resource; }
		}
	}
}