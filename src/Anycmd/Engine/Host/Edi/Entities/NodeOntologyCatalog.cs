
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using Model;

    /// <summary>
    /// 节点目录。将节点和本体和目录三者的关系视作实体。
    /// </summary>
    public class NodeOntologyCatalog : NodeOntologyCatalogBase, IAggregateRoot
    {
        public NodeOntologyCatalog() { }
    }
}
