using System;
using System.Collections;

namespace Anycmd.Xacml.Policy
{
    using Interfaces;
    using Runtime;

    /// <summary>
    /// An abstract base class used in the PolicyDocument and the ContextDocument to define a 
    /// constant value defined in the documents that are represented as strings and requires a 
    /// conversion in order to perform evaluations with this value.
    /// </summary>
    public abstract class StringValueBase :
        XacmlElement, IFunctionParameter
    {
        #region Constructors

        /// <summary>
        /// A protected default constructor.
        /// </summary>
        /// <param name="schemaVersion">The schema used to validate the document.</param>
        /// <param name="schema">The schema that defines this element.</param>
        protected StringValueBase(XacmlSchema schema, XacmlVersion schemaVersion)
            : base(schema, schemaVersion)
        {
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// The typed value of this string value.
        /// </summary>
        public abstract string Value { get; set; }

        /// <summary>
        /// The data type for this string value.
        /// </summary>
        public abstract string DataTypeValue { get; set; }

        #endregion

        #region IFunctionParameter Members

        /// <summary>
        /// Gets the data type of the value.
        /// </summary>
        /// <param name="context">The evaluation context.</param>
        /// <returns>The data type descriptor.</returns>
        public IDataType GetType(EvaluationContext context)
        {
            return EvaluationEngine.GetDataType(DataTypeValue);
        }

        /// <summary>
        /// Returns the typed value for this string value.
        /// </summary>
        /// <param name="dataType">The expected data type of the value.</param>
        /// <param name="parNo">The parameter number used only for error reporing.</param>
        /// <returns>The typed value as an object.</returns>
        public object GetTypedValue(IDataType dataType, int parNo)
        {
            if (dataType == null) throw new ArgumentNullException("dataType");
            if (IsBag)
            {
                return this;
            }
            else
            {
                if (DataTypeValue != dataType.DataTypeName)
                {
                    throw new EvaluationException("invalid data type"); //TODO: make the error more clear.
                }

                if (DataTypeValue != null)
                {
                    if (DataTypeValue != dataType.DataTypeName)
                    {
                        throw new EvaluationException(string.Format(Resource.exc_invalid_datatype_in_stringvalue, parNo, DataTypeValue));
                    }
                }

                return dataType.Parse(Value, parNo);

                //TODO: this exception is possible?
                //throw new EvaluationException(string.Format(Resource.exc_invalid_datatype_in_stringvalue, parNo, DataTypeValue));
            }
        }

        /// <summary>
        /// A string value can't be a Bag
        /// </summary>
        public bool IsBag
        {
            get { return false; }
        }

        /// <summary>
        /// A string value can't be a Bag
        /// </summary>
        public int BagSize
        {
            get { return 0; }
        }

        /// <summary>
        /// A string value can't be a Bag
        /// </summary>
        public ArrayList Elements
        {
            get { return null; }
        }

        /// <summary>
        /// A string value can't be converted into a function.
        /// </summary>
        /// <param name="parNo">THe parameter number used only for error reporing.</param>
        /// <returns></returns>
        public IFunction GetFunction(int parNo)
        {
            throw new EvaluationException(string.Format(Resource.exc_invalid_datatype_in_stringvalue, parNo, DataTypeValue));
        }

        #endregion
    }
}
