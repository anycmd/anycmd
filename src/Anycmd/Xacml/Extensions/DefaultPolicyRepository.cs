using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Xml;
using cor = Anycmd.Xacml;
using inf = Anycmd.Xacml.Interfaces;
using pol = Anycmd.Xacml.Policy;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Extensions
{
    /// <summary>
    /// The default policy repository implementation that uses the configuration file to establish all the policies 
    /// in the repository.
    /// </summary>
    public class DefaultPolicyRepository : inf.IPolicyRepository
    {
        #region Private members

        /// <summary>
        /// All the policies defined in the configuration file.
        /// </summary>
        private Hashtable _policies = new Hashtable();

        /// <summary>
        /// All the policy sets defined in the configuration file.
        /// </summary>
        private Hashtable _policySets = new Hashtable();

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
                    string policyId = node.Attributes["PolicySetId"].Value;
                    string filePath = node.Attributes["FilePath"].Value;
                    string version = node.Attributes["xacmlVersion"].Value;
                    XacmlVersion schemaVersion = (XacmlVersion)Enum.Parse(typeof(XacmlVersion), version, false);
                    _policySets.Add(policyId, new pol.PolicyDocument(new XmlTextReader(new StreamReader(filePath)), schemaVersion));
                }
                else if (node.Name == "Policy")
                {
                    string policyId = node.Attributes["PolicyId"].Value;
                    string filePath = node.Attributes["FilePath"].Value;
                    string version = node.Attributes["xacmlVersion"].Value;
                    XacmlVersion schemaVersion = (XacmlVersion)Enum.Parse(typeof(XacmlVersion), version, false);
                    _policies.Add(policyId, new pol.PolicyDocument(new XmlTextReader(new StreamReader(filePath)), schemaVersion));
                }
            }
        }

        /// <summary>
        /// Returns a policy document using the PolicyRefereneId specified.
        /// </summary>
        /// <param name="policyReference">The policy reference with the Id of the policy searched.</param>
        /// <returns>The policy document.</returns>
        public pol.PolicyElement GetPolicy(pol.PolicyIdReferenceElement policyReference)
        {
            if (policyReference == null) throw new ArgumentNullException("policyReference");
            pol.PolicyDocument doc = _policies[policyReference.PolicyId] as pol.PolicyDocument;
            if (doc != null)
            {
                return (pol.PolicyElement)doc.Policy; //TODO: check if we have to return a read write or a read only policy here.
            }
            return null;
        }

        /// <summary>
        /// Returns a policy set document using the PolicySetRefereneId specified.
        /// </summary>
        /// <param name="policySetReference">The policy set reference with the Id of the policy set searched.</param>
        /// <returns>The policy set document.</returns>
        public pol.PolicySetElement GetPolicySet(pol.PolicySetIdReferenceElement policySetReference)
        {
            if (policySetReference == null) throw new ArgumentNullException("policySetReference");
            pol.PolicyDocument doc = _policySets[policySetReference.PolicySetId] as pol.PolicyDocument;
            if (doc != null)
            {
                return (pol.PolicySetElement)doc.PolicySet; //TODO: check if we have to return a read write or a read only policy here.
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
        public pol.PolicyDocument Match(rtm.EvaluationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            pol.PolicyDocument polEv = null;

            //Search if there is a policySet which target matches the context document
            foreach (pol.PolicyDocument policy in _policySets.Values)
            {
                rtm.PolicySet tempPolicy = new rtm.PolicySet(context.Engine, (pol.PolicySetElement)policy.PolicySet);

                rtm.EvaluationContext tempContext = new rtm.EvaluationContext(context.Engine, policy, context.ContextDocument);

                // Match the policy set target with the context document
                if (tempPolicy.Match(tempContext) == rtm.TargetEvaluationValue.Match)
                {
                    if (polEv == null)
                    {
                        polEv = policy;
                    }
                    else
                    {
                        throw new EvaluationException(cor.Resource.exc_duplicated_policy_in_repository);
                    }
                }
            }

            //Search if there is a policy which target matches the context document
            foreach (pol.PolicyDocument policy in _policies.Values)
            {
                rtm.Policy tempPolicy = new rtm.Policy((pol.PolicyElement)policy.Policy);

                rtm.EvaluationContext tempContext = new rtm.EvaluationContext(context.Engine, policy, context.ContextDocument);

                // Match the policy target with the context document
                if (tempPolicy.Match(tempContext) == rtm.TargetEvaluationValue.Match)
                {
                    if (polEv == null)
                    {
                        polEv = policy;
                    }
                    else
                    {
                        throw new EvaluationException(cor.Resource.exc_duplicated_policy_in_repository);
                    }
                }
            }
            return polEv;
        }

        #endregion
    }
}
