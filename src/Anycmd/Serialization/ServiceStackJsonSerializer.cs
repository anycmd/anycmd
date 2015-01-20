
namespace Anycmd.Serialization
{
    using ServiceStack.Text;
    using System.IO;

    public class ServiceStackJsonSerializer : IObjectSerializer
    {
        public byte[] Serialize<TObject>(TObject obj)
        {
            using (var ms = new MemoryStream())
            {
                byte[] ret = null;
                JsonSerializer.SerializeToStream(obj, ms);
                ret = ms.ToArray();
                ms.Close();
                return ret;
            }
        }

        public TObject Deserialize<TObject>(byte[] stream)
        {
            using (var ms = new MemoryStream(stream))
            {
                var ret = JsonSerializer.DeserializeFromStream<TObject>(ms);
                ms.Close();
                return ret;
            }
        }
    }
}
