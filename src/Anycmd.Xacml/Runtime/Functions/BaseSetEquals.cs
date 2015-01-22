using System;

namespace Anycmd.Xacml.Runtime.Functions
{
    using Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseSetEquals : FunctionBase, ITypeSpecificFunction
    {
        #region IFunction Members

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
            IFunction function = DataType.SubsetFunction;
            if ((function.Evaluate(
                context,
                new EvaluationValue(args[0], args[0].GetType(context)), new EvaluationValue(args[1], args[1].GetType(context))) == EvaluationValue.True) &&
                (function.Evaluate(
                context,
                new EvaluationValue(args[1], args[1].GetType(context)), new EvaluationValue(args[0], args[0].GetType(context))) == EvaluationValue.True))
            {
                return EvaluationValue.True;
            }
            else
            {
                return EvaluationValue.False;
            }
        }

        /// <summary>
        /// The data type of the return value.
        /// </summary>
        public override IDataType Returns
        {
            get { return DataTypeDescriptor.Boolean; }
        }

        /// <summary>
        /// Defines the data types for the function arguments.
        /// </summary>
        public override IDataType[] Arguments
        {
            get
            {
                return new IDataType[] { DataTypeDescriptor.Bag, DataTypeDescriptor.Bag };
            }
        }

        /// <summary>
        /// Defines the data type for which the function was defined for.
        /// </summary>
        public abstract IDataType DataType { get; }

        #endregion
    }
}
