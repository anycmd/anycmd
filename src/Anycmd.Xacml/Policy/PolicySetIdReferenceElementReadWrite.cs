using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read/write PolicySetIdReference defined in the policy document.
    /// </summary>
    public class PolicySetIdReferenceElementReadWrite : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The Id of the referenced policy set.
        /// </summary>
        private string _policySetIdReference;

        /// <summary>
        /// The referenced Policy version.
        /// </summary>
        private string _version;

        /// <summary>
        /// The referenced Policy earliest version.
        /// </summary>
        private string _earliestVersion;

        /// <summary>
        /// The referenced Policy latest version.
        /// </summary>
        private string _latestVersion;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a policy set id reference using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the "PolicySetIdReference" 
        /// node</param>
        /// <param name="schemaVersion">The version of the schema that will be used to validate.</param>
        public PolicySetIdReferenceElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.Schema1.PolicySetIdReferenceElement.PolicySetIdReference &&
                ValidateSchema(reader, schemaVersion))
            {
                if (reader.HasAttributes)
                {
                    // Load all the attributes
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.LocalName == Consts.Schema2.PolicyReferenceElement.Version)
                        {
                            _version = reader.GetAttribute(Consts.Schema2.PolicyReferenceElement.Version);
                        }
                        else if (reader.LocalName == Consts.Schema2.PolicyReferenceElement.EarliestVersion)
                        {
                            _earliestVersion = reader.GetAttribute(Consts.Schema2.PolicyReferenceElement.EarliestVersion);
                        }
                        else if (reader.LocalName == Consts.Schema2.PolicyReferenceElement.LatestVersion)
                        {
                            _latestVersion = reader.GetAttribute(Consts.Schema2.PolicyReferenceElement.LatestVersion);
                        }
                    }
                    reader.MoveToElement();
                }
                _policySetIdReference = reader.ReadElementString();
            }
            else
            {
                throw new Exception(string.Format(Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        /// <summary>
        /// Creates an instance of the element, with the provided values
        /// </summary>
        /// <param name="policySetIdReference"></param>
        /// <param name="version"></param>
        /// <param name="earliestVersion"></param>
        /// <param name="latestVersion"></param>
        /// <param name="schemaVersion"></param>
        public PolicySetIdReferenceElementReadWrite(string policySetIdReference, string version, string earliestVersion, string latestVersion,
            XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            _policySetIdReference = policySetIdReference;
            _version = version;
            _earliestVersion = earliestVersion;
            _latestVersion = latestVersion;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The Id of the referenced policy set.
        /// </summary>
        public virtual string PolicySetId
        {
            set { _policySetIdReference = value; }
            get { return _policySetIdReference; }
        }

        /// <summary>
        /// The referenced Policy version.
        /// </summary>
        public virtual string Version
        {
            set { _version = value; }
            get { return _version; }
        }

        /// <summary>
        /// The referenced Policy earliest version.
        /// </summary>
        public virtual string EarliestVersion
        {
            set { _earliestVersion = value; }
            get { return _earliestVersion; }
        }

        /// <summary>
        /// The referenced Policy latest version.
        /// </summary>
        public virtual string LatestVersion
        {
            set { _latestVersion = value; }
            get { return _latestVersion; }
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
        /// Writes the current element in the provided XmlWriter
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.WriteElementString(Consts.Schema1.PolicySetIdReferenceElement.PolicySetIdReference, this._policySetIdReference);
        }

        #endregion
    }
}
