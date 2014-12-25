using System;
using System.Collections;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read-only PolicyDocument which may contain a Policy or a PolicySet
    /// </summary>
    public class PolicyDocument : PolicyDocumentReadWrite
    {
        #region Constructors
        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="schemaVersion"></param>
        public PolicyDocument(XmlReader reader, XacmlVersion schemaVersion)
            : base(reader, schemaVersion)
        {
        }
        /// <summary>
        /// Creates a new black PolicyDocument
        /// </summary>
        /// <param name="schemaVersion"></param>
        public PolicyDocument(XacmlVersion schemaVersion)
            : base(schemaVersion)
        {
        }
        #endregion

        public static PolicyDocumentReadWrite Create(XmlReader reader, XacmlVersion version, DocumentAccess access)
        {
            // Validate the parameters
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (access.Equals(DocumentAccess.ReadOnly))
            {
                return new PolicyDocument(reader, version);
            }
            else if (access.Equals(DocumentAccess.ReadWrite))
            {
                return new PolicyDocumentReadWrite(reader, version);
            }
            return null;
        }

        #region Public properties

        /// <summary>
        /// The version of the schema used to validate this instance.
        /// </summary>
        public override XacmlVersion Version
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Whether the document have passed the Xsd validation.
        /// </summary>
        public override bool IsValidDocument
        {
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// The PolicySet contained in the document.
        /// </summary>
        public override PolicySetElementReadWrite PolicySet
        {
            set { throw new NotSupportedException(); }
            get
            {
                if (base.PolicySet != null)
                    return new PolicySetElement(base.PolicySet.Id, base.PolicySet.Description, base.PolicySet.Target, base.PolicySet.Policies,
                        base.PolicySet.PolicyCombiningAlgorithm, base.PolicySet.Obligations, base.PolicySet.XPathVersion, base.PolicySet.SchemaVersion);
                else
                    return null;
            }
        }

        /// <summary>
        /// The Policy contained in the document.
        /// </summary>
        public override PolicyElementReadWrite Policy
        {
            set { throw new NotSupportedException(); }
            get
            {
                if (base.Policy != null)
                    return new PolicyElement(base.Policy.Id, base.Policy.Description, base.Policy.Target, base.Policy.Rules,
                        base.Policy.RuleCombiningAlgorithm, base.Policy.Obligations, base.Policy.XPathVersion, base.Policy.CombinerParameters,
                        base.Policy.RuleCombinerParameters, base.Policy.VariableDefinitions, base.Policy.SchemaVersion);
                else
                    return null;
            }
        }

        /// <summary>
        /// All the namespaced defined in the document.
        /// </summary>
        public override IDictionary Namespaces
        {
            set { throw new NotSupportedException(); }
        }

        #endregion
    }
}
