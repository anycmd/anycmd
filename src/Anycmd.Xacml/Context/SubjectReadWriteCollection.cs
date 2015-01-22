using System;
using System.Collections;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Defines a typed collection of Subjects.
    /// </summary>
    public class SubjectReadWriteCollection : CollectionBase
    {
        #region CollectionBase members

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public virtual int Add(SubjectElementReadWrite value)
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
        /// <summary>
        /// Gets the index of the given SubjectElementReadWrite in the collection
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public int GetIndex(SubjectElementReadWrite subject)
        {
            for (var i = 0; i < this.Count; i++)
            {
                if (this.List[i] == subject)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Writes the element in the provided writer
        /// </summary>
        /// <param name="writer">The XmlWriter in which the element will be written</param>
        public void WriteDocument(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            foreach (SubjectElementReadWrite subject in this.List)
            {
                writer.WriteStartElement(Consts.ContextSchema.RequestElement.Subject);
                if (!string.IsNullOrEmpty(subject.SubjectCategory))
                {
                    writer.WriteAttributeString(Consts.ContextSchema.SubjectElement.SubjectCategory, subject.SubjectCategory);
                }
                foreach (AttributeElementReadWrite attr in subject.Attributes)
                {
                    writer.WriteStartElement(Consts.ContextSchema.AttributeElement.Attribute);
                    writer.WriteAttributeString(Consts.ContextSchema.AttributeElement.AttributeId, attr.AttributeId);
                    writer.WriteAttributeString(Consts.ContextSchema.AttributeElement.DataType, attr.DataType);
                    if (!string.IsNullOrEmpty(attr.Issuer))
                    {
                        writer.WriteAttributeString(Consts.ContextSchema.AttributeElement.Issuer, attr.Issuer);
                    }
                    foreach (AttributeValueElementReadWrite attVal in attr.AttributeValues)
                    {
                        writer.WriteElementString(Consts.ContextSchema.AttributeElement.AttributeValue, attVal.Value);
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        #endregion
    }
}