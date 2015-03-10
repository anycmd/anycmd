using Anycmd.Xacml.Policy.TargetItems;
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
        private TargetItemBaseReadWrite _targetItem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetItem"></param>
        public TargetItem(TargetItemBaseReadWrite targetItem)
        {
            _targetItem = targetItem;

            if (targetItem is ActionElementReadWrite)
            {
                this.Text = "Action";
                this.SelectedImageIndex = 2;
                this.ImageIndex = 2;
            }
            else if (targetItem is SubjectElementReadWrite)
            {
                this.Text = "Subject";
                this.SelectedImageIndex = 1;
                this.ImageIndex = 1;
            }
            else if (targetItem is ResourceElementReadWrite)
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
        public TargetItemBaseReadWrite TargetItemDefinition
        {
            get { return _targetItem; }
        }
    }
}
