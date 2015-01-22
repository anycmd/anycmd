using System;
using System.Collections.Generic;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    using Policy;

    /// <summary>
    /// Represents a Response document created during the evaluation.
    /// </summary>
    public class ResponseElement : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The Result elements of this Response.
        /// </summary>
        private readonly ResultCollection _results;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new Response using the Result list provided.
        /// </summary>
        /// <param name="results">The list of Results that will be contained in this Response.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public ResponseElement(IEnumerable<ResultElement> results, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            _results = new ResultCollection();
            if (results != null)
            {
                foreach (var result in results)
                {
                    _results.Add(result);
                }
            }
        }

        /// <summary>
        /// Creates a response using the XmlReader instance provided.
        /// </summary>
        /// <remarks>This method is only provided for testing purposes, because it's easy to run the ConformanceTests
        /// comparing the expected response with the response created.</remarks>
        /// <param name="reader">The XmlReader positioned at the Response node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public ResponseElement(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Context, schemaVersion)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            _results = new ResultCollection();
            if (reader.LocalName == Consts.ContextSchema.ResponseElement.Response)
            {
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case Consts.ContextSchema.ResultElement.Result:
                            _results.Add(new ResultElement(reader, schemaVersion));
                            break;
                    }
                    if (reader.LocalName == Consts.ContextSchema.ResponseElement.Response &&
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
        /// Gets the list of results.
        /// </summary>
        public ResultCollection Results
        {
            get { return _results; }
        }
        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return true; }
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Writes the Xml of this Response document.
        /// </summary>
        /// <param name="writer">An XmlWriter where the xml will be sent.</param>
        public void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            writer.WriteStartDocument();
            writer.WriteStartElement(Consts.ContextSchema.ResponseElement.Response, Consts.Schema1.Namespaces.Context);
            foreach (ResultElement result in Results)
            {
                // Create the Result node
                writer.WriteStartElement(Consts.ContextSchema.ResultElement.Result, Consts.Schema1.Namespaces.Context);

                // Create the Decission node
                writer.WriteElementString(Consts.ContextSchema.ResultElement.Decision, result.Decision.ToString());

                // Create the Status node
                writer.WriteStartElement(Consts.ContextSchema.StatusElement.Status);
                writer.WriteStartElement(Consts.ContextSchema.StatusElement.StatusCode);
                writer.WriteAttributeString(Consts.ContextSchema.StatusElement.Value, result.Status.StatusCode.Value);
                writer.WriteEndElement();
                writer.WriteEndElement();

                // Create the obligations node
                if (result.Obligations != null && result.Obligations.Count != 0)
                {
                    writer.WriteStartElement(Consts.Schema1.ObligationsElement.Obligations);
                    foreach (ObligationElement obligation in result.Obligations)
                    {
                        writer.WriteStartElement(Consts.Schema1.ObligationElement.Obligation);
                        writer.WriteAttributeString(Consts.Schema1.ObligationElement.ObligationId, obligation.ObligationId);
                        writer.WriteAttributeString(Consts.Schema1.ObligationElement.FulfillOn, obligation.FulfillOn.ToString());

                        if (obligation.AttributeAssignment != null && obligation.AttributeAssignment.Count != 0)
                        {
                            foreach (AttributeAssignmentElement attrAssign in obligation.AttributeAssignment)
                            {
                                writer.WriteStartElement(Consts.Schema1.ObligationElement.AttributeAssignment);
                                writer.WriteAttributeString(Consts.Schema1.AttributeAssignmentElement.AttributeId, attrAssign.AttributeId);
                                writer.WriteAttributeString(Consts.Schema1.AttributeValueElement.DataType, attrAssign.DataTypeValue);

                                writer.WriteString(attrAssign.Value);

                                writer.WriteEndElement();
                            }
                        }

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        #endregion
    }
}