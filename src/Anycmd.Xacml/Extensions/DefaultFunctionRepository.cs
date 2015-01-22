using Anycmd.Xacml.Interfaces;
using System;
using System.Collections;
using System.Security.Permissions;
using System.Xml;
using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Extensions
{
    /// <summary>
    /// Default function repository which uses the configuration file to define the external 
    /// function types.
    /// </summary>
    public class DefaultFunctionRepository : inf.IFunctionRepository
    {
        #region Private members

        /// <summary>
        /// All the defined functions using the function id as the key.
        /// </summary>
        private readonly Hashtable _functions = new Hashtable();

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
            foreach (XmlNode node in functions)
            {
                var fun = (inf.IFunction)Activator.CreateInstance(Type.GetType(node.Attributes["type"].Value));
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
            return _functions[functionId] as inf.IFunction;
        }

        #endregion
    }
}
