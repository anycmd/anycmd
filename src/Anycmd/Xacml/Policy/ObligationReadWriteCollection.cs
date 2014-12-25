using System;
using System.Collections;
using System.Xml;


namespace Anycmd.Xacml.Policy
{
    /// <summary>
    /// Defines a typed collection of read/write Obligations.
    /// </summary>
    public class ObligationReadWriteCollection : CollectionBase
    {
        #region CollectionBase members

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public virtual int Add(ObligationElementReadWrite value)
        {
            return (List.Add(value));
        }

        /// <summary>
        /// Clears the collection
        /// </summary>
        public virtual new void Clear()
        {
            base.Clear();
        }
        /// <summary>
        /// Removes the specified element
        /// </summary>
        /// <param name="index">Position of the element</param>
        public virtual new void RemoveAt(int index)
        {
            base.RemoveAt(index);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Writes the XML of the current element
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (this.Count > 0)
            {
                writer.WriteStartElement(Consts.Schema1.ObligationsElement.Obligations);
                foreach (ObligationElementReadWrite oObligation in this)
                {
                    writer.WriteStartElement(Consts.Schema1.ObligationElement.Obligation);
                    writer.WriteAttributeString(Consts.Schema1.ObligationElement.ObligationId, oObligation.ObligationId);
                    writer.WriteAttributeString(Consts.Schema1.ObligationElement.FulfillOn, oObligation.FulfillOn.ToString());
                    foreach (AttributeAssignmentElementReadWrite oAttribute in oObligation.AttributeAssignment)
                    {
                        writer.WriteStartElement(Consts.Schema1.ObligationElement.AttributeAssignment);
                        writer.WriteAttributeString(Consts.Schema1.AttributeAssignmentElement.AttributeId, oAttribute.AttributeId);
                        writer.WriteAttributeString(Consts.Schema1.AttributeAssignmentElement.DataType, oAttribute.DataTypeValue);
                        writer.WriteString(oAttribute.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}
