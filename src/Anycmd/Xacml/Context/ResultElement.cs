using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    using Policy;
    using Runtime;

    /// <summary>
    /// Represents a Result node in the context document.
    /// </summary>
    public class ResultElement : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The resource id, used only if the request was a hierarchical request.
        /// </summary>
        private readonly string _resourceId;

        /// <summary>
        /// All the obligations copied during evaluation.
        /// </summary>
        private readonly ObligationCollection _obligations = new ObligationCollection();

        #endregion

        #region Constructor

        /// <summary>
        /// A default constructor.
        /// </summary>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public ResultElement(XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
        }

        /// <summary>
        /// Creates a new Result using the provided information.
        /// </summary>
        /// <param name="resourceId">The resource id for this result.</param>
        /// <param name="decision">The decission of the evaluation.</param>
        /// <param name="status">The status with information about the execution.</param>
        /// <param name="obligations">The list of obligations</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public ResultElement(string resourceId, Decision decision, StatusElement status, ObligationCollection obligations, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            _resourceId = resourceId;
            Decision = decision;

            // If the status is null, create an empty status
            Status = status ?? new StatusElement(null, null, null, schemaVersion);

            // If the obligations are null, leave the empty ObligationCollection.
            if (obligations != null)
            {
                _obligations = obligations;
            }
        }

        /// <summary>
        /// Creates a Result using an XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Result node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public ResultElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.ContextSchema.ResultElement.Result)
            {
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.ContextSchema.ResultElement.Decision:
                            // The parsing should be safe because the document was validated using a Xsd Shcema
                            Decision = (Decision)Enum.Parse(typeof(Decision), reader.ReadElementString(), false);
                            break;
                        case Consts.ContextSchema.StatusElement.Status:
                            Status = new StatusElement(reader, schemaVersion);
                            break;
                        case Consts.Schema1.ObligationsElement.Obligations:
                            while (reader.Read())
                            {
                                switch (reader.LocalName)
                                {
                                    case Consts.Schema1.ObligationElement.Obligation:
                                        _obligations.Add(new ObligationElement(reader, schemaVersion));
                                        break;
                                }
                                // Trick to support multiple nodes of the same name.
                                if (reader.LocalName == Consts.Schema1.ObligationsElement.Obligations &&
                                    reader.NodeType == XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    break;
                                }
                            }

                            break;
                    }
                    if (reader.LocalName == Consts.ContextSchema.ResultElement.Result &&
                        reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The resource id, if the request was a hierarchical request.
        /// </summary>
        public string ResourceId
        {
            get { return _resourceId; }
        }

        /// <summary>
        /// The evaluation decission.
        /// </summary>
        public Decision Decision { get; set; }

        /// <summary>
        /// The status for this result.
        /// </summary>
        public StatusElement Status { get; set; }

        /// <summary>
        /// The obligations copied during evaluation.
        /// </summary>
        public ObligationCollection Obligations
        {
            get { return _obligations; }
        }
        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return true; }
        }
        #endregion
    }
}
