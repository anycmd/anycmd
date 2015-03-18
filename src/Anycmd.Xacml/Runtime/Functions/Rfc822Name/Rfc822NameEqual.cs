using Anycmd.Xacml.Interfaces;
using System;

// ReSharper disable once CheckNamespace
namespace Anycmd.Xacml.Runtime.Functions
{
    /// <summary>
    /// Function implementation, in order to check the function behavior use the value of the Id
    /// property to search the function in the specification document.
    /// </summary>
    public class Rfc822NameEqual : BaseEqual
    {
        #region IFunction Members

        /// <summary>
        /// The id of the function, used only for notification.
        /// </summary>
        public override string Id
        {
            get { return Consts.Schema1.InternalFunctions.Rfc822NameEqual; }
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
            if (GetRfc822NameArgument(args, 0).Equals(GetRfc822NameArgument(args, 1)))
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
        public override IDataType DataType
        {
            get { return DataTypeDescriptor.Rfc822Name; }
        }

        #endregion
    }
}
