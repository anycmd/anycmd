using System;
using System.Collections;
using System.Xml;

namespace Anycmd.Xacml.Context
{
    /// <summary>
    /// Defines a typed collection of Resources.
    /// </summary>
    public class ResourceReadWriteCollection : CollectionBase
    {
        #region CollectionBase members

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <value>The element at the specified index.</value>
        public virtual ResourceElementReadWrite this[int index]
        {
            get
            {
                return ((ResourceElementReadWrite)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Adds an object to the end of the CollectionBase.
        /// </summary>
        /// <param name="value">The Object to be added to the end of the CollectionBase. </param>
        /// <returns>The CollectionBase index at which the value has been added.</returns>
        public virtual int Add(ResourceElementReadWrite value)
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
        /// Gets the index of the given ResourceElementReadWrite in the collection
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public int GetIndex(ResourceElementReadWrite resource)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this.List[i] == resource)
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
            foreach (ResourceElementReadWrite resource in this.List)
            {
                writer.WriteStartElement(Consts.ContextSchema.RequestElement.Resource);

                if (resource.ResourceContent != null)
                {
                    writer.WriteStartElement(Consts.ContextSchema.ResourceElement.ResourceContent);
                    writer.WriteRaw(resource.ResourceContent.XmlDocument.InnerXml);
                    writer.WriteEndElement();
                }
                foreach (AttributeElementReadWrite attr in resource.Attributes)
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