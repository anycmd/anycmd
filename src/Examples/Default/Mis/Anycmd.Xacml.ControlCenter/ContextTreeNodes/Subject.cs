
using con = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.ControlCenter.ContextTreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Subject : TreeNodes.NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private con.SubjectElementReadWrite _subject;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="subject"></param>
		public Subject( con.SubjectElementReadWrite subject )
		{
			_subject = subject;

			this.Text = "Subject";
			this.SelectedImageIndex = 2;
			this.ImageIndex = 2;

			foreach( con.AttributeElementReadWrite attr in _subject.Attributes )
			{
				this.Nodes.Add( new Attribute( attr ) );
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public con.SubjectElementReadWrite SubjectDefinition
		{
			get{ return _subject; }
		}
	}
}