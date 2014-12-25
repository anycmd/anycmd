using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read-only PolicyIdReference defined within a PolicySet.
    /// </summary>
    public class PolicyIdReferenceElement : PolicyIdReferenceElementReadWrite
    {
        #region Constructors

        /// <summary>
        /// Creates a new PolicyIdReference using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the PolicyIdReference element.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public PolicyIdReferenceElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }
        /// <summary>
        /// Creates an instance of the element, with the provided values
        /// </summary>
        /// <param name="policyIdReference"></param>
        /// <param name="version"></param>
        /// <param name="earliestVersion"></param>
        /// <param name="latestVersion"></param>
        /// <param name="schemaVersion"></param>
        public PolicyIdReferenceElement(string policyIdReference, string version, string earliestVersion, string latestVersion,
            XacmlVersion schemaVersion)
            : base(policyIdReference, version, earliestVersion, latestVersion, schemaVersion)
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The referenced PolicyId.
        /// </summary>
        public override string PolicyId
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The referenced Policy version.
        /// </summary>
        public override string Version
        {
            set { throw new NotSupportedException(); }
        }


        /// <summary>
        /// The referenced Policy earliest version.
        /// </summary>
        public override string EarliestVersion
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The referenced Policy latest version.
        /// </summary>
        public override string LatestVersion
        {
            set { throw new NotSupportedException(); }
        }

        #endregion
    }
}
