
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using Model;

    /// <summary>
    /// 节点本体元素级动作实体。将节点和本体元素和本体动作三者的关系视作实体。
    /// </summary>
    public class NodeElementAction : NodeElementActionBase, IAggregateRoot
    {
        public NodeElementAction() { }
    }
}
