using Anycmd.Xacml.Interfaces;
using Anycmd.Xacml.Policy;
using System;
using System.Diagnostics;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// Represents the Condition element of the policy document during evaluation.
    /// </summary>
    public class Condition2 : Condition, IEvaluable
    {
        #region Constructors

        /// <summary>
        /// Creates a new Condition using the reference to the condition definition in the policy document.
        /// </summary>
        /// <param name="condition">The condition definition of the policy document.</param>
        public Condition2(ConditionElement condition)
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
            context.Trace("Evaluating condition");
            context.AddIndent();
            try
            {
                // Iterate through the arguments, the IExpressionType is a mark interface
                foreach (IExpression arg in ApplyDefinition.Arguments)
                {
                    if (arg is ApplyElement)
                    {
                        context.Trace("Apply within condition.");

                        // There is a nested apply un this policy a new Apply will be created and also 
                        // evaluated. It's return value will be used as the processed argument.
                        var childApply = new Apply((ApplyElement)arg);

                        // Evaluate the Apply
                        EvaluationValue retVal = childApply.Evaluate(context);

                        return retVal;
                    }
                    else if (arg is FunctionElementReadWrite)
                    {
                        throw new NotImplementedException("FunctionElement"); //TODO:
                    }
                    else if (arg is VariableReferenceElement)
                    {
                        var variableRef = arg as VariableReferenceElement;
                        var variableDef = context.CurrentPolicy.VariableDefinition[variableRef.VariableId] as VariableDefinition;

                        Debug.Assert(variableDef != null, "variableDef != null");
                        if (!variableDef.IsEvaluated)
                        {
                            return variableDef.Evaluate(context);
                        }
                        else
                        {
                            return variableDef.Value;
                        }
                    }
                    else if (arg is AttributeValueElementReadWrite)
                    {
                        // The AttributeValue does not need to be processed
                        context.Trace("Attribute value {0}", arg.ToString());

                        var attributeValue = new AttributeValueElement(((AttributeValueElementReadWrite)arg).DataType, ((AttributeValueElementReadWrite)arg).Contents, ((AttributeValueElementReadWrite)arg).SchemaVersion);

                        return new EvaluationValue(
                            attributeValue.GetTypedValue(attributeValue.GetType(context), 0),
                            attributeValue.GetType(context));
                    }
                    else if (arg is AttributeDesignatorBase)
                    {
                        // Returning an empty bag, since the condition is not supposed to work with a bag
                        context.Trace("Processing attribute designator: {0}", arg.ToString());

                        var attrDes = (AttributeDesignatorBase)arg;
                        var bag = new BagValue(EvaluationEngine.GetDataType(attrDes.DataType));
                        return new EvaluationValue(bag, bag.GetType(context));
                    }
                    else if (arg is AttributeSelectorElement)
                    {
                        // Returning an empty bag, since the condition is not supposed to work with a bag
                        context.Trace("Attribute selector");

                        var attrSel = (AttributeSelectorElement)arg;
                        var bag = new BagValue(EvaluationEngine.GetDataType(attrSel.DataType));
                        return new EvaluationValue(bag, bag.GetType(context));
                    }
                }
                throw new NotSupportedException("internal error");
            }
            finally
            {
                context.TraceContextValues();
                context.RemoveIndent();
            }
        }

        #endregion
    }
}
