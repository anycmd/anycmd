using System;

namespace Anycmd.Xacml.Runtime.Functions
{
    using Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseOneAndOnly : FunctionBase, ITypeSpecificFunction
    {
        #region IFunction Members

        /// <summary>
        /// Gets the value as a generic object for the specified element.
        /// </summary>
        /// <param name="element">The element to get the value from.</param>
        /// <returns></returns>
        protected object GetTypedValue(IFunctionParameter element)
        {
            if (element == null) throw new ArgumentNullException("element");
            return element.GetTypedValue(DataType, 0);
        }

        /// <summary>
        /// Evaluates the function.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="args">The function arguments.</param>
        /// <returns>The result value of the function evaluation.</returns>
        public override EvaluationValue Evaluate(EvaluationContext context, params IFunctionParameter[] args)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (args == null) throw new ArgumentNullException("args");
            if (args[0].IsBag && args[0].BagSize == 1)
            {
                IFunctionParameter attrib = (IFunctionParameter)args[0].Elements[0];
                return new EvaluationValue(GetTypedValue(attrib), DataType);
            }
            else
            {
                return EvaluationValue.Indeterminate;
            }
        }

        /// <summary>
        /// The data type of the return value.
        /// </summary>
        public override IDataType Returns
        {
            get { return DataType; }
        }

        /// <summary>
        /// Defines the data types for the function arguments.
        /// </summary>
        public override IDataType[] Arguments
        {
            get
            {
                return new IDataType[] { DataTypeDescriptor.Bag };
            }
        }

        /// <summary>
        /// Defines the data type for which the function was defined for.
        /// </summary>
        public abstract IDataType DataType { get; }

        #endregion
    }
}
