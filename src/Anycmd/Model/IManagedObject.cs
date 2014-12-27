
namespace Anycmd.Model
{
    using Engine.Edi;
    using Engine.Info;

    /// <summary>
    /// 表示该接口的实现类是权限托管对象。
    /// </summary>
    public interface IManagedObject
    {
        OntologyDescriptor Ontology { get; }
        InfoItem[] InputValues { get; }
        InfoItem[] Entity { get; }
    }
}
