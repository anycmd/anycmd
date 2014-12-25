using System;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a RuleCombinerParameter defined in the policy document.
    /// </summary>
    public class RuleCombinerParameterElement : CombinerParameterElement
    {
        #region Private members

        /// <summary>
        /// The parameter name.
        /// </summary>
        private Uri _ruleIdRef;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new RuleCombinerParameter using the provided argument values.
        /// </summary>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <param name="ruleIdRef">The rule Id reference.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public RuleCombinerParameterElement(string parameterName, AttributeValueElement attributeValue, Uri ruleIdRef, XacmlVersion version)
            : base(parameterName, attributeValue, version)
        {
            _ruleIdRef = ruleIdRef;
        }

        /// <summary>
        /// Creates a new RuleCombinerParameter using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the CombinerParameterElement node.</param>
        /// <param name="version">The version of the schema that was used to validate.</param>
        public RuleCombinerParameterElement(XmlReader reader, XacmlVersion version)
            : base(reader, Consts.Schema2.RuleCombinerParameterElement.RuleCombinerParameter, version)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The parameter name
        /// </summary>
        public Uri RuleIdRef
        {
            get { return _ruleIdRef; }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Method called by the base class when unknown attributes are found during parsing of this element. 
        /// </summary>
        /// <param name="reader">The reader positioned at the attribute</param>
        protected override void AttributeFound(XmlReader reader)
        {
            if (reader.LocalName == Consts.Schema2.RuleCombinerParameterElement.RuleIdRef)
            {
                _ruleIdRef = new Uri(reader.GetAttribute(Consts.Schema2.RuleCombinerParameterElement.RuleIdRef));
            }
        }

        #endregion
    }
}
