
namespace Anycmd.Engine.Edi.Abstractions
{
    using Ac;
    using Hecp;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 定义本体集合
    /// </summary>
    public interface IOntologySet : IEnumerable<OntologyDescriptor>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 根据本体码索引本体
        /// </summary>
        /// <param name="ontologyCode"></param>
        /// <returns></returns>
        OntologyDescriptor this[string ontologyCode] { get; }

        /// <summary>
        /// 根据本体标识索引书体描述对象
        /// </summary>
        /// <param name="ontologyId">本体标识</param>
        /// <returns></returns>
        OntologyDescriptor this[Guid ontologyId] { get; }

        /// <summary>
        /// 获取与指定的本体码相关联的本体描述对象。
        /// </summary>
        /// <param name="ontologyCode">
        /// 本体码。
        /// </param>
        /// <param name="ontology">
        /// 本体描述对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="ontology"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="IOntologySet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetOntology(string ontologyCode, out OntologyDescriptor ontology);

        /// <summary>
        /// 获取与指定的本体标识相关联的本体描述对象。
        /// </summary>
        /// <param name="ontologyId">
        /// 节点标识。
        /// </param>
        /// <param name="ontology">
        /// 本体描述对象。当此方法返回时，如果找到指定键，则返回与该键相关联的值；
        /// 否则，将返回 <paramref name="ontology"/> 参数的类型的默认值。该参数未经初始化即被传递。
        /// </param>
        /// <returns>
        /// 如果实现<seealso cref="IOntologySet"/>的对象包含具有指定键的元素，则为
        /// true；否则，为 false。
        /// </returns>
        bool TryGetOntology(Guid ontologyId, out OntologyDescriptor ontology);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        bool TryGetElement(Guid elementId, out ElementDescriptor element);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        ElementDescriptor GetElement(Guid elementId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, ElementDescriptor> GetElements(OntologyDescriptor ontology);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns></returns>
        IReadOnlyDictionary<Verb, ActionState> GetActons(OntologyDescriptor ontology);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        ActionState GetAction(Guid actionId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns></returns>
        IList<InfoGroupState> GetInfoGroups(OntologyDescriptor ontology);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns></returns>
        IReadOnlyDictionary<CatalogState, OntologyCatalogState> GetOntologyCatalogs(OntologyDescriptor ontology);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, TopicState> GetEventSubjects(OntologyDescriptor ontology);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveId"></param>
        /// <param name="archive"></param>
        /// <returns></returns>
        bool TryGetArchive(Guid archiveId, out ArchiveState archive);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<ArchiveState> GetArchives(OntologyDescriptor ontology);
    }
}
