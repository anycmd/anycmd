using Anycmd.Xacml.Policy;
using Anycmd.Xacml.Policy.TargetItems;
using System;
using ctx = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// Mantains the value resulted of the evaluation of the variable definition.
    /// </summary>
    public class VariableDefinition : IEvaluable
    {
        #region Private members

        /// <summary>
        /// References the variable definition element in the policy document.
        /// </summary>
        private readonly VariableDefinitionElement _variableDefinition;

        /// <summary>
        /// The value resulted from the evaluation.
        /// </summary>
        private EvaluationValue _value;

        /// <summary>
        /// Whether the variable have been evaluated.
        /// </summary>
        private bool _isEvaluated;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="variableDefinition">The variable definition in the policy document.</param>
        public VariableDefinition(VariableDefinitionElement variableDefinition)
        {
            _variableDefinition = variableDefinition;
        }

        #endregion

        #region IEvaluable Members

        /// <summary>
        /// Evaluates the variable into a value.
        /// </summary>
        /// <param name="context">The contex of the evaluation.</param>
        /// <returns>The value of the function.</returns>
        public EvaluationValue Evaluate(EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            context.Trace("Evaluating variable");
            context.AddIndent();

            try
            {
                if (_variableDefinition.Expression is ApplyElement)
                {
                    context.Trace("Apply within condition.");

                    // There is a nested apply un this policy a new Apply will be created and also 
                    // evaluated. It's return value will be used as the processed argument.
                    Apply childApply = new Apply((ApplyElement)_variableDefinition.Expression);

                    // Evaluate the Apply
                    _value = childApply.Evaluate(context);

                    context.TraceContextValues();
                    return _value;
                }
                else if (_variableDefinition.Expression is FunctionElement)
                {
                    throw new NotImplementedException("FunctionElement"); //TODO:
                }
                else if (_variableDefinition.Expression is VariableReferenceElement)
                {
                    var variableRef = _variableDefinition.Expression as VariableReferenceElement;
                    var variableDef = context.CurrentPolicy.VariableDefinition[variableRef.VariableId] as VariableDefinition;

                    context.TraceContextValues();

                    if (!variableDef.IsEvaluated)
                    {
                        return variableDef.Evaluate(context);
                    }
                    else
                    {
                        return variableDef.Value;
                    }
                }
                else if (_variableDefinition.Expression is AttributeValueElementReadWrite)
                {
                    // The AttributeValue does not need to be processed
                    context.Trace("Attribute value {0}", _variableDefinition.Expression.ToString());

                    var att = (AttributeValueElementReadWrite)_variableDefinition.Expression;
                    var attributeValue = new AttributeValueElement(att.DataType, att.Contents, att.SchemaVersion);

                    _value = new EvaluationValue(
                        attributeValue.GetTypedValue(attributeValue.GetType(context), 0),
                        attributeValue.GetType(context));
                    return _value;
                }
                else if (_variableDefinition.Expression is AttributeDesignatorBase)
                {
                    // Resolve the AttributeDesignator using the EvaluationEngine public methods.
                    context.Trace("Processing attribute designator: {0}", _variableDefinition.Expression.ToString());

                    var attrDes = (AttributeDesignatorBase)_variableDefinition.Expression;
                    BagValue bag = EvaluationEngine.Resolve(context, attrDes);

                    // If the attribute was not resolved by the EvaluationEngine search the 
                    // attribute in the context document, also using the EvaluationEngine public 
                    // methods.
                    if (bag.BagSize == 0)
                    {
                        if (_variableDefinition.Expression is SubjectAttributeDesignatorElement)
                        {
                            ctx.AttributeElement attrib = EvaluationEngine.GetAttribute(context, attrDes);
                            if (attrib != null)
                            {
                                context.Trace("Adding subject attribute designator: {0}", attrib.ToString());
                                bag.Add(attrib);
                            }
                        }
                        else if (_variableDefinition.Expression is ResourceAttributeDesignatorElement)
                        {
                            ctx.AttributeElement attrib = EvaluationEngine.GetAttribute(context, attrDes);
                            if (attrib != null)
                            {
                                context.Trace("Adding resource attribute designator {0}", attrib.ToString());
                                bag.Add(attrib);
                            }
                        }
                        else if (_variableDefinition.Expression is ActionAttributeDesignatorElement)
                        {
                            ctx.AttributeElement attrib = EvaluationEngine.GetAttribute(context, attrDes);
                            if (attrib != null)
                            {
                                context.Trace("Adding action attribute designator {0}", attrib.ToString());
                                bag.Add(attrib);
                            }
                        }
                        else if (_variableDefinition.Expression is EnvironmentAttributeDesignatorElement)
                        {
                            ctx.AttributeElement attrib = EvaluationEngine.GetAttribute(context, attrDes);
                            if (attrib != null)
                            {
                                context.Trace("Adding environment attribute designator {0}", attrib.ToString());
                                bag.Add(attrib);
                            }
                        }
                    }

                    // If the argument was not found and the attribute must be present this is 
                    // a MissingAttribute situation so set the flag. Otherwise add the attribute 
                    // to the processed arguments.
                    if (bag.BagSize == 0 && attrDes.MustBePresent)
                    {
                        context.Trace("Attribute is missing");
                        context.IsMissingAttribute = true;
                        context.AddMissingAttribute(attrDes);
                        _value = EvaluationValue.Indeterminate;
                    }
                    else
                    {
                        _value = new EvaluationValue(bag, bag.GetType(context));
                    }
                    return _value;
                }
                else if (_variableDefinition.Expression is AttributeSelectorElement)
                {
                    // Resolve the XPath query using the EvaluationEngine public methods.
                    context.Trace("Attribute selector");
                    try
                    {
                        var attributeSelector = (AttributeSelectorElement)_variableDefinition.Expression;
                        BagValue bag = EvaluationEngine.Resolve(context, attributeSelector);
                        if (bag.Elements.Count == 0 && attributeSelector.MustBePresent)
                        {
                            context.Trace("Attribute is missing");
                            context.IsMissingAttribute = true;
                            context.AddMissingAttribute(attributeSelector);
                            _value = EvaluationValue.Indeterminate;
                        }
                        else
                        {
                            _value = new EvaluationValue(bag, bag.GetType(context));
                        }
                    }
                    catch (EvaluationException e)
                    {
                        context.Trace("ERR: {0}", e.Message);
                        context.ProcessingError = true;
                        _value = EvaluationValue.Indeterminate;
                    }
                    return _value;
                }
                throw new NotSupportedException("internal error");
            }
            finally
            {
                _isEvaluated = true;
                context.RemoveIndent();
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The value of the variable.
        /// </summary>
        public EvaluationValue Value
        {
            get
            {
                if (!_isEvaluated)
                {
                    throw new EvaluationException("Te variable must be evaluated.");
                }
                return _value;
            }
        }

        /// <summary>
        /// Whether the variable have been evaluated.
        /// </summary>
        public bool IsEvaluated
        {
            get { return _isEvaluated; }
        }

        #endregion
    }
}
