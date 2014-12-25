using System;
using cor = Anycmd.Xacml;
using inf = Anycmd.Xacml.Interfaces;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// This class represents the values that can be handled by the evaluation engine.
    /// </summary>
    public class EvaluationValue : inf.IFunctionParameter
    {
        #region Static members

        /// <summary>
        /// Default evaluation value True.
        /// </summary>
        private static EvaluationValue _true = new EvaluationValue((object)true, DataTypeDescriptor.Boolean);

        /// <summary>
        /// Default evaluation value False.
        /// </summary>
        private static EvaluationValue _false = new EvaluationValue((object)false, DataTypeDescriptor.Boolean);

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
        private bool _isIndeterminate;

        /// <summary>
        /// The value contained.
        /// </summary>
        private object _value;

        /// <summary>
        /// The data type of the value.
        /// </summary>
        private inf.IDataType _dataType;

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
        public EvaluationValue(object value, inf.IDataType dataType)
        {
            if (dataType == null) throw new ArgumentNullException("dataType");
            if (value is EvaluationValue)
            {
                _value = ((EvaluationValue)value)._value;
                _dataType = ((EvaluationValue)value)._dataType;
            }
            else if (value is BagValue)
            {
                _value = value;
                _dataType = dataType;
            }
            else if (value is inf.IFunctionParameter)
            {
                _value = ((inf.IFunctionParameter)value).GetTypedValue(dataType, 0);
                _dataType = dataType;
            }
            else
            {
                _value = value;
                _dataType = dataType;
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
        public inf.IDataType GetType(rtm.EvaluationContext context)
        {
            return _dataType;
        }

        /// <summary>
        /// Gets the value as a generic object.
        /// </summary>
        /// <param name="dataType">The expected data type of the value.</param>
        /// <param name="parNo">THe number of parameter used only for error notification.</param>
        /// <returns></returns>
        public object GetTypedValue(inf.IDataType dataType, int parNo)
        {
            if (dataType != _dataType)
            {
                throw new EvaluationException(string.Format(cor.Resource.exc_type_mismatch, dataType, _dataType));
            }
            return _value;
        }

        /// <summary>
        /// A function can't be within an EvaluationValue so an exception will be thrown always.
        /// </summary>
        /// <param name="parNo">THe number of parameter used only for error notification.</param>
        /// <returns>None.</returns>
        public inf.IFunction GetFunction(int parNo)
        {
            throw new EvaluationException(string.Format(cor.Resource.exc_invalid_datatype_in_stringvalue, parNo, ""));
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
                if (_value != null)
                {
                    return "[" + _dataType + ":" + _value.ToString() + "]";
                }
                else
                {
                    return "null";
                }
            }
        }

        #endregion
    }
}
