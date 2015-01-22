using System;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Represents a StatusCode in the Status node.
    /// </summary>
    public class StatusCodeElement : XacmlElement
    {
        #region Private members

        /// <summary>
        /// An inner status code.
        /// </summary>
        private readonly StatusCodeElement _statusCode;

        /// <summary>
        /// The value of the status code.
        /// </summary>
        private readonly string _value;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a StatusCode using an XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader instance positioned at the StatusCode node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public StatusCodeElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader.LocalName == Consts.ContextSchema.StatusElement.StatusCode)
            {
                _value = reader.GetAttribute(Consts.ContextSchema.StatusElement.Value);
                if (!reader.IsEmptyElement)
                {
                    while (reader.Read())
                    {
                        switch (reader.LocalName)
                        {
                            case Consts.ContextSchema.StatusElement.StatusCode:
                                _statusCode = new StatusCodeElement(reader, schemaVersion);
                                break;
                        }
                        if (reader.LocalName == Consts.ContextSchema.StatusElement.StatusCode &&
                            reader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                throw new Exception(string.Format(Resource.exc_invalid_node_name, reader.LocalName));
            }
        }

        /// <summary>
        /// Creates a status code using the values supplied.
        /// </summary>
        /// <param name="value">The value of the status code.</param>
        /// <param name="statusCode">Another inner status code.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public StatusCodeElement(string value, StatusCodeElement statusCode, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            _value = value;
            _statusCode = statusCode;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The inner status code.
        /// </summary>
        public StatusCodeElement InnerStatusCode
        {
            get { return _statusCode; }
        }

        /// <summary>
        /// The value for this status code.
        /// </summary>
        public string Value
        {
            get { return _value; }
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
