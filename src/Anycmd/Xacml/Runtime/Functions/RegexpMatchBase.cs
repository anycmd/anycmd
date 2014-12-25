using System;
using System.Text.RegularExpressions;

namespace Anycmd.Xacml.Runtime.Functions
{
    using Interfaces;

    /// <summary>
    /// Function implementation, in order to check the function behavior use the value of the Id
    /// property to search the function in the specification document.
    /// </summary>
    public abstract class RegexpMatchBase : FunctionBase
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
            string pattern = GetStringArgument(args, 0);
            string input = GetArgumentAsString(args, 1);

            Regex regEx = new Regex(pattern);
            Match m = regEx.Match(input);
            if (m.Success)
            {
                return EvaluationValue.True;
            }
            else
            {
                return EvaluationValue.False;
            }
        }

        /// <summary>
        /// Defines the data type for which the function was defined for.
        /// </summary>
        public abstract IDataType DataType { get; }

        /// <summary>
        /// Returns the specified indexed argument as a string value.
        /// </summary>
        /// <param name="args">The list of arguments.</param>
        /// <param name="idx">The argument index requested.</param>
        /// <returns>A string representing the value of the argument.</returns>
        public virtual string GetArgumentAsString(IFunctionParameter[] args, int idx)
        {
            if (args == null) throw new ArgumentNullException("args");
            return args[idx].ToString();
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
                return new IDataType[] { DataTypeDescriptor.String, this.DataType };
            }
        }

        #endregion
    }
}
