using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    using Interfaces;

    /// <summary>
    /// 表示一个可读写的策略集。
    /// </summary>
    public class PolicySetElementReadWrite : XacmlElement, IHasTarget
    {
        #region Private members

        /// <summary>
        /// The policy combining algorithm name.
        /// </summary>
        private string _policyCombiningAlgorithm;

        /// <summary>
        /// The list of obigations defined.
        /// </summary>
        private ObligationReadWriteCollection _obligations = new ObligationReadWriteCollection();

        /// <summary>
        /// The target defining whether this policy set applies to a specific context request.
        /// </summary>
        private TargetElementReadWrite _target;

        /// <summary>
        /// The id.
        /// </summary>
        private string _id = String.Empty;

        /// <summary>
        /// The description.
        /// </summary>
        private string _description = String.Empty;

        /// <summary>
        /// All the policies defined in this policy set.
        /// </summary>
        private ArrayList _policies = new ArrayList();

        /// <summary>
        /// All the combiner parmeters in the policy set.
        /// </summary>
        private readonly ArrayList _combinerParameters = new ArrayList();

        /// <summary>
        /// All the combiner parmeters in the policy set.
        /// </summary>
        private readonly ArrayList _policyCombinerParameters = new ArrayList();

        /// <summary>
        /// All the combiner parmeters in the policy set.
        /// </summary>
        private readonly ArrayList _policySetCombinerParameters = new ArrayList();

        /// <summary>
        /// The XPath version supported.
        /// </summary>
        private string _xpathVersion = String.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new policySet using the arguments provided.
        /// </summary>
        /// <param name="id">The policy set id.</param>
        /// <param name="description">The description of the policy set.</param>
        /// <param name="target">The target for this policy set.</param>
        /// <param name="policies">All the policies inside this policy set.</param>
        /// <param name="policyCombiningAlgorithm">The policy combining algorithm for this policy set.</param>
        /// <param name="obligations">The obligations.</param>
        /// <param name="xpathVersion">The XPath version supported.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public PolicySetElementReadWrite(string id, string description, TargetElementReadWrite target, ArrayList policies, string policyCombiningAlgorithm,
            ObligationReadWriteCollection obligations, string xpathVersion, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _id = id;
            _description = description;
            _target = target;
            _policies = policies;
            _policyCombiningAlgorithm = policyCombiningAlgorithm;
            _obligations = obligations;
            _xpathVersion = xpathVersion;
        }

        /// <summary>
        /// Creates a new PolicySet using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReder positioned at the PolicySet element.</param>
        /// <param name="schemaVersion">The version of the schema that will be used to validate.</param>
        public PolicySetElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            // Validates the current node name
            if (reader.LocalName == Consts.Schema1.PolicySetElement.PolicySet &&
                ValidateSchema(reader, schemaVersion))
            {
                // Get the attributes
                _id = reader.GetAttribute(Consts.Schema1.PolicySetElement.PolicySetId);
                _policyCombiningAlgorithm = reader.GetAttribute(Consts.Schema1.PolicySetElement.PolicyCombiningAlgorithmId);

                // Read the inner nodes
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.Schema1.PolicySetElement.Description:
                            _description = reader.ReadElementString();
                            break;
                        case Consts.Schema1.PolicySetElement.PolicySetDefaults:
                            if (reader.Read() && reader.Read())
                            {
                                if (reader.LocalName == Consts.Schema1.PolicySetDefaultsElement.XPathVersion &&
                                    ValidateSchema(reader, schemaVersion))
                                {
                                    _xpathVersion = reader.ReadElementString();
                                    if (!string.IsNullOrEmpty(_xpathVersion) && _xpathVersion != Consts.Schema1.Namespaces.XPath10)
                                    {
                                        throw new Exception(string.Format(Properties.Resource.exc_unsupported_xpath_version, _xpathVersion));
                                    }
                                }
                                reader.Read();
                            }
                            break;
                        case Consts.Schema1.TargetElement.Target:
                            _target = new TargetElementReadWrite(reader, schemaVersion);
                            break;
                        case Consts.Schema1.PolicySetElement.PolicySet:
                            if (!reader.IsEmptyElement && reader.NodeType != XmlNodeType.EndElement)
                            {
                                _policies.Add(new PolicySetElementReadWrite(reader, schemaVersion));
                            }
                            break;
                        case Consts.Schema1.PolicyElement.Policy:
                            _policies.Add(new PolicyElementReadWrite(reader, schemaVersion));
                            break;
                        case Consts.Schema1.PolicySetIdReferenceElement.PolicySetIdReference:
                            _policies.Add(new PolicySetIdReferenceElementReadWrite(reader, schemaVersion));
                            break;
                        case Consts.Schema1.PolicyIdReferenceElement.PolicyIdReference:
                            _policies.Add(new PolicyIdReferenceElement(reader, schemaVersion));
                            break;
                        case Consts.Schema1.PolicySetElement.Obligations:
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
                        case Consts.Schema2.PolicySetElement.CombinerParameters:
                            // Read all the combiner parameters
                            while (reader.Read())
                            {
                                switch (reader.LocalName)
                                {
                                    case Consts.Schema2.CombinerParameterElement.CombinerParameter:
                                        _combinerParameters.Add(new CombinerParameterElement(reader, schemaVersion));
                                        break;
                                }
                                if (reader.LocalName == Consts.Schema2.PolicySetElement.CombinerParameters &&
                                    reader.NodeType == XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    break;
                                }
                            }
                            break;
                        case Consts.Schema2.PolicySetElement.PolicyCombinerParameters:
                            // Read all the policy combiner parameters
                            while (reader.Read())
                            {
                                switch (reader.LocalName)
                                {
                                    case Consts.Schema2.PolicyCombinerParameterElement.PolicyCombinerParameter:
                                        _policyCombinerParameters.Add(new PolicyCombinerParameterElement(reader, schemaVersion));
                                        break;
                                }
                                if (reader.LocalName == Consts.Schema2.PolicySetElement.PolicyCombinerParameters &&
                                    reader.NodeType == XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    break;
                                }
                            }
                            break;
                        case Consts.Schema2.PolicySetElement.PolicySetCombinerParameters:
                            // Read all the policy set combiner parameters
                            while (reader.Read())
                            {
                                switch (reader.LocalName)
                                {
                                    case Consts.Schema2.PolicySetCombinerParameterElement.PolicySetCombinerParameter:
                                        _policySetCombinerParameters.Add(new PolicySetCombinerParameterElement(reader, schemaVersion));
                                        break;
                                }
                                if (reader.LocalName == Consts.Schema2.PolicySetElement.PolicySetCombinerParameters &&
                                    reader.NodeType == XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    break;
                                }
                            }
                            break;
                    }
                    if (reader.LocalName == Consts.Schema1.PolicySetElement.PolicySet &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
                        reader.Read();
                        break;
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(Properties.Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The PolicySet Id.
        /// </summary>
        public virtual string Id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// The policy set description.
        /// </summary>
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// The policy combining algorithm Id.
        /// </summary>
        public virtual string PolicyCombiningAlgorithm
        {
            get { return _policyCombiningAlgorithm; }
            set { _policyCombiningAlgorithm = value; }
        }

        /// <summary>
        /// The list of obligations.
        /// </summary>
        public virtual ObligationReadWriteCollection Obligations
        {
            get { return _obligations; }
            set { _obligations = value; }
        }

        /// <summary>
        /// The Target for this PolicySet
        /// </summary>
        public virtual TargetElementReadWrite Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// All the policies defined in this PolicySet
        /// </summary>
        public virtual ArrayList Policies
        {
            get { return _policies; }
            set { _policies = value; }
        }

        /// <summary>
        /// The XPath version supported.
        /// </summary>
        public virtual string XPathVersion
        {
            get { return _xpathVersion; }
            set { _xpathVersion = value; }
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
        public void WriteDocument(XmlWriter writer, IDictionary<string, string> namespaces)
        {
            writer.WriteStartElement(Consts.Schema1.PolicySetElement.PolicySet);
            foreach (var name in namespaces)
            {
                writer.WriteAttributeString(Consts.Schema1.Namespaces.Xmlns, name.Key.ToString(CultureInfo.InvariantCulture), null, name.Value.ToString(CultureInfo.InvariantCulture));
            }
            writer.WriteAttributeString(Consts.Schema1.PolicySetElement.PolicySetId, this._id);
            writer.WriteAttributeString(Consts.Schema1.PolicySetElement.PolicyCombiningAlgorithmId, this._policyCombiningAlgorithm);
            if (!string.IsNullOrEmpty(this._description))
            {
                writer.WriteElementString(Consts.Schema1.PolicySetElement.Description, this._description);
            }
            if (!string.IsNullOrEmpty(this._xpathVersion))
            {
                writer.WriteStartElement(Consts.Schema1.PolicySetElement.PolicySetDefaults);
                writer.WriteElementString(Consts.Schema1.PolicySetDefaultsElement.XPathVersion, this._xpathVersion);
                writer.WriteEndElement();
            }

            if (this._target != null)
            {
                this._target.WriteDocument(writer);
            }
            foreach (object policy in this._policies)
            {
                if (policy is PolicyElementReadWrite)
                {
                    ((PolicyElementReadWrite)policy).WriteDocument(writer);
                }
                else if (policy is PolicySetElementReadWrite)
                {
                    ((PolicySetElementReadWrite)policy).WriteDocument(writer);
                }
                else if (policy is PolicyIdReferenceElementReadWrite)
                {
                    ((PolicyIdReferenceElementReadWrite)policy).WriteDocument(writer);
                }
                else if (policy is PolicySetIdReferenceElementReadWrite)
                {
                    ((PolicySetIdReferenceElementReadWrite)policy).WriteDocument(writer);
                }
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
            writer.WriteStartElement(Consts.Schema1.PolicySetElement.PolicySet);
            writer.WriteAttributeString(Consts.Schema1.PolicySetElement.PolicySetId, this._id);
            writer.WriteAttributeString(Consts.Schema1.PolicySetElement.PolicyCombiningAlgorithmId, this._policyCombiningAlgorithm);
            if (!string.IsNullOrEmpty(this._description))
            {
                writer.WriteElementString(Consts.Schema1.PolicySetElement.Description, this._description);
            }
            if (!string.IsNullOrEmpty(this._xpathVersion))
            {
                writer.WriteStartElement(Consts.Schema1.PolicySetElement.PolicySetDefaults);
                writer.WriteElementString(Consts.Schema1.PolicySetDefaultsElement.XPathVersion, this._xpathVersion);
                writer.WriteEndElement();
            }

            this._target.WriteDocument(writer);
            foreach (object policy in this._policies)
            {
                var write = policy as PolicyElementReadWrite;
                if (write != null)
                {
                    write.WriteDocument(writer);
                }
                else
                {
                    var readWrite = policy as PolicySetElementReadWrite;
                    if (readWrite != null)
                    {
                        readWrite.WriteDocument(writer);
                    }
                    else
                    {
                        var elementReadWrite = policy as PolicyIdReferenceElementReadWrite;
                        if (elementReadWrite != null)
                        {
                            elementReadWrite.WriteDocument(writer);
                        }
                        else
                        {
                            var referenceElementReadWrite = policy as PolicySetIdReferenceElementReadWrite;
                            if (referenceElementReadWrite != null)
                            {
                                referenceElementReadWrite.WriteDocument(writer);
                            }
                        }
                    }
                }
            }
            this._obligations.WriteDocument(writer);
            writer.WriteEndElement();
        }
        #endregion
    }
}
