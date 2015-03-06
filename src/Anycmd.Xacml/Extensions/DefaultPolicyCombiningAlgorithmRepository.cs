using Anycmd.Xacml.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using System.Xml;

namespace Anycmd.Xacml.Extensions
{
    /// <summary>
    /// Default data type repository which uses the configuration file to define the external 
    /// data types.
    /// </summary>
    public class DefaultPolicyCombiningAlgorithmRepository : IPolicyCombiningAlgorithmRepository
    {
        #region Private members

        /// <summary>
        /// All the defined functions using the function id as the key.
        /// </summary>
        private readonly IDictionary<string, IPolicyCombiningAlgorithm> _algorithms = new Dictionary<string, IPolicyCombiningAlgorithm>();

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultPolicyCombiningAlgorithmRepository()
        {
        }

        #endregion

        #region IDataTypeRepository Members

        /// <summary>
        /// Initializes the repository provider using XmlNode that defines the provider in the configuration file.
        /// </summary>
        /// <param name="configNode">The XmlNode that defines the provider in the configuration file.</param>
        [ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
        public void Init(XmlNode configNode)
        {
            if (configNode == null) throw new ArgumentNullException("configNode");
            XmlNodeList dataTypes = configNode.SelectNodes("./policyCombiningAlgorithm");
            Debug.Assert(dataTypes != null, "dataTypes != null");
            foreach (XmlNode node in dataTypes)
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
                var pca = (IPolicyCombiningAlgorithm)Activator.CreateInstance(type);
                _algorithms.Add(node.Attributes["id"].Value, pca);
            }
        }

        /// <summary>
        /// Returns an instance of the policyCombiningAlgorithm descriptor using the data type id specified.
        /// </summary>
        /// <param name="policyCombiningAlgorithmId">The policyCombiningAlgorithm id referenced in the policy document.</param>
        /// <returns>The policyCombiningAlgorithm instance or null if the policyCombiningAlgorithm was not found.</returns>
        public IPolicyCombiningAlgorithm GetPolicyCombiningAlgorithm(string policyCombiningAlgorithmId)
        {
            return _algorithms[policyCombiningAlgorithmId];
        }

        #endregion
    }
}
