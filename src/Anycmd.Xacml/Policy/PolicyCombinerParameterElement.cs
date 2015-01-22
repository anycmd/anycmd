using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a PolicyCombinerParameter defined in the policy document.
    /// </summary>
    public class PolicyCombinerParameterElement : CombinerParameterElement
    {
        #region Private members

        /// <summary>
        /// The parameter name.
        /// </summary>
        private Uri _policyIdRef;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new PolicyCombinerParameter using the provided argument values.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="policyIdRef">The policy Id reference.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public PolicyCombinerParameterElement(string parameterName, AttributeValueElement attributeValue, Uri policyIdRef, XacmlVersion version)
            : base(parameterName, attributeValue, version)
        {
            _policyIdRef = policyIdRef;
        }

        /// <summary>
        /// Creates a new PolicyCombinerParameter using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the CombinerParameterElement node.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public PolicyCombinerParameterElement(XmlReader reader, XacmlVersion version)
            : base(reader, Consts.Schema2.PolicyCombinerParameterElement.PolicyCombinerParameter, version)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The parameter name
        /// </summary>
        public Uri PolicyIdRef
        {
            get { return _policyIdRef; }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Method called by the base class when unknown attributes are found during parsing of this element. 
        /// </summary>
        /// <param name="reader">The reader positioned at the attribute</param>
        protected override void AttributeFound(XmlReader reader)
        {
            if (reader.LocalName == Consts.Schema2.PolicyCombinerParameterElement.PolicyIdRef)
            {
                _policyIdRef = new Uri(reader.GetAttribute(Consts.Schema2.PolicyCombinerParameterElement.PolicyIdRef));
            }
        }

        #endregion
    }
}
