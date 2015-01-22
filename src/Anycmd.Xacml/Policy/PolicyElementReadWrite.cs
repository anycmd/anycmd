using System;
using System.Collections;
using System.Xml;

using cor = Anycmd.Xacml;
using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read/write policy node defined in the Policy document.
    /// </summary>
    public class PolicyElementReadWrite : XacmlElement, inf.IHasTarget
    {
        #region Private members

        /// <summary>
        /// The policy id.
        /// </summary>
        private string _id = String.Empty;

        /// <summary>
        /// The policy description.
        /// </summary>
        private string _description = String.Empty;

        /// <summary>
        /// The target for this policy.
        /// </summary>
        private TargetElementReadWrite _target;

        /// <summary>
        /// The list of rules defined in the policy.
        /// </summary>
        private RuleReadWriteCollection _rules = new RuleReadWriteCollection();

        /// <summary>
        /// The rule combination algorothm that will lead the policy evaluation.
        /// </summary>
        private string _ruleCombiningAlgorithm;

        /// <summary>
        /// The list of obligations that defines this policy.
        /// </summary>
        private ObligationReadWriteCollection _obligations = new ObligationReadWriteCollection();

        /// <summary>
        /// All the combiner parmeters in the policy 
        /// </summary>
        private ArrayList _combinerParameters = new ArrayList();

        /// <summary>
        /// All the combiner parmeters in the policy 
        /// </summary>
        private ArrayList _ruleCombinerParameters = new ArrayList();

        /// <summary>
        /// All the variable definitions in the policy 
        /// </summary>
        private IDictionary _variableDefinitions = new Hashtable();

        /// <summary>
        /// The XPath version asumed and required to be supported in the evaluation engine.
        /// </summary>
        private string _xpathVersion = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Policy with the specified arguments.
        /// </summary>
        /// <param name="id">The policy id.</param>
        /// <param name="description">The policy description.</param>
        /// <param name="target">The policy target.</param>
        /// <param name="rules">The rules for this policy.</param>
        /// <param name="ruleCombiningAlgorithm">The rule combining algorithm.</param>
        /// <param name="obligations">The Obligations for this policy.</param>
        /// <param name="xpathVersion">The XPath version supported.</param>
        /// <param name="combinerParameters">The combiner parameters in this policy.</param>
        /// <param name="ruleCombinerParameters">The rule parameters in this policy.</param>
        /// <param name="variableDefinitions">The variable definitions of this policy.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public PolicyElementReadWrite(string id, string description, TargetElementReadWrite target, RuleReadWriteCollection rules, string ruleCombiningAlgorithm, ObligationReadWriteCollection obligations, string xpathVersion,
            ArrayList combinerParameters, ArrayList ruleCombinerParameters, IDictionary variableDefinitions, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _id = id;
            _description = description;
            _target = target;
            _rules = rules;
            _ruleCombiningAlgorithm = ruleCombiningAlgorithm;
            _obligations = obligations;
            _combinerParameters = combinerParameters;
            _ruleCombinerParameters = ruleCombinerParameters;
            _variableDefinitions = variableDefinitions;
            _xpathVersion = xpathVersion;
        }

        /// <summary>
        /// Creates a new Policy using the XmlReader instance specified.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the Policy node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public PolicyElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader.LocalName == Consts.Schema1.PolicyElement.Policy &&
                ValidateSchema(reader, schemaVersion))
            {
                // Read the policy id
                _id = reader.GetAttribute(Consts.Schema1.PolicyElement.PolicyId);

                // Read the policy combining algorithm
                _ruleCombiningAlgorithm = reader.GetAttribute(Consts.Schema1.PolicyElement.RuleCombiningAlgorithmId);
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.Schema1.PolicyElement.Description:
                            _description = reader.ReadElementString();
                            break;
                        case Consts.Schema1.PolicyElement.PolicyDefaults:
                            // Read all the policy defaults.
                            if (reader.Read() && reader.Read())
                            {
                                if (reader.LocalName == Consts.Schema1.PolicyDefaultsElement.XPathVersion &&
                                    ValidateSchema(reader, schemaVersion))
                                {
                                    _xpathVersion = reader.ReadElementString();
                                    if (_xpathVersion != null && _xpathVersion.Length != 0 && _xpathVersion != Consts.Schema1.Namespaces.XPath10)
                                    {
                                        throw new Exception(string.Format(cor.Resource.exc_unsupported_xpath_version, _xpathVersion));
                                    }
                                }
                                reader.Read();
                            }
                            break;
                        case Consts.Schema1.TargetElement.Target:
                            _target = new TargetElementReadWrite(reader, schemaVersion);
                            break;
                        case Consts.Schema1.RuleElement.Rule:
                            _rules.Add(new RuleElementReadWrite(reader, schemaVersion));
                            break;
                        case Consts.Schema1.PolicyElement.Obligations:
                            // Read all the obligations
                            while (reader.Read())
                            {
                                switch (reader.LocalName)
                                {
                                    case Consts.Schema1.ObligationElement.Obligation:
                                        _obligations.Add(new ObligationElementReadWrite(reader, schemaVersion));
                                        break;
                                }
                                if (reader.LocalName == Consts.Schema1.ObligationsElement.Obligations &&
                                    reader.NodeType == XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    break;
                                }
                            }
                            break;
                        case Consts.Schema2.PolicyElement.CombinerParameters:
                            // Read all the combiner parameters
                            while (reader.Read())
                            {
                                switch (reader.LocalName)
                                {
                                    case Consts.Schema2.CombinerParameterElement.CombinerParameter:
                                        _combinerParameters.Add(new CombinerParameterElement(reader, schemaVersion));
                                        break;
                                }
                                if (reader.LocalName == Consts.Schema2.PolicyElement.CombinerParameters &&
                                    reader.NodeType == XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    break;
                                }
                            }
                            break;
                        case Consts.Schema2.PolicyElement.RuleCombinerParameters:
                            // Read all the rule parameters
                            while (reader.Read())
                            {
                                switch (reader.LocalName)
                                {
                                    case Consts.Schema2.RuleCombinerParameterElement.RuleCombinerParameter:
                                        _ruleCombinerParameters.Add(new RuleCombinerParameterElement(reader, schemaVersion));
                                        break;
                                }
                                if (reader.LocalName == Consts.Schema2.PolicyElement.RuleCombinerParameters &&
                                    reader.NodeType == XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    break;
                                }
                            }
                            break;
                        case Consts.Schema2.PolicyElement.VariableDefinition:
                            VariableDefinitionElement variable = new VariableDefinitionElement(reader, schemaVersion);
                            _variableDefinitions.Add(variable.Id, variable);
                            break;
                    }
                    if (reader.LocalName == Consts.Schema1.PolicyElement.Policy &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(cor.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the Id of the policy.
        /// </summary>
        public virtual string Id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// Gets the description of the policy.
        /// </summary>
        public virtual string Description
        {
            set { _description = value; }
            get { return _description; }
        }

        /// <summary>
        /// Gets the rule combining algorithm name
        /// </summary>
        public virtual string RuleCombiningAlgorithm
        {
            set { _ruleCombiningAlgorithm = value; }
            get { return _ruleCombiningAlgorithm; }
        }

        /// <summary>
        /// Gets the target instance.
        /// </summary>
        public virtual TargetElementReadWrite Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// Gets the list of rules.
        /// </summary>
        public virtual RuleReadWriteCollection Rules
        {
            set { _rules = value; }
            get { return _rules; }
        }

        /// <summary>
        /// Gets the list of obligations.
        /// </summary>
        public virtual ObligationReadWriteCollection Obligations
        {
            set { _obligations = value; }
            get { return _obligations; }
        }

        /// <summary>
        /// The XPath version supported.
        /// </summary>
        public virtual string XPathVersion
        {
            set { _xpathVersion = value; }
            get { return _xpathVersion; }
        }

        /// <summary>
        /// The variable definitions.
        /// </summary>
        public virtual IDictionary VariableDefinitions
        {
            set { _variableDefinitions = value; }
            get { return _variableDefinitions; }
        }

        /// <summary>
        /// The combiner parameters
        /// </summary>
        public virtual ArrayList CombinerParameters
        {
            set { _combinerParameters = value; }
            get { return _combinerParameters; }
        }

        /// <summary>
        /// The rule combiner parameters
        /// </summary>
        public virtual ArrayList RuleCombinerParameters
        {
            set { _ruleCombinerParameters = value; }
            get { return _ruleCombinerParameters; }
        }

        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        /// <param name="namespaces">The xml's namespaces</param>
        public void WriteDocument(XmlWriter writer, Hashtable namespaces)
        {
            writer.WriteStartElement(Consts.Schema1.PolicyElement.Policy);
            foreach (DictionaryEntry name in namespaces)
            {
                writer.WriteAttributeString(Consts.Schema1.Namespaces.Xmlns, name.Key.ToString(), null, name.Value.ToString());
            }

            writer.WriteAttributeString(Consts.Schema1.PolicyElement.PolicyId, this._id);
            writer.WriteAttributeString(Consts.Schema1.PolicyElement.RuleCombiningAlgorithmId, this._ruleCombiningAlgorithm);

            if (this._description != null && this._description.Length != 0)
            {
                writer.WriteElementString(Consts.Schema1.PolicyElement.Description, this._description);
            }
            if (this._xpathVersion != null && this._xpathVersion.Length != 0)
            {
                writer.WriteStartElement(Consts.Schema1.PolicyElement.PolicyDefaults);
                writer.WriteElementString(Consts.Schema1.PolicyDefaultsElement.XPathVersion, this._xpathVersion);
                writer.WriteEndElement();
            }

            if (this._target != null)
            {
                this._target.WriteDocument(writer);
            }
            if (_variableDefinitions != null)
            {
                foreach (DictionaryEntry variable in this._variableDefinitions)
                {
                    ((VariableDefinitionElement)variable.Value).WriteDocument(writer);
                }
            }
            if (this._rules != null)
            {
                this._rules.WriteDocument(writer);
            }
            if (this._obligations != null)
            {
                this._obligations.WriteDocument(writer);
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteDocument(XmlWriter writer)
        {
            writer.WriteStartElement(Consts.Schema1.PolicyElement.Policy);
            writer.WriteAttributeString(Consts.Schema1.PolicyElement.PolicyId, this._id);
            writer.WriteAttributeString(Consts.Schema1.PolicyElement.RuleCombiningAlgorithmId, this._ruleCombiningAlgorithm);
            if (this._description != null && this._description.Length != 0)
            {
                writer.WriteElementString(Consts.Schema1.PolicyElement.Description, this._description);
            }
            if (this._xpathVersion != null && this._xpathVersion.Length != 0)
            {
                writer.WriteStartElement(Consts.Schema1.PolicyElement.PolicyDefaults);
                writer.WriteElementString(Consts.Schema1.PolicyDefaultsElement.XPathVersion, this._xpathVersion);
                writer.WriteEndElement();
            }

            if (this._target != null)
            {
                this._target.WriteDocument(writer);
            }
            if (this._variableDefinitions != null)
            {
                foreach (DictionaryEntry variable in this._variableDefinitions)
                {
                    ((VariableDefinitionElement)variable.Value).WriteDocument(writer);
                }
            }
            if (this._rules != null)
            {
                this._rules.WriteDocument(writer);
            }
            if (this._obligations != null)
            {
                this._obligations.WriteDocument(writer);
            }
            writer.WriteEndElement();
        }
        #endregion
    }
}
