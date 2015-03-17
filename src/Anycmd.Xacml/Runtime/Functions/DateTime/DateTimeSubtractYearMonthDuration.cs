using Anycmd.Xacml.Interfaces;
using System;
using typ = Anycmd.Xacml.Runtime.DataTypes;

// ReSharper disable once CheckNamespace
namespace Anycmd.Xacml.Runtime.Functions.DateTimeDataType
{
    /// <summary>
    /// Function implementation, in order to check the function behavior use the value of the Id
    /// property to search the function in the specification document.
    /// </summary>
    public class SubtractYearMonthDuration : FunctionBase
    {
        #region IFunction Members

        /// <summary>
        /// The id of the function, used only for notification.
        /// </summary>
        public override string Id
        {
            get { return Consts.Schema1.InternalFunctions.DateTimeSubtractYearMonthDuration; }
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
            DateTime baseDate = GetDateTimeArgument(args, 0);
            var yearMonthDuration = (typ.YearMonthDuration)args[1].GetTypedValue(DataTypeDescriptor.YearMonthDuration, 1);

            baseDate = baseDate.AddYears(yearMonthDuration.Years * -1);
            baseDate = baseDate.AddMonths(yearMonthDuration.Months * -1);

            return new EvaluationValue(baseDate, DataTypeDescriptor.DateTime);
        }

        /// <summary>
        /// The data type of the return value.
        /// </summary>
        public override IDataType Returns
        {
            get { return DataTypeDescriptor.DateTime; }
        }

        /// <summary>
        /// Defines the data types for the function arguments.
        /// </summary>
        public override IDataType[] Arguments
        {
            get
            {
                return new IDataType[] { DataTypeDescriptor.DateTime, DataTypeDescriptor.YearMonthDuration };
            }
        }
        #endregion
    }
}
