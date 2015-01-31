
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using Model;

    /// <summary>
    /// 将本体与目录的关系视作实体
    /// </summary>
    public class OntologyCatalog : OntologyCatalogBase, IAggregateRoot
    {
        public OntologyCatalog() { }
    }
}
