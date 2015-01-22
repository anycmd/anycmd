using System;

namespace Anycmd.Xacml.Runtime.Functions
{
    using Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseBag : FunctionBase, ITypeSpecificFunction
    {
        #region IFunction Members

        /// <summary>
        /// Defines the data type for which the function was defined for.
        /// </summary>
        public abstract IDataType DataType { get; }

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
            IDataType type = DataType;
            BagValue bag = new BagValue(type);
            for (int i = 0; i < args.Length; i++)
            {
                bag.Add(args[i].GetTypedValue(type, i));
            }
            return new EvaluationValue(bag, type);
        }

        /// <summary>
        /// The data type of the return value.
        /// </summary>
        public override IDataType Returns
        {
            get { return DataTypeDescriptor.Bag; }
        }

        /// <summary>
        /// Whether the function defines variable arguments. The data type of the variable arguments will be the 
        /// data type of the last parameter.
        /// </summary>
        public override bool VarArgs { get { return true; } }

        /// <summary>
        /// Defines the data types for the function arguments.
        /// </summary>
        public override IDataType[] Arguments
        {
            get
            {
                return new IDataType[] { DataType };
            }
        }

        #endregion
    }
}
