using Anycmd.Xacml.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using System.Xml;

namespace Anycmd.Xacml.Extensions
{
    /// <summary>
    /// Default function repository which uses the configuration file to define the external 
    /// function types.
    /// </summary>
    public class DefaultFunctionRepository : IFunctionRepository
    {
        #region Private members

        /// <summary>
        /// All the defined functions using the function id as the key.
        /// </summary>
        private readonly IDictionary<string, IFunction> _functions = new Dictionary<string, IFunction>();

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultFunctionRepository()
        {
        }

        #endregion

        #region IFunctionRepository Members

        /// <summary>
        /// Initializes the repository provider using XmlNode that defines the provider in the configuration file.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the provider in the configuration file.</param>
        [ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
        public void Init(XmlNode configNode)
        {
            if (configNode == null) throw new ArgumentNullException("configNode");
            XmlNodeList functions = configNode.SelectNodes("./function");
            Debug.Assert(functions != null, "functions != null");
            foreach (XmlNode node in functions)
            {
                if (node == null || node.Attributes == null || node.Attributes["type"] == null || string.IsNullOrEmpty(node.Attributes["type"].Value))
                {
                    throw new EvaluationException();
                }
                var type = Type.GetType(node.Attributes["type"].Value);
                if (type == null)
                {
                    throw new EvaluationException();
                }
                var fun = (IFunction)Activator.CreateInstance(type);
                _functions.Add(node.Attributes["id"].Value, fun);
            }
        }

        /// <summary>
        /// Returns an instance of the function using the function id specified.
        /// </summary>
        /// <param name="functionId">The function id referenced in the policy document.</param>
        /// <returns>The function instance or null if the function was not found.</returns>
        public IFunction GetFunction(string functionId)
        {
            return _functions[functionId];
        }

        #endregion
    }
}
