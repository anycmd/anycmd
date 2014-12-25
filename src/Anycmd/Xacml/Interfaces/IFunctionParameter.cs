using System.Collections;

namespace Anycmd.Xacml.Interfaces
{
    using Runtime;

    /// <summary>
    /// Defines a value that can be used as a function parameter. Containing methods that allows getting a typed 
    /// value in order to evaluate the function with typed values.
    /// </summary>
    public interface IFunctionParameter
    {
        /// <summary>
        /// Gets the data type of the value.
        /// </summary>
        /// <param name="context">The evaluation context.</param>
        /// <returns>The data type descriptor.</returns>
        IDataType GetType(EvaluationContext context);

        /// <summary>
        /// Whether the function parameter is a bag of values.
        /// </summary>
        bool IsBag { get; }

        /// <summary>
        /// The size of the bag.
        /// </summary>
        int BagSize { get; }

        /// <summary>
        /// All the elements of the bag.
        /// </summary>
        ArrayList Elements { get; }

        /// <summary>
        /// Returns a function value from the function parameter.
        /// </summary>
        /// <param name="parNo"></param>
        /// <returns></returns>
        IFunction GetFunction(int parNo);

        /// <summary>
        /// Returns an object value from the function parameter. This method is used in fuctions that uses data 
        /// types not defined in the specification.
        /// </summary>
        /// <param name="dataType">The expected data type of the value.</param>
        /// <param name="parNo">The parameter number that represents this value.</param>
        object GetTypedValue(IDataType dataType, int parNo);
    }
}
