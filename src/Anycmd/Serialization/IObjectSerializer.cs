
namespace Anycmd.Serialization
{
    /// <summary>
    /// 表示该接口的实现类是对象序列化器。
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        /// 将给定的对象序列化为字节数组。
        /// </summary>
        /// <typeparam name="TObject">被序列化的对象的类型。</typeparam>
        /// <param name="obj">被序列化的对象。</param>
        /// <returns>对象被序列化后得到的字节数组。</returns>
        byte[] Serialize<TObject>(TObject obj);

        /// <summary>
        /// 将给定的字节数组反序列化为指定类型的对象。
        /// </summary>
        /// <typeparam name="TObject">对象类型。</typeparam>
        /// <param name="stream">对象被序列化后得到的字节数组。</param>
        /// <returns>反序列化得到的对象。</returns>
        TObject Deserialize<TObject>(byte[] stream);
    }
}
