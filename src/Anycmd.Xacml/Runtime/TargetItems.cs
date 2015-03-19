using Anycmd.Xacml.Interfaces;
using Anycmd.Xacml.Policy.TargetItems;
using System;
using ctx = Anycmd.Xacml.Context;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// The base class for any target item that will be used in runtime.
    /// </summary>
    public abstract class TargetItems
    {
        #region Private members

        /// <summary>
        /// The evaluation value that results the evaluation of the target item.
        /// </summary>
        private TargetEvaluationValue _evaluationValue;

        /// <summary>
        /// The target item reference to the policy document.
        /// </summary>
        private readonly TargetItemsBaseReadWrite _targetItems;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of any target item.
        /// </summary>
        protected TargetItems(TargetItemsBaseReadWrite targetItems)
        {
            _targetItems = targetItems;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Evaluates the target items and return wether the target applies to the context or not.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="targetItem">The target item in the context document.</param>
        /// <returns></returns>
        public virtual TargetEvaluationValue Evaluate(EvaluationContext context, ctx.TargetItemBase targetItem)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (_targetItems.IsAny)
            {
                context.Trace("IsAny");
                return TargetEvaluationValue.Match;
            }

            _evaluationValue = TargetEvaluationValue.NoMatch;

            //Match TargetItem
            foreach (TargetItemBase polItem in _targetItems.ItemsList)
            {
                foreach (TargetMatchBase match in polItem.Match)
                {
                    _evaluationValue = TargetEvaluationValue.NoMatch;

                    context.Trace("Using function: {0}", match.MatchId);

                    IFunction matchFunction = EvaluationEngine.GetFunction(match.MatchId);
                    if (matchFunction == null)
                    {
                        context.Trace("ERR: function not found {0}", match.MatchId);
                        context.ProcessingError = true;
                        return TargetEvaluationValue.Indeterminate;
                    }
                    else if (matchFunction.Returns == null)
                    {
                        // Validates the function return value
                        context.Trace("ERR: The function '{0}' does not defines it's return value", match.MatchId);
                        context.ProcessingError = true;
                        return TargetEvaluationValue.Indeterminate;
                    }
                    else if (matchFunction.Returns != DataTypeDescriptor.Boolean)
                    {
                        context.Trace("ERR: Function does not return Boolean a value");
                        context.ProcessingError = true;
                        return TargetEvaluationValue.Indeterminate;
                    }
                    else
                    {
                        Context.AttributeElement attribute = EvaluationEngine.Resolve(context, match, targetItem);

                        if (attribute != null)
                        {
                            context.Trace("Attribute found, evaluating match function");
                            try
                            {
                                EvaluationValue returnValue = EvaluationEngine.EvaluateFunction(context, matchFunction, match.AttributeValue, attribute);
                                _evaluationValue = returnValue.BoolValue ? TargetEvaluationValue.Match : TargetEvaluationValue.NoMatch;
                            }
                            catch (EvaluationException e)
                            {
                                context.Trace("ERR: {0}", e.Message);
                                _evaluationValue = TargetEvaluationValue.Indeterminate;
                            }
                        }

                        // Validate MustBePresent
                        if (match.AttributeReference.MustBePresent)
                        {
                            if (context.IsMissingAttribute)
                            {
                                context.Trace("Attribute not found and must be present");
                                _evaluationValue = TargetEvaluationValue.Indeterminate;
                            }
                        }

                        if (context.ProcessingError)
                        {
                            _evaluationValue = TargetEvaluationValue.Indeterminate;
                        }

                        // Do not iterate if the value was found
                        if (_evaluationValue != TargetEvaluationValue.Match)
                        {
                            break;
                        }
                    }
                }

                // Do not iterate if the value was found
                if (_evaluationValue == TargetEvaluationValue.Match)
                {
                    return _evaluationValue;
                }
            }

            return _evaluationValue;
        }

        #endregion
    }
}
