
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class TargetItem : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.TargetItemBaseReadWrite _targetItem;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetItem"></param>
		public TargetItem( pol.TargetItemBaseReadWrite targetItem )
		{
			_targetItem = targetItem;

			if( targetItem is pol.ActionElementReadWrite )
			{
				this.Text = "Action";
				this.SelectedImageIndex = 2;
				this.ImageIndex = 2;
			}
			else if( targetItem is pol.SubjectElementReadWrite )
			{
				this.Text = "Subject";
				this.SelectedImageIndex = 1;
				this.ImageIndex = 1;
			}
			else if( targetItem is pol.ResourceElementReadWrite )
			{
				this.Text = "Resource";
				this.SelectedImageIndex = 3;
				this.ImageIndex = 3;
			}

			this.Text += " (" + targetItem.Match.Count + " match/es)";
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.TargetItemBaseReadWrite TargetItemDefinition
		{
			get{ return _targetItem; }
		}
	}
}
