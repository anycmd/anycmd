using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Xml;
using ctx = Anycmd.Xacml.Context;
using inf = Anycmd.Xacml.Interfaces;
using pol = Anycmd.Xacml.Policy;
using rtm = Anycmd.Xacml.Runtime;

namespace Anycmd.Xacml.Extensions
{
    /// <summary>
    /// The default implementation of a repository attribute that uses the configuration file to define the 
    /// attribute.
    /// </summary>
    public class DefaultAttributeRepository : inf.IAttributeRepository
    {
        /// <summary>
        /// The attributes defined in the configuration file.
        /// </summary>
        private readonly ctx.AttributeCollection _attributes = new ctx.AttributeCollection();

        /// <summary>
        /// Default public constructor.
        /// </summary>
        public DefaultAttributeRepository()
        {
        }

        /// <summary>
        /// Initialization method called by the EvaluationEngine with the XmlNode with the configuration element 
        /// that defines the repository.
        /// </summary>
        /// <param name="configNode">The XmlNode with the configuration.</param>
        [ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
        public void Init(XmlNode configNode)
        {
            if (configNode == null) throw new ArgumentNullException("configNode");
            // Iterate through the child nodes and finds any node named "Attribute"
            foreach (XmlNode node in configNode.ChildNodes)
            {
                if (node.Name != "Attribute") continue;
                // Search for the value of the attribute
                var value = node.SelectSingleNode("./AttributeValue");

                // Get the attribute Id
                Debug.Assert(node.Attributes != null, "node.Attributes != null");
                var attributeId = node.Attributes["AttributeId"].Value;

                // Get the issuer
                Debug.Assert(value != null, "value != null");
                Debug.Assert(value.Attributes != null, "value.Attributes != null");
                var issuer = value.Attributes["Issuer"] != null ? value.Attributes["Issuer"].Value : "";

                string dataType = value.Attributes["DataType"] != null ? value.Attributes["DataType"].Value : "";

                // Add a new instance of the Attribute class using the configuration information
                _attributes.Add(
                    new ctx.AttributeElement(
                        attributeId,
                        dataType,
                        issuer,
                        null, value.InnerText, XacmlVersion.Version11)); //TODO: remove the version hardcoded
            }
        }

        /// <summary>
        /// Method called by the EvaluationEngine when an attribute is not found.
        /// </summary>
        /// <param name="context">The evaluation context instance.</param>
        /// <param name="designator">The attribute designator.</param>
        /// <returns>An instance of an Attribute with it's value.</returns>
        public ctx.AttributeElement GetAttribute(rtm.EvaluationContext context, pol.AttributeDesignatorBase designator)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (designator == null) throw new ArgumentNullException("designator");
            foreach (var attrib in _attributes.Cast<ctx.AttributeElement>().Where(attrib => attrib.AttributeId == designator.AttributeId))
            {
                if (!string.IsNullOrEmpty(designator.Issuer))
                {
                    if (designator.Issuer == attrib.Issuer)
                    {
                        return attrib;
                    }
                }
                else
                {
                    return attrib;
                }
            }
            return null;
        }
    }
}
