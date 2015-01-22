
namespace Anycmd.Xacml.Interfaces
{
    using Runtime;

    /// <summary>
    /// Defines a function that can be executed by the EvaluationEngine.
    /// </summary>
    public interface IFunction : IFunctionParameter
    {
        /// <summary>
        /// The id of the function, used only for notification.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Method called by the EvaluationEngine to evaluate the function.
        /// </summary>
        /// <param name="context">The Evaluation context information.</param>
        /// <param name="args">The IFuctionParameters that will be used as arguments to the function.</param>
        /// <returns></returns>
        EvaluationValue Evaluate(EvaluationContext context, params IFunctionParameter[] args);

        /// <summary>
        /// Defines the data type returned by the function.
        /// </summary>
        IDataType Returns { get; }

        /// <summary>
        /// Defines the data types for the function arguments.
        /// </summary>
        IDataType[] Arguments { get; }

        /// <summary>
        /// Whether the function defines variable arguments. The data type of the variable arguments will be the 
        /// data type of the last parameter.
        /// </summary>
        bool VarArgs { get; }
    }
}
