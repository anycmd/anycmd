
namespace Anycmd.Events.Serialization
{
    using Anycmd.Serialization;

    /// <summary>
    /// DataContract格式领域对象序列化反序列化器。
    /// </summary>
    public class DomainEventDataContractSerializer : ObjectDataContractSerializer, IDomainEventSerializer
    {
    }
}
