
namespace Anycmd.Engine.Host.Edi.Handlers
{
    using Engine.Edi;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 命令提供程序
    /// <remarks>
    /// 仓储接口层不应出现任何与存储相关的对象。
    /// </remarks>
    /// </summary>
    public interface IMessageProvider : IWfResource
    {
        #region Public Properties
        /// <summary>
        /// 命令提供程序标识
        /// </summary>
        new Guid Id { get; }

        /// <summary>
        /// 命令提供程序标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 命令提供程序描述
        /// </summary>
        new string Description { get; }

        /// <summary>
        /// 命令提供程序作者
        /// </summary>
        string Author { get; }
        #endregion

        /// <summary>
        /// 保存命令事件
        /// </summary>
        /// <param name="ontology"></param>
        /// <param name="command"></param>
        ProcessResult SaveCommand(OntologyDescriptor ontology, MessageEntity command);

        /// <summary>
        /// 批量保存本地事件
        /// </summary>
        /// <param name="ontology">本体</param>
        /// <param name="commands"></param>
        ProcessResult SaveCommands(OntologyDescriptor ontology, MessageEntity[] commands);

        /// <summary>
        /// 判断给定标识的命令是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandType"></param>
        /// <param name="ontology"></param>
        /// <param name="id"></param>
        /// <param name="isDumb"></param>
        /// <returns></returns>
        ProcessResult DeleteCommand(MessageTypeKind commandType, OntologyDescriptor ontology, Guid id, bool isDumb);

        /// <summary>
        /// 获取给定条目的给定类型的命令
        /// </summary> 
        /// <param name="commandType"></param>
        /// <param name="ontology"></param>
        /// <param name="n">条目数</param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <returns>待分发命令，如果待分发命令不足指定的条目数返回的结果将不足指定的条目数</returns>
        IList<MessageEntity> GetTopNCommands(MessageTypeKind commandType, OntologyDescriptor ontology, int n, string sortField, string sortOrder);

        /// <summary>
        /// 根据ID获取命令
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="ontology">本体</param>
        /// <param name="id">命令标识</param>
        /// <returns></returns>
        MessageEntity GetCommand(MessageTypeKind commandType, OntologyDescriptor ontology, Guid id);

        /// <summary>
        /// 根据节点分页获取命令
        /// </summary>
        /// <typeparam name="T">命令类型参数</typeparam>
        /// <param name="commandType"></param>
        /// <param name="ontology">本体</param>
        /// <param name="catalogCode">目录码</param>
        /// <param name="actionCode">动作码，空值表示忽略本查询条件</param>
        /// <param name="nodeId">节点标识，空值表示忽略本查询条件</param>
        /// <param name="localEntityId">本地实体标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortOrder">排序方向</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        IList<MessageEntity> GetPlistCommands(
            MessageTypeKind commandType, OntologyDescriptor ontology, string catalogCode,
            string actionCode, Guid? nodeId, string localEntityId, int pageIndex,
            int pageSize, string sortField, string sortOrder, out Int64 total);
    }
}
