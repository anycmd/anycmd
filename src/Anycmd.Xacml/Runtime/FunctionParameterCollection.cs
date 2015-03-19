using Anycmd.Xacml.Interfaces;
using System.Collections;

namespace Anycmd.Xacml.Runtime
{
    /// <summary>
    /// Defines a typed collection of IFunctionParameters.
    /// </summary>
    public class FunctionParameterCollection : CollectionBase
    {
        #region CollectionBase members

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public int Add(IFunctionParameter value)
        {
            return (List.Add(value));
        }

        /// <summary>
        /// Returns a IFunctionParameter[] of the Collection contents.
        /// </summary>
        /// <returns></returns>
        public IFunctionParameter[] ToArray()
        {
            var retArr = new IFunctionParameter[Count];
            List.CopyTo(retArr, 0);
            return retArr;
        }

        #endregion
    }
}
