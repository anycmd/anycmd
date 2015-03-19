using Anycmd.Xacml.Interfaces;
using Anycmd.Xacml.Policy;
using System;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// Represents the Condition element of the policy document during evaluation.
    /// </summary>
    public class Condition : ApplyBase, IEvaluable
    {
        #region Constructors

        /// <summary>
        /// Creates a new Condition using the reference to the condition definition in the policy document.
        /// </summary>
        /// <param name="condition">The condition definition of the policy document.</param>
        public Condition(ConditionElement condition)
            : base(condition)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method overrides the ApplyBase method in order to provide extra validations 
        /// required in the condition evaluation, for example the final return value should be a
        /// boolean value.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>The EvaluationValue with the results of the condition evaluation.</returns>
        public override EvaluationValue Evaluate(EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            EvaluationValue evaluationValue = null;
            context.Trace("Evaluating condition...");
            context.AddIndent();
            try
            {
                // Get the function instance
                IFunction function = EvaluationEngine.GetFunction(ApplyDefinition.FunctionId);
                if (function == null)
                {
                    context.Trace("ERR: function not found {0}", ApplyDefinition.FunctionId);
                    context.ProcessingError = true;
                    return EvaluationValue.Indeterminate;
                }

                // Validates the function return value
                if (function.Returns == null)
                {
                    context.Trace("The function '{0}' does not defines it's return value", ApplyDefinition.FunctionId);
                    evaluationValue = EvaluationValue.Indeterminate;
                    context.ProcessingError = true;
                }
                else if (function.Returns != DataTypeDescriptor.Boolean)
                {
                    context.Trace("Function does not return Boolean a value");
                    evaluationValue = EvaluationValue.Indeterminate;
                    context.ProcessingError = true;
                }
                else
                {
                    // Call the ApplyBase method to perform the evaluation.
                    evaluationValue = base.Evaluate(context);
                }

                // Validate the results of the evaluation
                if (evaluationValue.IsIndeterminate)
                {
                    context.Trace("condition evaluated into {0}", evaluationValue.ToString());
                    return evaluationValue;
                }
                if (!(evaluationValue.Value is bool))
                {
                    context.Trace("condition evaluated into {0}", evaluationValue.ToString());
                    return EvaluationValue.Indeterminate;
                }
                if (evaluationValue.BoolValue)
                {
                    context.Trace("condition evaluated into {0}", evaluationValue.ToString());
                    return EvaluationValue.True;
                }
                else
                {
                    // If the evaluation was false, validate if there was a missin attribute during 
                    // evaluation and return an Indeterminate, otherwise return the False value.
                    if (context.IsMissingAttribute)
                    {
                        context.Trace("condition evaluated into {0}", evaluationValue.ToString());
                        return EvaluationValue.Indeterminate;
                    }
                    else
                    {
                        context.Trace("condition evaluated into {0}", evaluationValue.ToString());
                        return EvaluationValue.False;
                    }
                }
            }
            finally
            {
                context.TraceContextValues();

                context.RemoveIndent();
                context.Trace("Condition: {0}", evaluationValue == null ? string.Empty : evaluationValue.ToString());
            }
        }

        #endregion
    }
}
