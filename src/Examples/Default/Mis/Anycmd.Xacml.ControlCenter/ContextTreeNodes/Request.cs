
using con = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.ControlCenter.ContextTreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Request : TreeNodes.NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private con.RequestElementReadWrite _request;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		public Request( con.RequestElementReadWrite request )
		{
			_request = request;

			this.Text = "Request";
			this.SelectedImageIndex = 1;
			this.ImageIndex = 1;

			if( _request.Action != null )
			{
				this.Nodes.Add( new Action( _request.Action ) );
			}
			if( _request.Environment != null )
			{
				this.Nodes.Add( new Environment( _request.Environment ) ) ;
			}
			if( _request.Resources != null )
			{
				foreach( con.ResourceElementReadWrite resource in _request.Resources )
				{
					this.Nodes.Add( new Resource( resource ) );
				}
			}
			if( _request.Subjects != null )
			{
				foreach( con.SubjectElementReadWrite subject in _request.Subjects )
				{
					this.Nodes.Add( new Subject( subject ) );
				}
			}
			this.ExpandAll();
		}

		/// <summary>
		/// 
		/// </summary>
		public con.RequestElementReadWrite RequestDefinition
		{
			get{ return _request; }
		}
	}
}