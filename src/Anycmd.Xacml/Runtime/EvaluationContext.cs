using Anycmd.Xacml.Context;
using System;
using System.Collections;
using System.Globalization;

namespace Anycmd.Xacml.Runtime
{
    using Xacml.Policy;

    /// <summary>
    /// The evaluation context class.
    /// </summary>
    public class EvaluationContext
    {
        #region Private members

        /// <summary>
        /// The engine instance.
        /// </summary>
        private readonly EvaluationEngine _engine;

        /// <summary>
        /// The policy document instance.
        /// </summary>
        private readonly PolicyDocument _policyDocument;

        /// <summary>
        /// The context document instance.
        /// </summary>
        private readonly ContextDocument _contextDocument;

        /// <summary>
        /// The current indentation level.
        /// </summary>
        private int _indent;

        /// <summary>
        /// Keep the information about the missing attributes.
        /// </summary>
        private readonly ArrayList _missingAttributes = new ArrayList();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        private EvaluationContext()
        {
        }

        /// <summary>
        /// Creates a new instance of the evaluaion context.
        /// </summary>
        /// <param name="engine">The engine instance.</param>
        /// <param name="policyDocument">The policy document instance.</param>
        /// <param name="contextDocument">The context document instance.</param>
        public EvaluationContext(EvaluationEngine engine, PolicyDocument policyDocument, ContextDocument contextDocument)
            : this()
        {
            var attributes = new AttributeReadWriteCollection();
            foreach (AttributeElementReadWrite attribute in contextDocument.Request.Resources[0].Attributes)
            {
                attributes.Add(new AttributeElementReadWrite(attribute));
            }

            ResourceContentElement resourceContent = null;
            if (contextDocument.Request.Resources[0].ResourceContent != null)
            {
                resourceContent = new ResourceContentElement(
                        contextDocument.Request.Resources[0].ResourceContent.XmlDocument,
                        contextDocument.Request.Resources[0].ResourceContent.SchemaVersion);
            }

            _engine = engine;
            _policyDocument = policyDocument;
            _contextDocument = contextDocument;
            CurrentResource = new ResourceElementReadWrite(
                resourceContent,
                contextDocument.Request.Resources[0].ResourceScopeValue,
                attributes,
                contextDocument.Request.Resources[0].SchemaVersion);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The engine instance.
        /// </summary>
        public EvaluationEngine Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// The policy document instance.
        /// </summary>
        public PolicyDocument PolicyDocument
        {
            get { return _policyDocument; }
        }

        /// <summary>
        /// The context document instance.
        /// </summary>
        public ContextDocument ContextDocument
        {
            get { return _contextDocument; }
        }

        /// <summary>
        /// The current resource for evatuation.
        /// </summary>
        public ResourceElementReadWrite CurrentResource { get; set; }

        /// <summary>
        /// Whether an attribute was not found during the evaluation.
        /// </summary>
        public bool IsMissingAttribute { get; set; }

        /// <summary>
        /// Whether an error was found during the evaluation.
        /// </summary>
        public bool ProcessingError { get; set; }

        /// <summary>
        /// The current Policy being evaluated.
        /// </summary>
        public Policy CurrentPolicy { get; set; }

        /// <summary>
        /// The current PolicySet being evaluated.
        /// </summary>
        public PolicySet CurrentPolicySet { get; set; }

        /// <summary>
        /// The current rule being evaluated.
        /// </summary>
        public Rule CurrentRule { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Trace a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters to replace using string.Format.</param>
        public void Trace(string message, params object[] parameters)
        {
            Console.Write(new string(' ', _indent));
            Console.Write(String.Format(CultureInfo.InvariantCulture, message, parameters));
            Console.WriteLine();
        }

        /// <summary>
        /// Trace the values of the IsMissingAttribute and ProcessingError values in the context.
        /// </summary>
        public void TraceContextValues()
        {
            // Notify the errors
            if (IsMissingAttribute)
            {
                Trace("IsMissingAttribute: {0}", IsMissingAttribute);
            }
            if (ProcessingError)
            {
                Trace("ProcessingError: {0}", ProcessingError);
            }
        }

        /// <summary>
        /// Add identation to the trace messages.
        /// </summary>
        public void AddIndent()
        {
            _indent++;
        }

        /// <summary>
        /// Remove the indentation.
        /// </summary>
        public void RemoveIndent()
        {
            _indent--;
        }

        /// <summary>
        /// Adds an attribute designator to the missing attributes list.
        /// </summary>
        /// <param name="attrDes"></param>
        public void AddMissingAttribute(AttributeReferenceBase attrDes)
        {
            _missingAttributes.Add(attrDes);
        }

        #endregion

    }
}
