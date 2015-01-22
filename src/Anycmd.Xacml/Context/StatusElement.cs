using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents the Status of the Result node.
    /// </summary>
    public class StatusElement : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The status message.
        /// </summary>
        private readonly string _statusMessage;

        /// <summary>
        /// The status detail.
        /// </summary>
        private readonly string _statusDetail;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a Status using the supplied values.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="statusMessage">The status message.</param>
        /// <param name="statusDetail">The status detail.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public StatusElement(StatusCodeElement statusCode, string statusMessage, string statusDetail, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            StatusCode = statusCode;
            _statusMessage = statusMessage;
            _statusDetail = statusDetail;
        }

        /// <summary>
        /// Creates a new Status class using the XmlReade instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the Status node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public StatusElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName != Consts.ContextSchema.StatusElement.Status) return;
            while (reader.Read())
            {
                switch (reader.LocalName)
                {
                    case Consts.ContextSchema.StatusElement.StatusCode:
                        StatusCode = new StatusCodeElement(reader, schemaVersion);
                        break;
                    case Consts.ContextSchema.StatusElement.StatusMessage:
                        _statusMessage = reader.ReadElementString();
                        break;
                    case Consts.ContextSchema.StatusElement.StatusDetail:
                        _statusDetail = reader.ReadElementString();
                        break;
                }
                if (reader.LocalName == Consts.ContextSchema.StatusElement.Status &&
                    reader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The status message.
        /// </summary>
        public string StatusMessage
        {
            get { return _statusMessage; }
        }

        /// <summary>
        /// The status detail.
        /// </summary>
        public string StatusDetail
        {
            get { return _statusDetail; }
        }

        /// <summary>
        /// The status code.
        /// </summary>
        public StatusCodeElement StatusCode { get; set; }

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
