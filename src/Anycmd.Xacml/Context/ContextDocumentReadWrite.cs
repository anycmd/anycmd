
namespace Anycmd.Xacml.Context
{
    using Policy;
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// Mantains a context document which can be a Request or a response document.
    /// </summary>
    public class ContextDocumentReadWrite
    {
        /// <summary>
        /// The compiled schemas are kept in memory for performance reasons.
        /// </summary>
        private static XmlSchemaSet _compiledSchemas11;

        /// <summary>
        /// The compiled schemas are kept in memory for performance reasons.
        /// </summary>
        private static XmlSchemaSet _compiledSchemas20;

        #region Private members
        /// <summary>
        /// The request defined in the context document.
        /// </summary>
        private RequestElementReadWrite _request;

        /// <summary>
        /// Whether the document have passed the validation.
        /// </summary>
        private bool _isValidDocument = true;

        /// <summary>
        /// The name of the embedded resource for the 1.0 schema.
        /// </summary>
        public const string Xacml10ContextSchemaResourceName = "Anycmd.Xacml.Schemas.cs-xacml-schema-context-01.xsd";

        /// <summary>
        /// The name of the embedded resource for the 2.0 schema.
        /// </summary>
        public const string Xacml20ContextSchemaResourceName = "Anycmd.Xacml.Schemas.access_control-xacml-2.0-context-schema-os.xsd";

        /// <summary>
        /// All the namespaces and the prefix defined in the document. This is used in the XPath
        /// queries because the XPath uses the preffixes and we must provide them in the 
        /// XmlNamespaceManager.
        /// </summary>
        private readonly Hashtable _namespaces = new Hashtable();

        /// <summary>
        /// The string of the context document, dirty trick to use the XmlReader and also create an XmlDocument 
        /// instance.
        /// </summary>
        private readonly string _xmlString;

        /// <summary>
        /// The XmlDocument instance will be created the first time its requested.
        /// </summary>
        private XmlDocument _xmlDocument;

        /// <summary>
        /// The XmlNamespaceManager used to execute XPath queries over the document with the namespaces defined 
        /// in the policy document.
        /// </summary>
        private XmlNamespaceManager _xmlNamespaceManager;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new blank ContextDocumentReadWrite
        /// </summary>
        public ContextDocumentReadWrite()
        {
        }

