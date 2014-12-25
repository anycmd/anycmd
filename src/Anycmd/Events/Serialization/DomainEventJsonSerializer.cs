
namespace Anycmd.Events.Serialization
{
    using Anycmd.Serialization;

    /// <summary>
    /// Json格式领域对象序列化反序列化器。
    /// </summary>
    public class DomainEventJsonSerializer : ObjectJsonSerializer, IDomainEventSerializer
    {
    }
}
