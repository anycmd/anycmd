using Anycmd.Xacml.Interfaces;
using System;

// ReSharper disable once CheckNamespace
namespace Anycmd.Xacml.Runtime.Functions
{
    /// <summary>
    /// Function implementation, in order to check the function behavior use the value of the Id
    /// property to search the function in the specification document.
    /// </summary>
    public class AllOfAll : FunctionBase
    {
        #region IFunction Members

        /// <summary>
        /// The id of the function, used only for notification.
        /// </summary>
        public override string Id
        {
            get { return Consts.Schema1.InternalFunctions.AllOfAll; }
        }

        /// <summary>
        /// Method called by the EvaluationEngine to evaluate the function.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="args">The IFuctionParameters that will be used as arguments to the function.</param>
        /// <returns></returns>
        public override EvaluationValue Evaluate(EvaluationContext context, params IFunctionParameter[] args)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (args == null) throw new ArgumentNullException("args");
            IFunction function = args[0].GetFunction(0);
            if (!Equals(function.Returns, DataTypeDescriptor.Boolean))
            {
                return EvaluationValue.Indeterminate;
            }

            foreach (var par1 in args[1].Elements)
            {
                foreach (var par2 in args[2].Elements)
                {
                    EvaluationValue retVal = function.Evaluate(
                        context,
                        new EvaluationValue(par1, args[1].GetType(context)), new EvaluationValue(par2, args[2].GetType(context)));
                    if (!retVal.BoolValue)
                    {
                        return EvaluationValue.False;
                    }
                }
            }
            return EvaluationValue.True;
        }

        /// <summary>
        /// Defines the data types for the function arguments.
        /// </summary>
        public override IDataType[] Arguments
        {
            get
            {
                return new IDataType[] { DataTypeDescriptor.Function, DataTypeDescriptor.Bag, DataTypeDescriptor.Bag };
            }
        }

        /// <summary>
        /// The data type of the return value.
        /// </summary>
        public override IDataType Returns
        {
            get { return DataTypeDescriptor.Boolean; }
        }

        #endregion
    }
}
