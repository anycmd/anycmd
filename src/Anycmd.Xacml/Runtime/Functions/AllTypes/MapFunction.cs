using Anycmd.Xacml.Interfaces;
using System;

// ReSharper disable once CheckNamespace
namespace Anycmd.Xacml.Runtime.Functions
{
    /// <summary>
    /// Function implementation, in order to check the function behavior use the value of the Id
    /// property to search the function in the specification document.
    /// </summary>
    public class MapFunction : FunctionBase
    {
        #region IFunction Members

        /// <summary>
        /// The id of the function, used only for notification.
        /// </summary>
        public override string Id
        {
            get { return Consts.Schema1.InternalFunctions.Map; }
        }

        /// <summary>
        /// Method called by the EvaluationEngine to evaluate the function.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="args">The IFuctionParameters that will be used as arguments to the function.</param>
        /// <returns></returns>
        public override Anycmd.Xacml.Runtime.EvaluationValue Evaluate(EvaluationContext context, params IFunctionParameter[] args)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (args == null) throw new ArgumentNullException("args");
            IFunction function = args[0].GetFunction(0);
            if (!args[1].IsBag)
            {
                return EvaluationValue.False;
            }
            var retVal = new BagValue(args[1].GetType(context));
            foreach (object par in args[1].Elements)
            {
                retVal.Add(
                    function.Evaluate(
                        context,
                        new EvaluationValue(par, args[1].GetType(context))));
            }
            return new EvaluationValue(retVal, args[1].GetType(context));
        }

        /// <summary>
        /// The data type of the return value.
        /// </summary>
        public override Anycmd.Xacml.Interfaces.IDataType Returns
        {
            get { return null; }
        }

        /// <summary>
        /// Defines the data types for the function arguments.
        /// </summary>
        public override IDataType[] Arguments
        {
            get
            {
                return new IDataType[] { DataTypeDescriptor.Function, DataTypeDescriptor.Bag };
            }
        }

        #endregion
    }
}
