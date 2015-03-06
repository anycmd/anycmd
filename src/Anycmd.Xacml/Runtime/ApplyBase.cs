using System;
using ctx = Anycmd.Xacml.Context;
using inf = Anycmd.Xacml.Interfaces;
using pol = Anycmd.Xacml.Policy;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// This abstract base class is used to mantain the Condition and Apply evaluation behavior
    /// without affecting the state of the instance created when the policy document was readed.
    /// </summary>
    public abstract class ApplyBase : IEvaluable
    {
        #region Private members

        /// <summary>
        /// A reference to the ApplyBase defined in the policy document.
        /// </summary>
        private pol.ApplyBaseReadWrite _applyBase;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ApplyBase using the ApplyBase form the policy document.
        /// </summary>
        /// <param name="apply"></param>
        protected ApplyBase(pol.ApplyBaseReadWrite apply)
        {
            _applyBase = apply;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The apply base definition in the policy document.
        /// </summary>
        public pol.ApplyBaseReadWrite ApplyDefinition
        {
            get { return _applyBase; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Main evaluation method which is divided in two phases: Argument processing and 
        /// Function evaluation.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>The result of the evaluation in an EvaluationValue instance.</returns>
        public virtual EvaluationValue Evaluate(EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            // Validate the function exists
            context.Trace("Calling function: {0}", _applyBase.FunctionId);
            inf.IFunction function = rtm.EvaluationEngine.GetFunction(_applyBase.FunctionId);
            if (function == null)
            {
                context.Trace("ERR: function not found {0}", _applyBase.FunctionId);
                context.ProcessingError = true;
                return EvaluationValue.Indeterminate;
            }

            // Process the arguments.
            IFunctionParameterCollection arguments = ProcessArguments(context, new pol.ExpressionCollection((pol.ExpressionReadWriteCollection)_applyBase.Arguments));

            // Call the function with the arguments processed.
            EvaluationValue returnValue = rtm.EvaluationEngine.EvaluateFunction(context, function, arguments.ToArray());
            return returnValue;
        }

        #endregion

        #region Private members

        /// <summary>
        /// THe argument processing method resolves all the attribute designators, attribute 
        /// selectors. If there is an internal Apply it will be evaulated within this method.
        /// </summary>
        /// <remarks>All the processed arguments are converted into an IFunctionParameter instance
        /// because this is the only value that can be used to call a function.</remarks>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="arguments">The arguments to process.</param>
        /// <returns>A list of arguments ready to be used by a function.</returns>
        private IFunctionParameterCollection ProcessArguments(EvaluationContext context, pol.ExpressionCollection arguments)
        {
            context.Trace("Processing arguments");
            context.AddIndent();

            // Create a list to return the processed values.
            IFunctionParameterCollection processedArguments = new IFunctionParameterCollection();

            // Iterate through the arguments, the IExpressionType is a mark interface
            foreach (inf.IExpression arg in arguments)
            {
                if (arg is pol.ApplyElement)
                {
                    context.Trace("Nested apply");

                    // There is a nested apply un this policy a new Apply will be created and also 
                    // evaluated. It's return value will be used as the processed argument.
                    Apply _childApply = new Apply((pol.ApplyElement)arg);

                    // Evaluate the Apply
                    EvaluationValue retVal = _childApply.Evaluate(context);

                    context.TraceContextValues();

                    // If the results were Indeterminate the Indeterminate value will be placed in 
                    // the processed arguments, later another method will validate the parameters
                    // and cancel the evaluation propperly.
                    if (!retVal.IsIndeterminate)
                    {
                        if (!context.IsMissingAttribute)
                        {
                            processedArguments.Add(retVal);
                        }
                    }
                    else
                    {
                        processedArguments.Add(retVal);
                    }
                }
                else if (arg is pol.FunctionElementReadWrite)
                {
                    // Search for the function and place it in the processed arguments.
                    pol.FunctionElement functionId = new pol.FunctionElement(((pol.FunctionElementReadWrite)arg).FunctionId, ((pol.FunctionElementReadWrite)arg).SchemaVersion);

                    context.Trace("Function {0}", functionId.FunctionId);
                    inf.IFunction function = EvaluationEngine.GetFunction(functionId.FunctionId);
                    if (function == null)
                    {
                        context.Trace("ERR: function not found {0}", _applyBase.FunctionId);
                        context.ProcessingError = true;
                        processedArguments.Add(EvaluationValue.Indeterminate);
                    }
                    else
                    {
                        processedArguments.Add(function);
                    }
                }
                else if (arg is pol.VariableReferenceElement)
                {
                    pol.VariableReferenceElement variableRef = arg as pol.VariableReferenceElement;
                    VariableDefinition variableDef = context.CurrentPolicy.VariableDefinition[variableRef.VariableId] as VariableDefinition;

                    context.TraceContextValues();

                    if (!variableDef.IsEvaluated)
                    {
                        processedArguments.Add(variableDef.Evaluate(context));
                    }
                    else
                    {
                        processedArguments.Add(variableDef.Value);
                    }
                }
                else if (arg is pol.AttributeValueElementReadWrite)
                {
                    // The AttributeValue does not need to be processed
                    context.Trace("Attribute value {0}", arg.ToString());
                    processedArguments.Add(new pol.AttributeValueElement(((pol.AttributeValueElementReadWrite)arg).DataType, ((pol.AttributeValueElementReadWrite)arg).Contents, ((pol.AttributeValueElementReadWrite)arg).SchemaVersion));
                }
                else if (arg is pol.AttributeDesignatorBase)
                {
                    // Resolve the AttributeDesignator using the EvaluationEngine public methods.
                    context.Trace("Processing attribute designator: {0}", arg.ToString());

                    pol.AttributeDesignatorBase attrDes = (pol.AttributeDesignatorBase)arg;
                    BagValue bag = EvaluationEngine.Resolve(context, attrDes);

                    // If the attribute was not resolved by the EvaluationEngine search the 
                    // attribute in the context document, also using the EvaluationEngine public 
                    // methods.
                    if (bag.BagSize == 0)
                    {
                        if (arg is pol.SubjectAttributeDesignatorElement)
                        {
                            ctx.AttributeElement attrib = EvaluationEngine.GetAttribute(context, attrDes);
                            if (attrib != null)
                            {
                                context.Trace("Adding subject attribute designator: {0}", attrib.ToString());
                                bag.Add(attrib);
                                break;
                            }
                        }
                        else if (arg is pol.ResourceAttributeDesignatorElement)
                        {
                            ctx.AttributeElement attrib = EvaluationEngine.GetAttribute(context, attrDes);
                            if (attrib != null)
                            {
                                context.Trace("Adding resource attribute designator {0}", attrib.ToString());
                                bag.Add(attrib);
                            }
                        }
                        else if (arg is pol.ActionAttributeDesignatorElement)
                        {
                            ctx.AttributeElement attrib = rtm.EvaluationEngine.GetAttribute(context, attrDes);
                            if (attrib != null)
                            {
                                context.Trace("Adding action attribute designator {0}", attrib.ToString());
                                bag.Add(attrib);
                            }
                        }
                        else if (arg is pol.EnvironmentAttributeDesignatorElement)
                        {
                            ctx.AttributeElement attrib = rtm.EvaluationEngine.GetAttribute(context, attrDes);
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
                    }
                    else
                    {
                        processedArguments.Add(bag);
                    }
                }
                else if (arg is pol.AttributeSelectorElement)
                {
                    // Resolve the XPath query using the EvaluationEngine public methods.
                    context.Trace("Attribute selector");
                    try
                    {
                        BagValue bag = EvaluationEngine.Resolve(context, (pol.AttributeSelectorElement)arg);
                        if (bag.Elements.Count == 0 && ((pol.AttributeSelectorElement)arg).MustBePresent)
                        {
                            context.Trace("Attribute is missing");
                            context.IsMissingAttribute = true;
                            context.AddMissingAttribute((pol.AttributeReferenceBase)arg);
                        }
                        else
                        {
                            processedArguments.Add(bag);
                        }
                    }
                    catch (EvaluationException e)
                    {
                        context.Trace("ERR: {0}", e.Message);
                        processedArguments.Add(EvaluationValue.Indeterminate);
                        context.ProcessingError = true;
                    }
                }
            }
            context.RemoveIndent();

            return processedArguments;
        }
        #endregion
    }
}