        /// <summary>
        /// Creates a new ContextDocumentReadWrite using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the begining of the document.</param>
        /// <param name="schemaVersion">The schema used to validate this context document.</param>
        public ContextDocumentReadWrite(XmlReader reader, XacmlVersion schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            // Search for the first element.
            while (reader.Read() && reader.NodeType != XmlNodeType.Element)
            { }

            // Keep the contents of the context document.
            // HACK: Due to XPath validation the document must be readed twice, the first time to load the instance
            // and the second one to keep a document to execute XPath sentences.
            _xmlString = reader.ReadOuterXml();

            // Read the contents in a new reader.
            var sreader = new StringReader(_xmlString);

            // Prepare the validation.
            var validationHandler = new ValidationEventHandler(vreader_ValidationEventHandler);
            var settings = new XmlReaderSettings {ValidationType = ValidationType.Schema};
            settings.ValidationEventHandler += validationHandler;
            XmlReader vreader = null;
            try
            {
                switch (schemaVersion)
                {
                    case XacmlVersion.Version10:
                    case XacmlVersion.Version11:
                        {
                            if (_compiledSchemas11 == null)
                            {
                                var policySchemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(PolicyDocumentReadWrite.Xacml10PolicySchemaResourceName);
                                var contextSchemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Xacml10ContextSchemaResourceName);
                                _compiledSchemas11 = new XmlSchemaSet();
                                _compiledSchemas11.Add(XmlSchema.Read(policySchemaStream, validationHandler));
                                _compiledSchemas11.Add(XmlSchema.Read(contextSchemaStream, validationHandler));
                                _compiledSchemas11.Compile();
                            }
                            settings.Schemas.Add(_compiledSchemas11);
                            break;
                        }
                    case XacmlVersion.Version20:
                        {
                            if (_compiledSchemas20 == null)
                            {
                                Stream policySchemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(PolicyDocumentReadWrite.Xacml20PolicySchemaResourceName);
                                Stream contextSchemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Xacml20ContextSchemaResourceName);
                                _compiledSchemas20 = new XmlSchemaSet();
                                _compiledSchemas20.Add(XmlSchema.Read(policySchemaStream, validationHandler));
                                _compiledSchemas20.Add(XmlSchema.Read(contextSchemaStream, validationHandler));
                                _compiledSchemas20.Compile();
                            }
                            settings.Schemas.Add(_compiledSchemas20);
                            break;
                        }
                    default:
                        throw new ArgumentException(Resource.exc_invalid_version_parameter_value, "version");
                }
                vreader = XmlReader.Create(sreader, settings);
                while (vreader.Read())
                {
                    //Read all the namespaces and keep them in the _namespaces hashtable.
                    if (vreader.HasAttributes)
                    {
                        while (vreader.MoveToNextAttribute())
                        {
                            if (vreader.LocalName == Consts.Schema1.Namespaces.Xmlns)
                            {
                                _namespaces.Add(vreader.Prefix, vreader.Value);
                            }
                            else if (vreader.Prefix == Consts.Schema1.Namespaces.Xmlns)
                            {
                                _namespaces.Add(vreader.LocalName, vreader.Value);
                            }
                        }
                        vreader.MoveToElement();
                    }
                    switch (vreader.LocalName)
                    {
                        case Consts.ContextSchema.RequestElement.Request:
                            _request = new RequestElementReadWrite(vreader, schemaVersion);
                            break;
                        case Consts.ContextSchema.ResponseElement.Response:
                            Response = new ResponseElement(vreader, schemaVersion);
                            break;
                    }
                }
            }
            finally
            {
                if (vreader != null)
                {
                    vreader.Close();
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Whether the document loaded have passed the Xsd validation.
        /// </summary>
        public bool IsValidDocument
        {
            get { return _isValidDocument; }
        }

        /// <summary>
        /// The Request in the context document.
        /// </summary>
        public virtual RequestElementReadWrite Request
        {
            get { return _request; }
            set { _request = value; }
        }

        /// <summary>
        /// The Response in the context document.
        /// </summary>
        public ResponseElement Response { get; set; }

        /// <summary>
        /// Gets the XmlDocument for the context document in order to execute XPath queries. 
        /// </summary>
        public XmlDocument XmlDocument
        {
            get
            {
                if (_xmlDocument == null)
                {
                    _xmlDocument = new XmlDocument();
                    _xmlDocument.LoadXml(_xmlString);
                }
                return _xmlDocument;
            }
        }

        /// <summary>
        /// The XmlNamespaceManager with the namespaces defined in the context document.
        /// </summary>
        public XmlNamespaceManager XmlNamespaceManager
        {
            get
            {
                return _xmlNamespaceManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Hashtable Namespaces
        {
            get
            {
                return _namespaces;
            }
        }

        /// <summary>
        /// Allows adding extra namespaces to the namespace manager.
        /// </summary>
        public void AddNamespaces(IDictionary namespaces)
        {
            if (namespaces == null) throw new ArgumentNullException("namespaces");
            _xmlNamespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            foreach (string key in namespaces.Keys)
            {
                _xmlNamespaceManager.AddNamespace(key, (string)namespaces[key]);
            }
        }
        #endregion

        #region Private members

        /// <summary>
        /// Method called by the Xsd validator when an error is found during Xsd validation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vreader_ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
            _isValidDocument = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Writes the context element in the provided writer
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteRequestDocument(XmlWriter writer)
        {
            if (this._request != null)
            {
                this._request.WriteDocument(writer, _namespaces);
            }
        }

        #endregion
    }
}
