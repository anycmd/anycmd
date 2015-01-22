using System;
using System.Collections;
using System.Security.Permissions;
using System.Xml;
using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Extensions
{
    /// <summary>
    /// Default data type repository which uses the configuration file to define the external 
    /// data types.
    /// </summary>
    public class DefaultRuleCombiningAlgorithmRepository : inf.IRuleCombiningAlgorithmRepository
    {
        #region Private members

        /// <summary>
        /// All the defined functions using the function id as the key.
        /// </summary>
        private Hashtable _algorithms = new Hashtable();

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultRuleCombiningAlgorithmRepository()
        {
        }

        #endregion

        #region IRuleCombiningAlgorithmRepository Members

        /// <summary>
        /// Initializes the repository provider using XmlNode that defines the provider in the configuration file.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the provider in the configuration file.</param>
        [ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
        public void Init(XmlNode configNode)
        {
            if (configNode == null) throw new ArgumentNullException("configNode");
            XmlNodeList dataTypes = configNode.SelectNodes("./ruleCombiningAlgorithm");
            foreach (XmlNode node in dataTypes)
            {
                inf.IRuleCombiningAlgorithm rca = (inf.IRuleCombiningAlgorithm)Activator.CreateInstance(Type.GetType(node.Attributes["type"].Value));
                _algorithms.Add(node.Attributes["id"].Value, rca);
            }
        }

        /// <summary>
        /// Returns an instance of the ruleCombiningAlgorithm descriptor using the data type id specified.
        /// </summary>
        /// <param name="ruleCombiningAlgorithmId">The ruleCombiningAlgorithm id referenced in the policy document.</param>
        /// <returns>The ruleCombiningAlgorithm instance or null if the ruleCombiningAlgorithm was not found.</returns>
        public inf.IRuleCombiningAlgorithm GetRuleCombiningAlgorithm(string ruleCombiningAlgorithmId)
        {
            return _algorithms[ruleCombiningAlgorithmId] as inf.IRuleCombiningAlgorithm;
        }

        #endregion
    }
}
