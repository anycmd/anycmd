using Anycmd.Xacml.Interfaces;
using System;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// This class represents the values that can be handled by the evaluation engine.
    /// </summary>
    public class EvaluationValue : IFunctionParameter
    {
        #region Static members

        /// <summary>
        /// Default evaluation value True.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly EvaluationValue _true = new EvaluationValue(true, DataTypeDescriptor.Boolean);

        /// <summary>
        /// Default evaluation value False.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly EvaluationValue _false = new EvaluationValue(false, DataTypeDescriptor.Boolean);

        /// <summary>
        /// Default evaluation value True.
        /// </summary>
        public static EvaluationValue True
        {
            get { return _true; }
        }

        /// <summary>
        /// Default evaluation value Fale.
        /// </summary>
        public static EvaluationValue False
        {
            get { return _false; }
        }

        /// <summary>
        /// Default evaluation value Indeterminate.
        /// </summary>
        public static EvaluationValue Indeterminate
        {
            get { return new EvaluationValue(true); }
        }

        #endregion

        #region Private members

        /// <summary>
        /// Whether the value is indeterminate.
        /// </summary>
        private readonly bool _isIndeterminate;

        /// <summary>
        /// The value contained.
        /// </summary>
        private readonly object _value;

        /// <summary>
        /// The data type of the value.
        /// </summary>
        private readonly IDataType _dataType;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new empty EvaluationValue or Indeterminate.
        /// </summary>
        /// <param name="isIndeterminate">Whether the EvaluationValue will be Indeterminate or not.</param>
        private EvaluationValue(bool isIndeterminate)
        {
            _isIndeterminate = isIndeterminate;
        }

        /// <summary>
        /// Creates a new evaluation value specifying the data type.
        /// </summary>
        /// <param name="value">The value to hold in the evaluation value.</param>
        /// <param name="dataType">The data type for the value.</param>
        public EvaluationValue(object value, IDataType dataType)
        {
            if (dataType == null) throw new ArgumentNullException("dataType");
            var evaluationValue = value as EvaluationValue;
            if (evaluationValue != null)
            {
                _value = evaluationValue._value;
                _dataType = evaluationValue._dataType;
            }
            else if (value is BagValue)
            {
                _value = value;
                _dataType = dataType;
            }
            else
            {
                var parameter = value as IFunctionParameter;
                if (parameter != null)
                {
                    _value = parameter.GetTypedValue(dataType, 0);
                    _dataType = dataType;
                }
                else
                {
                    _value = value;
                    _dataType = dataType;
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The value contained.
        /// </summary>
        public object Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Whether the value is Indeterminate.
        /// </summary>
        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
        }

        /// <summary>
        /// The value as a boolean value.
        /// </summary>
        public bool BoolValue
        {
            get
            {
                if (!(_value is bool) || _isIndeterminate)
                {
                    throw new EvaluationException("invalid data type.");
                }
                return (bool)_value;
            }
        }

        #endregion

        #region IFunctionParameter Members

        /// <summary>
        /// Gets the data type of the value.
        /// </summary>
        /// <param name="context">The evaluation context.</param>
        /// <returns>The data type descriptor.</returns>
        public IDataType GetType(EvaluationContext context)
        {
            return _dataType;
        }

        /// <summary>
        /// Gets the value as a generic object.
        /// </summary>
        /// <param name="dataType">The expected data type of the value.</param>
        /// <param name="parNo">THe number of parameter used only for error notification.</param>
        /// <returns></returns>
        public object GetTypedValue(IDataType dataType, int parNo)
        {
            if (!ReferenceEquals(_dataType, dataType))
            {
                throw new EvaluationException(string.Format(Properties.Resource.exc_type_mismatch, dataType, _dataType));
            }
            return _value;
        }

        /// <summary>
        /// A function can't be within an EvaluationValue so an exception will be thrown always.
        /// </summary>
        /// <param name="parNo">THe number of parameter used only for error notification.</param>
        /// <returns>None.</returns>
        public IFunction GetFunction(int parNo)
        {
            throw new EvaluationException(string.Format(Properties.Resource.exc_invalid_datatype_in_stringvalue, parNo, ""));
        }

        /// <summary>
        /// Whether the value is a bag.
        /// </summary>
        public bool IsBag
        {
            get { return (_value is BagValue); } //TODO: an exception if the element is not a bag
        }

        /// <summary>
        /// If the value is a bag the size will be returned otherwise an exception is thrown.
        /// </summary>
        public int BagSize
        {
            get { return ((BagValue)_value).BagSize; } //TODO: an exception if the element is not a bag
        }

        /// <summary>
        /// The elements of the bag value.
        /// </summary>
        public System.Collections.ArrayList Elements
        {
            get { return ((BagValue)_value).Elements; } //TODO: an exception if the element is not a bag
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        /// <returns>A string representation of the value.</returns>
        public override string ToString()
        {
            if (IsIndeterminate)
            {
                return Decision.Indeterminate.ToString();
            }
            else
            {
                return _value != null ? string.Format("[{0}:{1}]", _dataType, _value) : "null";
            }
        }

        #endregion
    }
}
