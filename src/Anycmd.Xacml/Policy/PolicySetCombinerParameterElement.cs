using System;
using System.Diagnostics;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a PolicyCombinerParameter defined in the policy document.
    /// </summary>
    public class PolicySetCombinerParameterElement : CombinerParameterElement
    {
        #region Private members

        /// <summary>
        /// The parameter name.
        /// </summary>
        private Uri _policySetIdRef;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new PolicyCombinerParameter using the provided argument values.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="policySetIdRef">The policy set Id reference.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public PolicySetCombinerParameterElement(string parameterName, AttributeValueElement attributeValue, Uri policySetIdRef, XacmlVersion version)
            : base(parameterName, attributeValue, version)
        {
            _policySetIdRef = policySetIdRef;
        }

        /// <summary>
        /// Creates a new PolicyCombinerParameter using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the CombinerParameterElement node.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public PolicySetCombinerParameterElement(XmlReader reader, XacmlVersion version)
            : base(reader, Consts.Schema2.PolicySetCombinerParameterElement.PolicySetCombinerParameter, version)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The parameter name
        /// </summary>
        public Uri PolicySetIdRef
        {
            get { return _policySetIdRef; }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Method called by the base class when unknown attributes are found during parsing of this element. 
        /// </summary>
        /// <param name="reader">The reader positioned at the attribute</param>
        protected override void AttributeFound(XmlReader reader)
        {
            if (reader.LocalName == Consts.Schema2.PolicySetCombinerParameterElement.PolicySetIdRef)
            {
                var url = reader.GetAttribute(Consts.Schema2.PolicySetCombinerParameterElement.PolicySetIdRef);
                Debug.Assert(url != null);
                _policySetIdRef = new Uri(url);
            }
        }

        #endregion
    }
}
