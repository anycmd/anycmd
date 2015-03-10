
using Anycmd.Xacml.Policy.TargetItems;
using pol = Anycmd.Xacml.Policy;

namespace Anycmd.Xacml.ControlCenter.TreeNodes
{
    /// <summary>
    /// 
    /// </summary>
    public class Target : NoBoldNode
    {
        /// <summary>
        /// 
        /// </summary>
        private pol.TargetElementReadWrite _target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public Target(pol.TargetElementReadWrite target)
        {
            _target = target;

            this.Text = "Target";
            this.SelectedImageIndex = 4;
            this.ImageIndex = 4;

            FillTargetItems(_target.Subjects);
            FillTargetItems(_target.Resources);
            FillTargetItems(_target.Actions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetItems"></param>
        protected void FillTargetItems(TargetItemsBaseReadWrite targetItems)
        {
            if (targetItems.IsAny)
            {
                if (targetItems is ActionsElementReadWrite)
                {
                    this.Nodes.Add(new AnyAction());
                }
                else if (targetItems is SubjectsElementReadWrite)
                {
                    this.Nodes.Add(new AnySubject());
                }
                else if (targetItems is ResourcesElementReadWrite)
                {
                    this.Nodes.Add(new AnyResource());
                }
            }
            else
            {
                foreach (TargetItemBaseReadWrite targetItem in targetItems.ItemsList)
                {
                    this.Nodes.Add(new TargetItem(targetItem));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public pol.TargetElementReadWrite TargetDefinition
        {
            get { return _target; }
        }
    }
}
