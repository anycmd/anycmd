using Anycmd.Xacml.Interfaces;
using Anycmd.Xacml.Policy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace Anycmd.Xacml.Extensions
{
    using Runtime;

    /// <summary>
    /// The default policy repository implementation that uses the configuration file to establish all the policies 
    /// in the repository.
    /// </summary>
    public class DefaultPolicyRepository : IPolicyRepository
    {
        #region Private members

        /// <summary>
        /// All the policies defined in the configuration file.
        /// </summary>
        private readonly IDictionary<string, PolicyDocument> _policies = new Dictionary<string, PolicyDocument>();

        /// <summary>
        /// All the policy sets defined in the configuration file.
        /// </summary>
        private readonly IDictionary<string, PolicyDocument> _policySets = new Dictionary<string, PolicyDocument>();

        #endregion

        #region IPolicyRepository Members

        /// <summary>
        /// Initialization method called by the EvaluationEngine to initialize the extension.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the extension in the configuration file.</param>
        [ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
        public void Init(XmlNode configNode)
        {
            if (configNode == null) throw new ArgumentNullException("configNode");
            foreach (XmlNode node in configNode.ChildNodes)
            {
                if (node.Name == "PolicySet")
                {
                    if (node.Attributes == null)
                    {
                        throw new EvaluationException();
                    }
                    string policyId = node.Attributes["PolicySetId"].Value;
                    string filePath = node.Attributes["FilePath"].Value;
                    string version = node.Attributes["xacmlVersion"].Value;
                    var schemaVersion = (XacmlVersion)Enum.Parse(typeof(XacmlVersion), version, false);
                    _policySets.Add(policyId, new PolicyDocument(new XmlTextReader(new StreamReader(filePath)), schemaVersion));
                }
                else if (node.Name == "Policy")
                {
                    if (node.Attributes == null)
                    {
                        throw new EvaluationException();
                    }
                    string policyId = node.Attributes["PolicyId"].Value;
                    string filePath = node.Attributes["FilePath"].Value;
                    string version = node.Attributes["xacmlVersion"].Value;
                    var schemaVersion = (XacmlVersion)Enum.Parse(typeof(XacmlVersion), version, false);
                    _policies.Add(policyId, new PolicyDocument(new XmlTextReader(new StreamReader(filePath)), schemaVersion));
                }
            }
        }

        /// <summary>
        /// Returns a policy document using the PolicyRefereneId specified.
        /// </summary>
        /// <param name="policyReference">The policy reference with the Id of the policy searched.</param>
        /// <returns>The policy document.</returns>
        public PolicyElement GetPolicy(PolicyIdReferenceElement policyReference)
        {
            if (policyReference == null) throw new ArgumentNullException("policyReference");
            PolicyDocument doc = _policies[policyReference.PolicyId];
            if (doc != null)
            {
                return (PolicyElement)doc.Policy; //TODO: check if we have to return a read write or a read only policy here.
            }
            return null;
        }

        /// <summary>
        /// Returns a policy set document using the PolicySetRefereneId specified.
        /// </summary>
        /// <param name="policySetReference">The policy set reference with the Id of the policy set searched.</param>
        /// <returns>The policy set document.</returns>
        public PolicySetElement GetPolicySet(PolicySetIdReferenceElement policySetReference)
        {
            if (policySetReference == null) throw new ArgumentNullException("policySetReference");
            PolicyDocument doc = _policySets[policySetReference.PolicySetId];
            if (doc != null)
            {
                return (PolicySetElement)doc.PolicySet; //TODO: check if we have to return a read write or a read only policy here.
            }
            return null;
        }

        /// <summary>
        /// Method called by the EvaluationEngine when the evaluation is executed without a policy document, this 
        /// method search in the policy repository and return the first policy that matches its target with the
        /// context document specified.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <returns>The policy document ready to be used by the evaluation engine.</returns>
        public PolicyDocument Match(EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            PolicyDocument polEv = null;

            //Search if there is a policySet which target matches the context document
            foreach (PolicyDocument policy in _policySets.Values)
            {
                var tempPolicy = new PolicySet(context.Engine, (PolicySetElement)policy.PolicySet);

                var tempContext = new EvaluationContext(context.Engine, policy, context.ContextDocument);

                // Match the policy set target with the context document
                if (tempPolicy.Match(tempContext) == TargetEvaluationValue.Match)
                {
                    if (polEv == null)
                    {
                        polEv = policy;
                    }
                    else
                    {
                        throw new EvaluationException(Resource.exc_duplicated_policy_in_repository);
                    }
                }
            }

            //Search if there is a policy which target matches the context document
            foreach (PolicyDocument policy in _policies.Values)
            {
                var tempPolicy = new Policy((PolicyElement)policy.Policy);

                var tempContext = new EvaluationContext(context.Engine, policy, context.ContextDocument);

                // Match the policy target with the context document
                if (tempPolicy.Match(tempContext) == TargetEvaluationValue.Match)
                {
                    if (polEv == null)
                    {
                        polEv = policy;
                    }
                    else
                    {
                        throw new EvaluationException(Resource.exc_duplicated_policy_in_repository);
                    }
                }
            }
            return polEv;
        }

        #endregion
    }
}
