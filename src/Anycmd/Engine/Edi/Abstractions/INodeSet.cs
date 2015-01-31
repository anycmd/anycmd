
namespace Anycmd.Engine.Edi.Abstractions
{
    using Ac;
    using Hecp;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义业务节点集合
    /// </summary>
    public interface INodeSet : IEnumerable<NodeDescriptor>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 中心节点
        /// </summary>
        NodeDescriptor CenterNode { get; }

        /// <summary>
        /// 本业务节点
        /// </summary>
        NodeDescriptor ThisNode { get; }

        /// <summary>
        /// 获取与指定的节点标识相关联的节点描述对象。
        /// </summary>
        /// <param name="nodeId">
        /// 节点标识。
        /// </param>
        /// <param name="node">
        /// 节点描述对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="node"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="INodeSet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetNodeById(string nodeId, out NodeDescriptor node);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        bool TryGetNodeByPublicKey(string publicKey, out NodeDescriptor node);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        IReadOnlyDictionary<Verb, NodeElementActionState> GetNodeElementActions(NodeDescriptor node, ElementDescriptor element);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IEnumerable<ElementDescriptor> GetInfoIdElements(NodeDescriptor node);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        bool IsInfoIdElement(NodeDescriptor node, ElementDescriptor element);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IReadOnlyCollection<NodeElementCareState> GetNodeElementCares(NodeDescriptor node);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IReadOnlyCollection<NodeOntologyCareState> GetNodeOntologyCares(NodeDescriptor node);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<NodeOntologyCareState> GetNodeOntologyCares();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        bool IsCareforElement(NodeDescriptor node, ElementDescriptor element);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ontology"></param>
        /// <returns></returns>
        bool IsCareForOntology(NodeDescriptor node, OntologyDescriptor ontology);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ontology"></param>
        /// <returns></returns>
        IReadOnlyDictionary<CatalogState, NodeOntologyCatalogState> GetNodeOntologyCatalogs(NodeDescriptor node, OntologyDescriptor ontology);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<NodeOntologyCatalogState> GetNodeOntologyCatalogs();
    }
}
