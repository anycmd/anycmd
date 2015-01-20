
namespace Anycmd.Serialization
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Represents the binary serializer.
    /// </summary>
    public class ObjectBinarySerializer : IObjectSerializer
    {
        #region Private Fields
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        #endregion

        #region IObjectSerializer Members
        /// <summary>
        /// Serializes an object into a byte stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The byte stream which contains the serialized data.</returns>
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            byte[] ret = null;
            using (var ms = new MemoryStream())
            {
                _binaryFormatter.Serialize(ms, obj);
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
            using (var ms = new MemoryStream(stream))
            {
                var ret = (TObject)_binaryFormatter.Deserialize(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}
