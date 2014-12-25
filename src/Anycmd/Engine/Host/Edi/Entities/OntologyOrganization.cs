
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using Model;

    /// <summary>
    /// 将本体与组织结构的关系视作实体
    /// </summary>
    public class OntologyOrganization : OntologyOrganizationBase, IAggregateRoot
    {
        public OntologyOrganization() { }
    }
}
