using Anycmd.Xacml.Policy.TargetItems;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// Represents the Actions element of the policy document during evaluation. This instance 
    /// points to te policy "target item".
    /// </summary>
    public class ActionTargetItems : TargetItems
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the Actions class using the policy actions definition.
        /// </summary>
        /// <param name="actions">The policy actions definition.</param>
        public ActionTargetItems(ActionsElement actions)
            : base(actions)
        {
        }

        #endregion
    }
}
