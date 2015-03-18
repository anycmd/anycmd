using System;

namespace Anycmd.Xacml.Runtime.Functions
{
    using Interfaces;

    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseUnion : FunctionBase, ITypeSpecificFunction
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

            var retBag = new BagValue(DataType);
            IFunction functionDup = DataType.IsInFunction;
            foreach (object par1 in args[0].Elements)
            {
                EvaluationValue retVal = functionDup.Evaluate(context, new EvaluationValue(par1, args[0].GetType(context)), retBag);
                if (!retVal.BoolValue)
                {
                    retBag.Add(new EvaluationValue(par1, args[0].GetType(context)));
                }
            }
            foreach (object par2 in args[1].Elements)
            {
                EvaluationValue retVal = functionDup.Evaluate(context, new EvaluationValue(par2, args[1].GetType(context)), retBag);
                if (!retVal.BoolValue)
                {
                    retBag.Add(new EvaluationValue(par2, args[1].GetType(context)));
                }
            }
            return new EvaluationValue(retBag, DataType);
        }

        /// <summary>
        /// The return value data type.
        /// </summary>
        public override IDataType Returns
        {
            get { return DataTypeDescriptor.Bag; }
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
