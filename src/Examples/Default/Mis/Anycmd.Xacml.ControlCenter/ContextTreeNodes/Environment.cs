
using con = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.ControlCenter.ContextTreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Environment : TreeNodes.NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private con.EnvironmentElementReadWrite _environment;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="environment"></param>
		public Environment( con.EnvironmentElementReadWrite environment )
		{
			_environment = environment;

			this.Text = "Environment";
			this.SelectedImageIndex = 2;
			this.ImageIndex = 2;

			foreach( con.AttributeElementReadWrite attr in _environment.Attributes )
			{
				this.Nodes.Add( new Attribute( attr ) );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public con.EnvironmentElementReadWrite EnvironmentDefinition
		{
			get{ return _environment; }
		}
	}
}