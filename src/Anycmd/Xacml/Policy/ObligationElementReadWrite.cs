using System;
using System.Xml;

namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Represents a read/write Obligation node found in the Policy document.
    /// </summary>
    public class ObligationElementReadWrite : XacmlElement
    {
        #region Private members

        /// <summary>
        /// The id of the obligation.
        /// </summary>
        private string _obligationId;

        /// <summary>
        /// The Effect that will trigger the obligation to be passed to the response.
        /// </summary>
        private Effect _fulfillOn;

        /// <summary>
        /// The AttributeAssignments nodes defined in the Obligation.
        /// </summary>
        private AttributeAssignmentReadWriteCollection _attributeAssignment = new AttributeAssignmentReadWriteCollection();

        #endregion

        #region Constructors
        /// <summary>
        /// Creates an ReadWriteObligationElement with the parameters given
        /// </summary>
        /// <param name="obligationId"></param>
        /// <param name="fulfillOn"></param>
        /// <param name="attributeAssignment"></param>
        public ObligationElementReadWrite(string obligationId, Effect fulfillOn, AttributeAssignmentReadWriteCollection attributeAssignment)
        {
            _obligationId = obligationId;
            _fulfillOn = fulfillOn;
            _attributeAssignment = attributeAssignment;
        }

        /// <summary>
        /// Creates a new instance of the Obligation class using the XmlReader instance provided.
        /// </summary>
        /// <param name="reader">The XmlReader positioned at the Obligation node.</param>
        /// <param name="schemaVersion">The version of the schema that was used to validate.</param>
        public ObligationElementReadWrite(XmlReader reader, XacmlVersion schemaVersion)
            : base(XacmlSchema.Policy, schemaVersion)
        {
            if (reader != null)
            {
                if (reader.LocalName == Consts.Schema1.ObligationElement.Obligation &&
                    ValidateSchema(reader, schemaVersion))
                {
                    _obligationId = reader.GetAttribute(Consts.Schema1.ObligationElement.ObligationId);

                    // Parses the Effect attribute value
                    _fulfillOn = (Effect)Enum.Parse(
                        typeof(Effect), reader.GetAttribute(Consts.Schema1.ObligationElement.FulfillOn), false);

                    // Read all the attribute assignments
                    while (reader.Read())
                    {
                        switch (reader.LocalName)
                        {
                            case Consts.Schema1.ObligationElement.AttributeAssignment:
                                _attributeAssignment.Add(new AttributeAssignmentElementReadWrite(reader, schemaVersion));
                                break;
                        }
                        if (reader.LocalName == Consts.Schema1.ObligationElement.Obligation &&
                            reader.NodeType == XmlNodeType.EndElement)
                        {
                            break;
                        }
                    }
                }
            }
            else
                _obligationId = "[TODO]: Add Obligation Id";
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets all the attribute assignments for this obligation.
        /// </summary>
        public virtual AttributeAssignmentReadWriteCollection AttributeAssignment
        {
            set { _attributeAssignment = value; }
            get { return _attributeAssignment; }
        }

        /// <summary>
        /// The effect that will trigger the obligation to be sent to the response.
        /// </summary>
        public virtual Effect FulfillOn
        {
            set { _fulfillOn = value; }
            get { return _fulfillOn; }
        }

        /// <summary>
        /// The obligation id.
        /// </summary>
        public virtual string ObligationId
        {
            set { _obligationId = value; }
            get { return _obligationId; }
        }
        /// <summary>
        /// Whether the instance is a read only version.
        /// </summary>
        public override bool IsReadOnly
        {
            get { return false; }
        }
        #endregion
    }
}
