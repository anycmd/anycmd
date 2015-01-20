
namespace Anycmd.Serialization
{
    using System.Text;

    public static class ObjectSerializerExtension
    {
        public static string SerializeToString<TObject>(this IObjectSerializer serializer, TObject obj)
        {
            byte[] data = serializer.Serialize(obj);

            return Encoding.UTF8.GetString(data);
        }

        public static TObject Deserialize<TObject>(this IObjectSerializer serializer, string stream)
        {
            byte[] data = Encoding.UTF8.GetBytes(stream);

            return serializer.Deserialize<TObject>(data);
        }
    }
}
