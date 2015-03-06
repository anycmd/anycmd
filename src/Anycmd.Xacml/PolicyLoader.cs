
namespace Anycmd.Xacml
{
    using Policy;
    using System;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// 帮助从xml数据提供程序加载策略文档的帮助类。
    /// </summary>
    public static class PolicyLoader
    {
        #region Constructor

        #endregion

        #region Public methods

        /// <summary>
        /// 根据给定的xml文档字符串创建一个策略文档。
        /// </summary>
        /// <param name="xmlDocument">xml文档片段</param>
        /// <param name="access">文档可访问性</param>
        /// <returns>一个策略文档实例</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(string xmlDocument, DocumentAccess access)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            XacmlVersion version = GetXacmlVersion(new StreamReader(xmlDocument));

            return LoadPolicyDocument(new StringReader(xmlDocument), version, access);
        }

        /// <summary>
        /// Creates an instace of the PolicyDocument using the stream provided with an Xml document.
        /// </summary>
        /// <param name="xmlDocument">The stream containing an Xml document.</param>
        /// <param name="access">The type of PolicyDocument</param>
        /// <returns>An instance of a PolicyDocument.</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(Stream xmlDocument, DocumentAccess access)
        {
            // Validate the parameters
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            // Validate the stream
            if (!xmlDocument.CanSeek)
            {
                throw new ArgumentException(Resource.exc_invalid_stream_parameter_canseek, "xmlDocument");
            }

            // Read the document to determine the version of the schema used.
            XacmlVersion version = GetXacmlVersion(new StreamReader(xmlDocument));

            xmlDocument.Position = 0;

            return LoadPolicyDocument(new StreamReader(xmlDocument), version, access);
        }

        /// <summary>
        /// Creates an instace of the PolicyDocument using the stream provided with an Xml document.
        /// </summary>
        /// <param name="xmlDocument">The stream containing an Xml document.</param>
        /// <param name="version">The version of the schema that will be used to validate.</param>
        /// <param name="access">The type of PolicyDocument</param>
        /// <returns>An instance of a PolicyDocument.</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(Stream xmlDocument, XacmlVersion version, DocumentAccess access)
        {
            // Validate the parameters
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            return LoadPolicyDocument(new StreamReader(xmlDocument), version, access);
        }


        /// <summary>
        /// 根据给定的xml文档字符串创建一个策略文档。
        /// </summary>
        /// <param name="xmlDocument">xml文档片段</param>
        /// <param name="version">用于验证的模式版本号</param>
        /// <param name="access">文档可访问性</param>
        /// <returns>一个策略文档实例</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(TextReader xmlDocument, XacmlVersion version, DocumentAccess access)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            return LoadPolicyDocument(new XmlTextReader(xmlDocument), version, access);
        }

        /// <summary>
        /// 根据给定的xml文档字符串创建一个策略文档。
        /// </summary>
        /// <param name="reader">用于读取xml文档的读取器</param>
        /// <param name="version">用于验证的模式版本号</param>
        /// <param name="access">文档可访问性</param>
        /// <returns>一个策略文档实例</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(XmlReader reader, XacmlVersion version, DocumentAccess access)
        {
            return PolicyDocument.Create(reader, version, access);
        }

        /// <summary>
        /// Creates a read-only instace of the PolicyDocument using the provided Xml document string.
        /// </summary>
        /// <param name="xmlDocument">The Xml document fragment.</param>
        /// <returns>An instance of a PolicyDocument.</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(string xmlDocument)
        {
            // Validate the parameters
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            // Read the document to determine the version of the schema used.
            XacmlVersion version = GetXacmlVersion(new StreamReader(xmlDocument));

            return LoadPolicyDocument(new StringReader(xmlDocument), version);
        }

        /// <summary>
        /// Creates a read-only instace of the PolicyDocument using the stream provided with an Xml document.
        /// </summary>
        /// <param name="xmlDocument">The stream containing an Xml document.</param>
        /// <returns>An instance of a PolicyDocument.</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(Stream xmlDocument)
        {
            // Validate the parameters
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            // Validate the stream
            if (!xmlDocument.CanSeek)
            {
                throw new ArgumentException(Resource.exc_invalid_stream_parameter_canseek, "xmlDocument");
            }

            // Read the document to determine the version of the schema used.
            XacmlVersion version = GetXacmlVersion(new StreamReader(xmlDocument));

            xmlDocument.Position = 0;

            return LoadPolicyDocument(new StreamReader(xmlDocument), version);
        }

        /// <summary>
        /// Creates a read-only instace of the PolicyDocument using the stream provided with an Xml document.
        /// </summary>
        /// <param name="xmlDocument">The stream containing an Xml document.</param>
        /// <param name="version">The version of the schema that will be used to validate.</param>
        /// <returns>An instance of a PolicyDocument.</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(Stream xmlDocument, XacmlVersion version)
        {
            // Validate the parameters
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            return LoadPolicyDocument(new StreamReader(xmlDocument), version);
        }


        /// <summary>
        /// Creates a read-only instace of the PolicyDocument using the provided Xml document string.
        /// </summary>
        /// <param name="xmlDocument">The Xml document fragment.</param>
        /// <param name="version">The version of the schema that will be used to validate.</param>
        /// <returns>An instance of a PolicyDocument.</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(TextReader xmlDocument, XacmlVersion version)
        {
            // Validate the parameters
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            return LoadPolicyDocument(new XmlTextReader(xmlDocument), version);
        }
        /// <summary>
        /// Creates a read-only instance of the PolicyDocument using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader used to read the Xml document.</param>
        /// <param name="version">The version of the schema that will be used to validate.</param>
        /// <returns>An instance of a PolicyDocument.</returns>
        public static PolicyDocumentReadWrite LoadPolicyDocument(XmlReader reader, XacmlVersion version)
        {
            // Validate the parameters
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return new PolicyDocument(reader, version);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads the document to the first Policy or PolicySet element and compares the Namespace of that
        /// element with the namespaces for the different versions of the spec and determines the version of the
        /// schema that will be used.
        /// </summary>
        /// <param name="textReader">A reader positioned and ready to process.</param>
        /// <returns>The vesion of the schema required in the policy document.</returns>
        private static XacmlVersion GetXacmlVersion(TextReader textReader)
        {
            var reader = new XmlTextReader(textReader);
            while (reader.Read())
            {
                switch (reader.LocalName)
                {
                    case Consts.Schema1.PolicyElement.Policy:
                    case Consts.Schema1.PolicySetElement.PolicySet:
                        switch (reader.NamespaceURI)
                        {
                            case Consts.Schema1.Namespaces.Policy:
                                return XacmlVersion.Version11;
                            case Consts.Schema2.Namespaces.Policy:
                                return XacmlVersion.Version20;
                        }
                        break;
                }
            }
            throw new EvaluationException(Resource.exc_invalid_document_format_no_policyorpolicyset);
        }

        #endregion
    }
}
