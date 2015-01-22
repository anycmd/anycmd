
namespace Anycmd.Xacml.Runtime
{
    using Xacml.Policy;

    /// <summary>
    /// Represents the Apply element of the policy document during evaluation.
    /// </summary>
    public class Apply : ApplyBase
    {
        #region Constructor

        /// <summary>
        /// Creates a new Apply using the reference to the apply definition in the policy document.
        /// </summary>
        /// <param name="apply">The apply definition of the policy document.</param>
        public Apply(ApplyElement apply)
            : base(apply)
        {
        }

        #endregion
    }
}