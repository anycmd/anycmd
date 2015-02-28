
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Ac.Infra;
    using Engine.Edi;
    using Engine.Edi.Abstractions;
    using Info;
    using Query;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ‘实体’提供程序
    /// <remarks>
    /// 单引号中的实体是本数据交换平台所面对的业务数据，就是像学生和教师这种本体类别下的数据
    /// </remarks>
    /// </summary>
    public interface IEntityProvider : IWfResource
    {
        #region 插件信息
        /// <summary>
        /// 标识
        /// </summary>
        new Guid Id { get; }

        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 描述
        /// </summary>
        new string Description { get; }

        /// <summary>
        /// 作者。如xuexs
        /// </summary>
        string Author { get; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        ElementDataSchema GetElementDataSchema(ElementDescriptor element);

        #region 进程地址
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ontology"></param>
        /// <returns></returns>
        string GetEntityDataSource(OntologyDescriptor ontology);
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        ProcessResult ExecuteCommand(DbCmd command);

        #region 归档

        /// <summary>
        /// 归档数据
        /// </summary>
        /// <param name="ontology"></param>
        /// <param name="archive"></param>
        void Archive(OntologyDescriptor ontology, IArchive archive);

        /// <summary>
        /// 删除归档数据
        /// </summary>
        /// <param name="archive"></param>
        void DropArchive(ArchiveState archive);
        #endregion

        #region 根据给定的条件获取最顶端的两条记录
        /// <summary>
        /// 根据给定的本体码和信息标识获取本节点两条数据
        /// </summary>
        /// <param name="ontology">本体</param>
        /// <param name="infoIDs">多列联合信息标识字典，键必须不区分大小写</param>
        /// <param name="selectElements">选择元素</param>
        /// <returns></returns>
        TowInfoTuple GetTopTwo(OntologyDescriptor ontology, IEnumerable<InfoItem> infoIDs, OrderedElementSet selectElements);
        #endregion

        #region Get
        /// <summary>
        /// 获取给定本体码和存储标识的这个节点的数据
        /// <remarks>这个节点就是本进程所属的节点</remarks>
        /// </summary>
        /// <param name="ontology">本体</param>
        /// <param name="localEntityId"></param>
        /// <param name="selectElements"></param>
        /// <returns>数据记录，表现为字典形式，键是数据元素编码值是相应数据元素对应的数据项值</returns>
        InfoItem[] Get(OntologyDescriptor ontology, string localEntityId, OrderedElementSet selectElements);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archive"></param>
        /// <param name="localEntityId"></param>
        /// <param name="selectElements"></param>
        /// <returns></returns>
        InfoItem[] Get(ArchiveState archive, string localEntityId, OrderedElementSet selectElements);
        #endregion

        /// <summary>
        /// 获取给定实体标识列表中的标识标识的每一个实体元组
        /// </summary>
        /// <param name="ontology"></param>
        /// <param name="selectElements"></param>
        /// <param name="entityIDs"></param>
        /// <returns></returns>
        DataTuple GetList(OntologyDescriptor ontology, OrderedElementSet selectElements, List<string> entityIDs);

        #region GetPlist

        /// <summary>
        /// 按照目录分页获取指定节点、本体的数据
        /// <remarks>
        /// 1，如果传入的node是本节点自己则查询的是全部实体集。
        /// 如果传入的node不是本节点则就是应用节点，对于应用节点则查询的是与该节点相关的实体集。
        /// 2，如果传入的目录为空则表示获取全部目录的数据。
        /// </remarks>
        /// </summary>
        /// <param name="filters">过滤器列表</param>
        /// <param name="pagingData"></param>
        /// <param name="ontology"></param>
        /// <param name="selectElements"></param>
        /// <returns></returns>
        DataTuple GetPlist(OntologyDescriptor ontology, OrderedElementSet selectElements, List<FilterData> filters, PagingInput pagingData);

        /// <summary>
        /// 按照目录分页获取指定节点、归档的数据
        /// <remarks>
        /// 1，如果传入的node是本节点自己则查询的是全部实体集。
        /// 如果传入的node不是本节点则就是应用节点，对于应用节点则查询的是与该节点相关的实体集。
        /// 2，如果传入的目录为空则表示获取全部目录的数据。
        /// </remarks>
        /// </summary>
        /// <param name="archive"></param>
        /// <param name="selectElements"></param>
        /// <param name="filters"></param>
        /// <param name="pagingData"></param>
        /// <returns></returns>
        DataTuple GetPlist(ArchiveState archive, OrderedElementSet selectElements, List<FilterData> filters, PagingInput pagingData);
        #endregion
    }
}
