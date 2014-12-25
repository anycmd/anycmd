
namespace Anycmd.Xacml.Configuration
{
    using Interfaces;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Diagnostics;
    using System.Xml;

    /// <summary>
    /// Main configuration class used to mantain all the configuration placed in the configuration section.
    /// </summary>
    public class ConfigurationRoot
    {
        #region Static members

        /// <summary>
        /// Static property used to reload the configuration.
        /// </summary>
        public static ConfigurationRoot Config
        {
            get
            {
                return (ConfigurationRoot)ConfigurationManager.GetSection("Xacml.net");
            }
        }

        #endregion

        #region Private members

        /// <summary>
        /// The configured attribute repositories.
        /// </summary>
        private readonly ArrayList _attributeRepositories = new ArrayList();

        /// <summary>
        /// The configured policy repositories.
        /// </summary>
        private readonly ArrayList _policyRepositories = new ArrayList();

        /// <summary>
        /// The configured function repositories.
        /// </summary>
        private readonly ArrayList _functionRepositories = new ArrayList();

        /// <summary>
        /// The configured data type repositories.
        /// </summary>
        private readonly ArrayList _dataTypeRepositories = new ArrayList();

        /// <summary>
        /// The configured rule combinig algorothm repositories.
        /// </summary>
        private readonly ArrayList _ruleCombiningRepositories = new ArrayList();

        /// <summary>
        /// The configured policy combinig algorothm repositories.
        /// </summary>
        private readonly ArrayList _policyCombiningRepositories = new ArrayList();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the configuration class using the XmlNode specified.
        /// </summary>
        /// <param name="configNode">The XmlNode for the configuration section.</param>
        public ConfigurationRoot(XmlNode configNode)
        {
            if (configNode == null) throw new ArgumentNullException("configNode");

            // Load all attribute repositories
            var nodeList = configNode.SelectNodes("./attributeRepositories/repository");
            Debug.Assert(nodeList != null, "nodeList != null");
            foreach (XmlNode node in nodeList)
            {
                var repConfig = new AttributeRepositoryConfig(node);

                var rep = (IAttributeRepository)Activator.CreateInstance(repConfig.Type);
                rep.Init(repConfig.XmlNode);

                _attributeRepositories.Add(rep);
            }

            // Load all policy repositories
            nodeList = configNode.SelectNodes("./policyRepositories/repository");
            Debug.Assert(nodeList != null, "nodeList != null");
            foreach (XmlNode node in nodeList)
            {
                var policyConfig = new PolicyRepositoryConfig(node);

                var rep = (IPolicyRepository)Activator.CreateInstance(policyConfig.Type);
                rep.Init(policyConfig.XmlNode);

                _policyRepositories.Add(rep);
            }

            // Load all function repositories
            nodeList = configNode.SelectNodes("./functionRepositories/repository");
            Debug.Assert(nodeList != null, "nodeList != null");
            foreach (XmlNode node in nodeList)
            {
                var functionConfig = new FunctionRepositoryConfig(node);

                var rep = (IFunctionRepository)Activator.CreateInstance(functionConfig.Type);
                rep.Init(functionConfig.XmlNode);

                _functionRepositories.Add(rep);
            }

            // Load all dataType repositories
            nodeList = configNode.SelectNodes("./dataTypeRepositories/repository");
            Debug.Assert(nodeList != null, "nodeList != null");
            foreach (XmlNode node in nodeList)
            {
                var dataTypeConfig = new DataTypeRepositoryConfig(node);

                var rep = (IDataTypeRepository)Activator.CreateInstance(dataTypeConfig.Type);
                rep.Init(dataTypeConfig.XmlNode);

                _dataTypeRepositories.Add(rep);
            }

            // Load all rule combinig algorothm repositories
            nodeList = configNode.SelectNodes("./ruleCombiningAlgorithmRepositories/repository");
            Debug.Assert(nodeList != null, "nodeList != null");
            foreach (XmlNode node in nodeList)
            {
                var ruleCaConfig = new RuleCombiningAlgorithmRepository(node);

                var rep = (IRuleCombiningAlgorithmRepository)Activator.CreateInstance(ruleCaConfig.Type);
                rep.Init(ruleCaConfig.XmlNode);

                _ruleCombiningRepositories.Add(rep);
            }

            // Load all policy combinig algorothm repositories
            nodeList = configNode.SelectNodes("./policyCombiningAlgorithmRepositories/repository");
            Debug.Assert(nodeList != null, "nodeList != null");
            foreach (XmlNode node in nodeList)
            {
                var policyCaConfig = new PolicyCombiningAlgorithmRepository(node);

                var rep = (IPolicyCombiningAlgorithmRepository)Activator.CreateInstance(policyCaConfig.Type);
                rep.Init(policyCaConfig.XmlNode);

                _policyCombiningRepositories.Add(rep);
            }

            // Load all rule combinig algorothm repositories
            nodeList = configNode.SelectNodes("./ruleCombiningAlgorithmRepositories/repository");
            Debug.Assert(nodeList != null, "nodeList != null");
            foreach (XmlNode node in nodeList)
            {
                _ruleCombiningRepositories.Add(new RuleCombiningAlgorithmRepository(node));
            }

        }

        #endregion

        #region Public properties

        /// <summary>
        /// The attribute repositories loaded.
        /// </summary>
        public ArrayList AttributeRepositories
        {
            get { return _attributeRepositories; }
        }

        /// <summary>
        /// The policy repositories loaded.
        /// </summary>
        public ArrayList PolicyRepositories
        {
            get { return _policyRepositories; }
        }

        /// <summary>
        /// The function repositories loaded.
        /// </summary>
        public ArrayList FunctionRepositories
        {
            get { return _functionRepositories; }
        }

        /// <summary>
        /// The datatype repositories loaded.
        /// </summary>
        public ArrayList DataTypeRepositories
        {
            get { return _dataTypeRepositories; }
        }

        /// <summary>
        /// The pca repositories loaded.
        /// </summary>
        public ArrayList PolicyCombiningAlgorithmRepositories
        {
            get { return _policyCombiningRepositories; }
        }

        /// <summary>
        /// The rca repositories loaded.
        /// </summary>
        public ArrayList RuleCombiningAlgorithmRepositories
        {
            get { return _ruleCombiningRepositories; }
        }

        #endregion
    }
}
