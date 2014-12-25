using System;

namespace Anycmd.Xacml.Runtime.Functions
{
    using Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseSubset : FunctionBase, ITypeSpecificFunction
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
            IFunction function = DataType.EqualFunction;
            foreach (object par1 in args[0].Elements)
            {
                bool hasFound = false;
                foreach (object par2 in args[1].Elements)
                {
                    EvaluationValue retVal = function.Evaluate(context, new EvaluationValue(par1, args[0].GetType(context)), new EvaluationValue(par2, args[1].GetType(context)));
                    if (retVal.BoolValue)
                    {
                        hasFound = true;
                        break;
                    }
                }
                if (!hasFound)
                {
                    return EvaluationValue.False;
                }

            }
            return EvaluationValue.True;
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
