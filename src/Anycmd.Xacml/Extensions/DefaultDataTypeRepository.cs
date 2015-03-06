using Anycmd.Xacml.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using System.Xml;
using inf = Anycmd.Xacml.Interfaces;

namespace Anycmd.Xacml.Extensions
{
    /// <summary>
    /// Default data type repository which uses the configuration file to define the external 
    /// data types.
    /// </summary>
    public class DefaultDataTypeRepository : inf.IDataTypeRepository
    {
        #region Private members

        /// <summary>
        /// All the defined functions using the function id as the key.
        /// </summary>
        private readonly IDictionary<string, IDataType> _dataTypes = new Dictionary<string, IDataType>();

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultDataTypeRepository()
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
            var dataTypes = configNode.SelectNodes("./dataType");
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
                var dataType = (IDataType)Activator.CreateInstance(type);
                _dataTypes.Add(node.Attributes["id"].Value, dataType);
            }
        }

        /// <summary>
        /// Returns an instance of the data type descriptor using the data type id specified.
        /// </summary>
        /// <param name="typeId">The data type id referenced in the policy document.</param>
        /// <returns>The data type instance or null if the data type was not found.</returns>
        public IDataType GetDataType(string typeId)
        {
            return _dataTypes[typeId];
        }

        #endregion
    }
}
