
namespace Anycmd.Xacml.Interfaces
{
    /// <summary>
    /// Defines a data type that can be used by the evaluation engine. All the data types must define a set of 
    /// functions that are used by the evaluation engine to support function creation and some internal 
    /// requirements.
    /// </summary>
    public interface IDataType
    {
        /// <summary>
        /// Returns the function that allows comparing two values of this data type which returns a boolean value.
        /// </summary>
        /// <value>The instance of the function.</value>
        IFunction EqualFunction { get; }

        /// <summary>
        /// Returns the function that allows determining if the first argument is contained in the
        /// bag of the second argument. This method is used to support the generic function BaseSubset, which allows 
        /// creating custom functions deriving from that class.
        /// </summary>
        /// <value>The instance of the function.</value>
        IFunction IsInFunction { get; }

        /// <summary>
        /// Returns the function that allows determining if all the elements of first argument are contained in the
        /// bag of the second argument. This method is used to support the generic function BaseSubset, which allows 
        /// creating custom functions deriving from that class.
        /// </summary>
        /// <value>The instance of the function.</value>
        IFunction SubsetFunction { get; }

        /// <summary>
        /// The string name of the data type.
        /// </summary>
        string DataTypeName { get; }

        /// <summary>
        /// Return an instance of the type using the specified string value.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <param name="parNo">The parameter number used only for error reporing.</param>
        object Parse(string value, int parNo);
    }
}
