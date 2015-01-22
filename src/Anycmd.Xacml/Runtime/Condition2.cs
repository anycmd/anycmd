using System;
using inf = Anycmd.Xacml.Interfaces;
using pol = Anycmd.Xacml.Policy;

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
        public Condition2(pol.ConditionElement condition)
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
                foreach (inf.IExpression arg in ApplyDefinition.Arguments)
                {
                    if (arg is pol.ApplyElement)
                    {
                        context.Trace("Apply within condition.");

                        // There is a nested apply un this policy a new Apply will be created and also 
                        // evaluated. It's return value will be used as the processed argument.
                        Apply _childApply = new Apply((pol.ApplyElement)arg);

                        // Evaluate the Apply
                        EvaluationValue retVal = _childApply.Evaluate(context);

                        return retVal;
                    }
                    else if (arg is pol.FunctionElementReadWrite)
                    {
                        throw new NotImplementedException("FunctionElement"); //TODO:
                    }
                    else if (arg is pol.VariableReferenceElement)
                    {
                        pol.VariableReferenceElement variableRef = arg as pol.VariableReferenceElement;
                        VariableDefinition variableDef = context.CurrentPolicy.VariableDefinition[variableRef.VariableId] as VariableDefinition;

                        if (!variableDef.IsEvaluated)
                        {
                            return variableDef.Evaluate(context);
                        }
                        else
                        {
                            return variableDef.Value;
                        }
                    }
                    else if (arg is pol.AttributeValueElementReadWrite)
                    {
                        // The AttributeValue does not need to be processed
                        context.Trace("Attribute value {0}", arg.ToString());

                        pol.AttributeValueElement attributeValue = new pol.AttributeValueElement(((pol.AttributeValueElementReadWrite)arg).DataType, ((pol.AttributeValueElementReadWrite)arg).Contents, ((pol.AttributeValueElementReadWrite)arg).SchemaVersion);

                        return new EvaluationValue(
                            attributeValue.GetTypedValue(attributeValue.GetType(context), 0),
                            attributeValue.GetType(context));
                    }
                    else if (arg is pol.AttributeDesignatorBase)
                    {
                        // Returning an empty bag, since the condition is not supposed to work with a bag
                        context.Trace("Processing attribute designator: {0}", arg.ToString());

                        pol.AttributeDesignatorBase attrDes = (pol.AttributeDesignatorBase)arg;
                        BagValue bag = new BagValue(EvaluationEngine.GetDataType(attrDes.DataType));
                        return new EvaluationValue(bag, bag.GetType(context));
                    }
                    else if (arg is pol.AttributeSelectorElement)
                    {
                        // Returning an empty bag, since the condition is not supposed to work with a bag
                        context.Trace("Attribute selector");

                        pol.AttributeSelectorElement attrSel = (pol.AttributeSelectorElement)arg;
                        BagValue bag = new BagValue(EvaluationEngine.GetDataType(attrSel.DataType));
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
