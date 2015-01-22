
namespace Anycmd.Xacml.Interfaces
{
    using Runtime;

    /// <summary>
    /// This interface allows extension of the policy combining algorithms and is also 
    /// responsible for as a factory of the derived classes.
    /// </summary>
    public interface IPolicyCombiningAlgorithm
    {
        /// <summary>
        /// Method that should be implemented by the derived classe where the evaluation 
        /// logic is implemented.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="policies">The policies that will be evaulated.</param>
        /// <returns>The final decission if the evaluation.</returns>
        Decision Evaluate(EvaluationContext context, IMatchEvaluableCollection policies);
    }
}
