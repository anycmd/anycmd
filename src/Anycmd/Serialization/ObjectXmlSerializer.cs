
namespace Anycmd.Serialization
{
    using System.IO;
    using NSXmlSerialization = System.Xml.Serialization;

    /// <summary>
    /// 表示Xml序列化器。
    /// </summary>
    public class ObjectXmlSerializer : IObjectSerializer
    {
        #region IObjectSerializer Members
        /// <summary>
        /// Serializes an object into a byte stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The byte stream which contains the serialized data.</returns>
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            var graphType = obj.GetType();
            var xmlSerializer = new NSXmlSerialization.XmlSerializer(graphType);
            byte[] ret = null;
            using (var ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                ret = ms.ToArray();
                ms.Close();
            }
            return ret;
        }

        /// <summary>
        /// Deserializes an object from the given byte stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The byte stream which contains the serialized data of the object.</param>
        /// <returns>The deserialized object.</returns>
        public virtual TObject Deserialize<TObject>(byte[] stream)
        {
            var xmlSerializer = new NSXmlSerialization.XmlSerializer(typeof(TObject));
            using (var ms = new MemoryStream(stream))
            {
                var ret = (TObject)xmlSerializer.Deserialize(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}
